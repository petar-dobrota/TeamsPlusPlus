using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Chat.Web
{
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly string reverseProxyBaseUri;
        private readonly StatelessServiceContext serviceContext;
        private readonly EventCounter requestCounter;

        /// <summary>
        /// Constructs a reverse proxy URL for a given service.
        /// </summary>
        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"{this.reverseProxyBaseUri}{serviceName.AbsolutePath}");
        }

        private long GetPartitionKey(string name)
        {
            return Util.Hash64(name);
        }

        public RoomController(HttpClient httpClient, StatelessServiceContext context, FabricClient fabricClient, EventCounter requestCounter)
        {
            this.fabricClient = fabricClient;
            this.httpClient = httpClient;
            this.serviceContext = context;
            this.reverseProxyBaseUri = Environment.GetEnvironmentVariable("ReverseProxyBaseUri");
            this.requestCounter = requestCounter;
        }

        [HttpGet("{room}")]
        public async Task<ActionResult> GetRoom(string room)
        {
            try
            {
                requestCounter.SignalEventOccured();

                Uri serviceName = Web.GetChatDataServiceName(this.serviceContext);
                Uri proxyAddress = GetProxyAddress(serviceName);
                long partitionKey = GetPartitionKey(room);

                string proxyUrl = $"{proxyAddress}/api/room/{room}?PartitionKey={partitionKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return new StatusCodeResult((int)response.StatusCode);
                    }

                    var resp = await response.Content.ReadAsStringAsync();
                    return Ok(resp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost("{room}")]
        public async Task<ActionResult> SendMessage(string room, string user, [FromBody] string message)
        {
            try
            {
                requestCounter.SignalEventOccured();

                Uri serviceName = Web.GetChatDataServiceName(this.serviceContext);
                Uri proxyAddress = GetProxyAddress(serviceName);
                long partitionKey = GetPartitionKey(room);

                string proxyUrl = $"{proxyAddress}/api/room/{room}/?user={user}&PartitionKey={partitionKey}&PartitionKind=Int64Range";

                StringContent content = new StringContent($"\"{message}\"".ToString(), Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    return new ContentResult()
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = await response.Content.ReadAsStringAsync()
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

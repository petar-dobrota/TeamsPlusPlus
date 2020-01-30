

using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly string reverseProxyBaseUri;
        private readonly StatelessServiceContext serviceContext;

        public UserController(HttpClient httpClient, StatelessServiceContext context, FabricClient fabricClient)
        {
            this.fabricClient = fabricClient;
            this.httpClient = httpClient;
            this.serviceContext = context;
            this.reverseProxyBaseUri = Environment.GetEnvironmentVariable("ReverseProxyBaseUri");
        }

        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"{this.reverseProxyBaseUri}{serviceName.AbsolutePath}");
        }

        private long GetPartitionKey(string username)
        {
            return Util.Hash64(username);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserInfo(string userId)
        {
            Uri serviceName = Web.GetChatDataServiceName(this.serviceContext);
            Uri proxyAddress = GetProxyAddress(serviceName);
            long partitionKey = GetPartitionKey(userId);

            string proxyUrl = $"{proxyAddress}/api/user/{userId}?PartitionKey={partitionKey}&PartitionKind=Int64Range";

            using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new StatusCodeResult((int) response.StatusCode);
                }

                var resp = await response.Content.ReadAsStringAsync();
                return Ok(resp);
            }
        }

        [HttpPut("{userId}/joinRoom")]
        public async Task<ActionResult> JoinRoom(string userId, [FromQuery] string roomName)
        {
            Uri serviceName = Web.GetChatDataServiceName(this.serviceContext);
            Uri proxyAddress = GetProxyAddress(serviceName);
            long partitionKey = GetPartitionKey(userId);

            string proxyUrl = $"{proxyAddress}/api/user/{userId}/joinRoom?roomName={roomName}&PartitionKey={partitionKey}&PartitionKind=Int64Range";

            using (HttpResponseMessage response = await this.httpClient.PutAsync(proxyUrl, null))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new StatusCodeResult((int)response.StatusCode);
                }

                var resp = await response.Content.ReadAsStringAsync();
                return Ok(resp);
            }
        }
    }
}
using HelloStatefulWorld;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alphabet.Web
{
    internal class AddUserProxy : HttpHandler
    {
        private readonly ServicePartitionResolver servicePartitionResolver = new ServicePartitionResolver();//ServicePartitionResolver.GetDefault();

        public AddUserProxy() : base(null)
        {
        }

        public async override Task ProcessInternalRequest(HttpListenerContext context, CancellationToken cancelRequest)
        {
            String output = null;
            try
            {
                string lastname = context.Request.QueryString["lastname"];

                char firstLetterOfLastName = lastname.First();
                ServicePartitionKey partitionKey = new ServicePartitionKey(Char.ToUpper(firstLetterOfLastName) - 'A');

                ResolvedServicePartition partition = await this.servicePartitionResolver.ResolveAsync(new Uri("fabric:/SF_Test/HelloStatefulWorld"), partitionKey, cancelRequest);
                ResolvedServiceEndpoint rep = partition.GetEndpoint();
                
                Newtonsoft.Json.Linq.JObject addresses = Newtonsoft.Json.Linq.JObject.Parse(rep.Address);
                string primaryReplicaAddress = (string)addresses["Endpoints"].First();

                UriBuilder primaryReplicaUriBuilder = new UriBuilder(primaryReplicaAddress);
                primaryReplicaUriBuilder.Query = "lastname=" + lastname;

                string result = await HttpGetAsync(primaryReplicaUriBuilder.Uri.ToString());

                output = String.Format(
                    "Result: {0}. <p>Partition key: '{1}' generated from the first letter '{2}' of input value '{3}'. <br>Processing service partition ID: {4}. <br>Processing service replica address: {5}",
                    result,
                    partitionKey,
                    firstLetterOfLastName,
                    lastname,
                    partition.Info.Id,
                    primaryReplicaAddress);
            }
            catch (Exception ex) { output = ex.Message; }

            using (var response = context.Response)
            {
                if (output != null)
                {
                    byte[] outBytes = Encoding.UTF8.GetBytes(output);
                    response.OutputStream.Write(outBytes, 0, outBytes.Length);
                }
            }
        }

        public async Task<string> HttpGetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}

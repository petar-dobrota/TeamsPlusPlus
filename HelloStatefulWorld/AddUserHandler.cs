using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloStatefulWorld
{
    internal class AddUserHandler : HttpHandler
    {
        public AddUserHandler(IReliableStateManager stateManager) : base(stateManager)
        {
        }

        public override async Task ProcessInternalRequest(HttpListenerContext context, CancellationToken cancelRequest)
        {
            string output = null;
            string user = context.Request.QueryString["lastname"].ToString();

            try
            {
                output = await this.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }

            using (HttpListenerResponse response = context.Response)
            {
                if (output != null)
                {
                    byte[] outBytes = Encoding.UTF8.GetBytes(output);
                    response.OutputStream.Write(outBytes, 0, outBytes.Length);
                }
            }
        }

        private async Task<string> AddUserAsync(string user)
        {
            IReliableDictionary<String, String> dictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<String, String>>("dictionary");

            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                bool addResult = await dictionary.TryAddAsync(tx, user.ToUpperInvariant(), user);

                await tx.CommitAsync();
                
                return String.Format(
                    "User {0} {1}",
                    user,
                    addResult ? "successfully added" : "already exists");
            }
        }
    }
}

using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloStatefulWorld
{
    abstract class HttpHandler
    {
        protected HttpHandler(IReliableStateManager stateManager)
        {
            StateManager = stateManager;
        }

        protected readonly IReliableStateManager StateManager;

        public abstract Task ProcessInternalRequest(HttpListenerContext context, CancellationToken cancelRequest);
        
    }
}

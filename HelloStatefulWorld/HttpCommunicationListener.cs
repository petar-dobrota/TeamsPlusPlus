using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace HelloStatefulWorld
{
    class HttpCommunicationListener : ICommunicationListener
    {
        private readonly string uriPrefix;
        private readonly string uriPublished;
        private readonly HttpHandler httpHandler;

        private readonly HttpListener httpListener;
        private Thread worker;

        public HttpCommunicationListener(string uriPrefix, string uriPublished, HttpHandler httpHandler)
        {
            this.uriPrefix = uriPrefix;
            this.uriPublished = uriPublished;
            this.httpHandler = httpHandler;
            this.httpListener = new HttpListener();
        }

        public void Abort()
        {
            httpListener.Stop();
            worker.Abort();
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            Abort();
            return Task.CompletedTask;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
                
            httpListener.Prefixes.Add(uriPrefix);
            httpListener.Start();

            worker = new Thread(() =>
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    HttpListenerContext context = httpListener.GetContext();

                    httpHandler.ProcessInternalRequest(context, cancellationToken).ContinueWith(t => {
                        //var ostream = context.Response.OutputStream;
                        //if (ostream!=null) ostream.Close();                        
                    });
                }
            });
            worker.Start();

            // the string returned here will be published in the Naming Service.
            return Task.FromResult(this.uriPublished);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace HelloStatefulWorld
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal class HelloStatefulWorld : StatefulService
    {
        public HelloStatefulWorld(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            
            return new ServiceReplicaListener[] {
                new ServiceReplicaListener(ctx => CreateInternalListener(ctx)),
                // TODO: Add more listeners
            };
        }

        private ICommunicationListener CreateInternalListener(ServiceContext context)
        {
            EndpointResourceDescription internalEndpoint = context.CodePackageActivationContext.GetEndpoint("ProcessingServiceEndpoint");
            
            string uriPrefix = String.Format(
                   "{0}://+:{1}/{2}/{3}-{4}/",
                   internalEndpoint.Protocol,
                   internalEndpoint.Port,
                   context.PartitionId,
                   context.ReplicaOrInstanceId,
                   Guid.NewGuid());

            string nodeIP = FabricRuntime.GetNodeContext().IPAddressOrFQDN;

            // for srv discovery
            string uriPublished = uriPrefix.Replace("+", nodeIP);
            
            return new HttpCommunicationListener(uriPrefix, uriPublished, new AddUserHandler(StateManager));
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                using (var tx = this.StateManager.CreateTransaction())
                {
                    
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);
                    
                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.

                    // await tx.CommitAsync();
                    var task = tx.CommitAsync();
                    await task;
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}

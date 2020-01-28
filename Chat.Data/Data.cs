using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Chat.Data.Domain;
using Chat.Data.Repository;

namespace Chat.Data
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Data : StatefulService
    {
        private readonly RoomService roomService;

        public Data(StatefulServiceContext context)
            : base(context)
        {
            roomService = new RoomService(new ChatRoomReliableRepository(StateManager));
        }

        protected override Task OnOpenAsync(ReplicaOpenMode openMode, CancellationToken cancellationToken)
        {
            new Thread(async () => {

                while (!cancellationToken.IsCancellationRequested)
                {

                    try
                    {
                        if (Partition != null && Partition.ReadStatus != PartitionAccessStatus.ReconfigurationPending)
                        {
                            int roomCount = await roomService.GetRoomCountAsync(cancellationToken);
                            Partition.ReportLoad(new List<LoadMetric> { new LoadMetric("RoomCount", roomCount) });
                        }
                    }
                    catch (Exception) { }

                    Thread.Sleep(1000);
                }

            }).Start();

            return base.OnOpenAsync(openMode, cancellationToken);
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[]
            {
                new ServiceReplicaListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");
                        
                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton(serviceContext)
                                            .AddSingleton(new FabricClient())
                                            .AddSingleton(roomService)
                                            .AddSingleton(this.StateManager))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseUniqueServiceUrl)
                                    .UseUrls(url)
                                    .Build();
                    }), "", true)
            };

        }

        internal static Uri GetChatDataServiceName(StatefulServiceContext ctx)
        {
            return new Uri($"{ctx.CodePackageActivationContext.ApplicationName}/Chat.Data");
        }
    }
}

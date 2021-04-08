using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Chess.Queue.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Chess.Queue.Move
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static async Task Main(string[] args)
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                await ServiceRuntime.RegisterServiceAsync("Chess.Queue.MoveType",
                    context => CreateService(args, context));

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, nameof(Move));

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static Move CreateService(string[] args, StatefulServiceContext context)
        {
            try
            {
                return Host.CreateDefaultBuilder(args)
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(context);
                        services.AddMoveQueueService();

                        services.AddSingleton<Move>();
                    })
                    .Build()
                    .Services
                    .GetService<Move>();
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}

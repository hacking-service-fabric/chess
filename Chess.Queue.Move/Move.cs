using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chess.Queue.Common.Interfaces;
using Chess.Queue.Common.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Chess.Queue.Move
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Move : StatefulService, IMoveQueueService
    {
        public Move(StatefulServiceContext context)
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
            => this.CreateServiceRemotingReplicaListeners();

        public async Task Enqueue(MoveModel payload)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<MoveModel>>("MoveQueue");

            using var tx = StateManager.CreateTransaction();
            await queue.EnqueueAsync(tx, payload);
            await tx.CommitAsync();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<MoveModel>>("MoveQueue");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    using (var tx = this.StateManager.CreateTransaction())
                    {
                        var maybeMove = await queue.TryDequeueAsync(tx);
                        if (maybeMove.HasValue)
                        {
                            var move = maybeMove.Value;

                            ServiceEventSource.Current.Message(
                                "{0} (takes: {1}) from {2} to {3}, check: {4}, checkmate: {5}",
                                move.Piece,
                                move.Takes ? "yes" : "no",
                                move.FromPosition,
                                move.ToPosition,
                                move.Check ? "yes" : "no",
                                move.Checkmate ? "yes" : "no");
                        }

                        await tx.CommitAsync();
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
                catch (Exception e)
                {
                    ServiceEventSource.Current.ServiceProcessingFailed(e.ToString());
                    await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
                }
            }
        }
    }
}

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

namespace Chess.Queue.SMS
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SMS : StatefulService, ISmsQueueService
    {
        private readonly IChessMoveParser _chessMoveParser;

        public SMS(IChessMoveParser chessMoveParser, StatefulServiceContext context)
            : base(context)
        {
            _chessMoveParser = chessMoveParser;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, SMS Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
            => this.CreateServiceRemotingReplicaListeners();

        public async Task Enqueue(SmsModel payload)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<SmsModel>>("SMSQueue");

            using var tx = StateManager.CreateTransaction();
            await queue.EnqueueAsync(tx, payload);
            await tx.CommitAsync();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when SMS Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<SmsModel>>("SMSQueue");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var message = await queue.TryDequeueAsync(tx);
                    if (message.HasValue && _chessMoveParser.TryParse(message.Value.TextContent, out var move))
                    {
                        ServiceEventSource.Current.Message("{0} => {1} {2} from {3}{4} to {5}{6}",
                            message.Value.TextContent,
                            move.Piece,
                            move.Takes ? "takes" : "",
                            move.StartingFile,
                            (int?)move.StartingRank,
                            move.DestinationFile,
                            (int)move.DestinationRank);
                    }

                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}

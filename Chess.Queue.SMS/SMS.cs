using Chess.Queue.Common;
using Chess.Queue.Common.Interfaces;
using Chess.Queue.Common.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Queue.SMS
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SMS : StatefulService, ISmsQueueService
    {
        private readonly IChessMoveParser _chessMoveParser;
        private readonly IMoveQueueServiceAccessor _moveQueueServiceAccessor;

        public SMS(IChessMoveParser chessMoveParser, IMoveQueueServiceAccessor moveQueueServiceAccessor, StatefulServiceContext context)
            : base(context)
        {
            _chessMoveParser = chessMoveParser;
            _moveQueueServiceAccessor = moveQueueServiceAccessor;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, SMS Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        [ExcludeFromCodeCoverage]
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

                try
                {
                    using (var tx = this.StateManager.CreateTransaction())
                    {
                        var message = await queue.TryDequeueAsync(tx);
                        if (message.HasValue && _chessMoveParser.TryParse(message.Value.TextContent, out var move))
                        {
                            await _moveQueueServiceAccessor.GetInstance(message.Value.Conversation.RecipientPhoneNumbers)
                                .Enqueue(move);
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

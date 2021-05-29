using Chess.Data.Common.Models.V1;
using Chess.Queue.Common;
using Chess.Queue.SMS.Models;
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
using Chess.Data.Common;

namespace Chess.Queue.SMS
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SMS : StatefulService, ISmsQueueService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMoveQueueService _moveQueueServiceAccessor;

        public SMS(IConversationRepository conversationRepository, IMoveQueueService moveQueueServiceAccessor, StatefulServiceContext context)
            : base(context)
        {
            _conversationRepository = conversationRepository;
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

        public async Task Enqueue(ConversationDto conversation, MessageDto message)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<SmsModel>>("SMSQueue");

            using var tx = StateManager.CreateTransaction();
            await queue.EnqueueAsync(tx, new SmsModel
            {
                Conversation = conversation,
                Message = message
            });
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
                    var hasMessage = false;
                    using (var tx = this.StateManager.CreateTransaction())
                    {
                        var message = await queue.TryDequeueAsync(tx);

                        if (message.HasValue)
                        {
                            var conversation = await _conversationRepository.GetConversation(message.Value.Conversation);
                            var messageId = await conversation.WriteMessage(message.Value.Message);

                            if (string.IsNullOrEmpty(message.Value.Message.MediaUrl))
                            {
                                await _moveQueueServiceAccessor.Enqueue(message.Value.Conversation, messageId);
                            }

                            hasMessage = true;
                        }
                        
                        await tx.CommitAsync();
                    }

                    if (!hasMessage)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }
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

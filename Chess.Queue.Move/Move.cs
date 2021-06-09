using Chess.Data.Common;
using Chess.Data.Common.Models.V1;
using Chess.Queue.Common;
using Chess.Queue.Move.Model;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Queue.Move
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Move : StatefulService, IMoveQueueService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IReplyQueueService _replyQueueService;

        public Move(StatefulServiceContext context,
            IConversationRepository conversationRepository,
            IReplyQueueService replyQueueService)
            : base(context)
        {
            _conversationRepository = conversationRepository;
            _replyQueueService = replyQueueService;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
            => this.CreateServiceRemotingReplicaListeners();

        public async Task Enqueue(ConversationDto conversation, ushort conversationMessageId)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<MessageModel>>("MoveQueue");

            using var tx = StateManager.CreateTransaction();
            await queue.EnqueueAsync(tx, new MessageModel
            {
                Conversation = conversation,
                ConversationMessageId = conversationMessageId
            });
            await tx.CommitAsync();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var queue = await StateManager.GetOrAddAsync<IReliableQueue<MessageModel>>("MoveQueue");

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
                            
                            var conversation = await _conversationRepository.GetConversation(move.Conversation);
                            var game = await conversation.GetGame();

                            var message = await conversation.GetMessage(move.ConversationMessageId);
                            var response = await game.TryMove(message);

                            await _replyQueueService.Enqueue(move.Conversation, response);
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

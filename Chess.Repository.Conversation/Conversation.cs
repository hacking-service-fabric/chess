using Chess.Data.Common;
using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using PhoneNumbers;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Repository.Conversation
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Conversation : StatefulService, IConversationRepository
    {
        public Conversation(StatefulServiceContext context)
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

        public async Task<IConversation> GetConversation(ConversationDto conversation)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            using var tx = StateManager.CreateTransaction();

            var conversations = await StateManager.GetOrAddAsync<IReliableDictionary<string, ActorId>>(tx,
                phoneUtil.Format(conversation.HostPhoneNumber, PhoneNumberFormat.E164));

            var actorId = string.Join("/", conversation.PhoneNumbers
                .Select(p => phoneUtil.Format(p, PhoneNumberFormat.E164))
                .OrderBy(p => p));
            var conversationId = await conversations.GetOrAddAsync(tx, actorId, key => new ActorId(key));

            await tx.CommitAsync();
            return ActorProxy.Create<IConversation>(conversationId);
        }
    }
}

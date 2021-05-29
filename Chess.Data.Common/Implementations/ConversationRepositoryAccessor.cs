using Chess.Data.Common.Models.V1;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using PhoneNumbers;

namespace Chess.Data.Common.Implementations
{
    class ConversationRepositoryAccessor: IConversationRepository
    {
        public Task<IConversation> GetConversation(ConversationDto conversation)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();

            var host = phoneUtil.Format(conversation.HostPhoneNumber, PhoneNumberFormat.INTERNATIONAL);
            var recipients = conversation.PhoneNumbers
                .OrderBy(p => p.CountryCode)
                .ThenBy(p => p.NationalNumber)
                .Select(p => phoneUtil.Format(p, PhoneNumberFormat.INTERNATIONAL));

            var actor = ActorProxy.Create<IConversation>(
                new ActorId($"conversation/{host}/{string.Join("/", recipients)}"));

            return Task.FromResult(actor);
        }
    }
}

using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using PhoneNumbers;
using System;
using System.Threading.Tasks;

namespace Chess.Queue.Common.Implementations
{
    public class ReplyQueueServiceAccessor: IReplyQueueService
    {
        private IReplyQueueService GetInstance(PhoneNumber hostPhoneNumber)
        {
            var key = PhoneNumberUtil.GetInstance().Format(hostPhoneNumber, PhoneNumberFormat.INTERNATIONAL);

            return ServiceProxy.Create<IReplyQueueService>(
                new Uri("fabric:/Chess.App/Chess.Queue.Reply"),
                new ServicePartitionKey(key));
        }

        public async Task Enqueue(ConversationDto conversation, MoveResultDtoBase message)
        {
            if (message is not null && message is not NoReplyDto)
            {
                await GetInstance(conversation.HostPhoneNumber).Enqueue(conversation, message);
            }
        }
    }
}

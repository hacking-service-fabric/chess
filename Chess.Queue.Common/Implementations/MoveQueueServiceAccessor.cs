using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Queue.Common.Implementations
{
    public class MoveQueueServiceAccessor: IMoveQueueService
    {
        public IMoveQueueService GetInstance(IEnumerable<PhoneNumber> recipients)
        {
            var key = (int)recipients
                .Where(r => r.HasNationalNumber)
                .Select(r => r.NationalNumber)
                .Min() % 100;

            return ServiceProxy.Create<IMoveQueueService>(
                new Uri("fabric:/Chess.App/Chess.Queue.Move"),
                new ServicePartitionKey(key));
        }

        public async Task Enqueue(ConversationDto conversation, MessageDto message)
        {
            await GetInstance(conversation.PhoneNumbers).Enqueue(conversation, message);
        }
    }
}

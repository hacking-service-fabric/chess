using Chess.Queue.Common.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using PhoneNumbers;

namespace Chess.Queue.Common.Implementations
{
    public class MoveQueueServiceAccessor: IMoveQueueServiceAccessor
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
    }
}

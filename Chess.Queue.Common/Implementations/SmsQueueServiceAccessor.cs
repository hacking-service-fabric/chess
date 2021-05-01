using Chess.Queue.Common.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using PhoneNumbers;

namespace Chess.Queue.Common.Implementations
{
    public class SmsQueueServiceAccessor: ISmsQueueServiceAccessor
    {
        public ISmsQueueService GetInstance(IEnumerable<PhoneNumber> recipients)
        {
            var key = (int)recipients
                .Where(r => r.HasNationalNumber)
                .Select(r => r.NationalNumber)
                .Min() % 100;

            return ServiceProxy.Create<ISmsQueueService>(
                new Uri("fabric:/Chess.App/Chess.Queue.SMS"),
                new ServicePartitionKey(key));
        }
    }
}

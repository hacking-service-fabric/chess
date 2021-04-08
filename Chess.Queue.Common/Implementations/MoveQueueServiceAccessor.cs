using Chess.Queue.Common.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Queue.Common.Implementations
{
    public class MoveQueueServiceAccessor: IMoveQueueServiceAccessor
    {
        public IMoveQueueService GetInstance(IEnumerable<int> recipients)
        {
            return ServiceProxy.Create<IMoveQueueService>(
                new Uri("fabric:/Chess.App/Chess.Queue.Move"),
                new ServicePartitionKey(recipients.Min() % 100));
        }
    }
}

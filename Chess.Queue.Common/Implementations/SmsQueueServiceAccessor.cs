﻿using Chess.Queue.Common.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Queue.Common.Implementations
{
    public class SmsQueueServiceAccessor: ISmsQueueServiceAccessor
    {
        public ISmsQueueService GetInstance(IEnumerable<int> recipients)
        {
            return ServiceProxy.Create<ISmsQueueService>(
                new Uri("fabric:/Chess.App/Chess.Queue.SMS"),
                new ServicePartitionKey(recipients.Min() % 100));
        }
    }
}

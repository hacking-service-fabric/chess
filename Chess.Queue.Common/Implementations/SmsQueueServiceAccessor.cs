using Chess.Queue.Common.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;

namespace Chess.Queue.Common.Implementations
{
    public class SmsQueueServiceAccessor: ISmsQueueServiceAccessor
    {
        public ISmsQueueService GetInstance()
        {
            return ServiceProxy.Create<ISmsQueueService>(
                new Uri("fabric:/Chess.App/Chess.Queue.SMS"));
        }
    }
}

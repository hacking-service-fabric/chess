using Chess.Queue.Common.Interfaces;
using System.Collections.Generic;

namespace Chess.Queue.Common
{
    public interface ISmsQueueServiceAccessor
    {
        ISmsQueueService GetInstance(IEnumerable<int> recipients);
    }
}

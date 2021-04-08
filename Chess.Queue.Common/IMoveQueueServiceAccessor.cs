using Chess.Queue.Common.Interfaces;
using System.Collections.Generic;

namespace Chess.Queue.Common
{
    public interface IMoveQueueServiceAccessor
    {
        IMoveQueueService GetInstance(IEnumerable<int> recipients);
    }
}

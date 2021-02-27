using Chess.Queue.Common.Interfaces;

namespace Chess.Queue.Common
{
    public interface ISmsQueueServiceAccessor
    {
        ISmsQueueService GetInstance();
    }
}

using Chess.Queue.Common.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Chess.Queue.Common.Interfaces
{
    public interface IMoveQueueService: IService
    {
        Task Enqueue(MoveModel payload);
    }
}

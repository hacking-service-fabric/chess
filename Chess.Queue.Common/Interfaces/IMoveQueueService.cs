using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Chess.Queue.Common.Interfaces
{
    public interface IMoveQueueService: IService
    {
        Task Enqueue(ConversationDto conversation, MoveDtoBase payload);
    }
}

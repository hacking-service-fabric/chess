using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Chess.Queue.Common
{
    public interface IMoveQueueService: IService
    {
        Task Enqueue(ConversationDto conversation, ushort conversationMessageId);
    }
}

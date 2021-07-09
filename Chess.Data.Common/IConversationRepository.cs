using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversationRepository: IService
    {
        Task<IConversation> GetConversation(ConversationDto conversation);
    }
}
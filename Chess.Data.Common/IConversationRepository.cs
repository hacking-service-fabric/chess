using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Actors;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversationRepository: IActor
    {
        Task<IConversation> GetConversation(ConversationDto conversation);
    }
}
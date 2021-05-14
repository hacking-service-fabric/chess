using Chess.Data.Common.Models.V1;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversationRepository
    {
        Task<IConversation> GetConversation(ConversationDto conversation);
    }
}
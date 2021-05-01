using System.Threading.Tasks;
using Chess.Data.Common.Models;
using PhoneNumbers;

namespace Chess.Data.Common
{
    public interface IConversationRepository
    {
        Task<IConversation> GetConversation(Conversation conversation);
    }
}
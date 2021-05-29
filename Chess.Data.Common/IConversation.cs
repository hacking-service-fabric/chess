using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Actors;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversation: IActor
    {
        Task<IGame> GetGame();

        Task<ushort> WriteMessage(MessageDto message);
        Task<MessageDto> GetMessage(ushort messageId);
    }
}
using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversation: IActor
    {
        Task<IGame> GetGame();

        Task<MessageDto> GetMessageAt(DateTime time);
        Task<MessageDto> GetMessageBefore(MessageDto message);
    }
}
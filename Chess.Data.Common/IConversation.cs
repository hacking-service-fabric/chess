using Chess.Data.Common.Models;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversation: IActor
    {
        Task<IGame> GetCurrentGame();
        Task<IGame> StartNewGame();

        Task<Message> GetMessageAt(DateTime time);
        Task<Message> GetMessageBefore(Message message);
    }
}
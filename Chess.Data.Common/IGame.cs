using Microsoft.ServiceFabric.Actors;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IGame: IActor
    {
        Task Move();
    }
}
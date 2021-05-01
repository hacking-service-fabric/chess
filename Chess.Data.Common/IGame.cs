using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IGame
    {
        Task Move();
    }
}
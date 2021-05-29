using Chess.Queue.Common.Models;

namespace Chess.Queue.Move
{
    public interface IChessMoveParser
    {
        bool TryParse(string message, out MoveModel result);
    }
}

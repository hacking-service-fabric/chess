using Chess.Queue.Common.Models;

namespace Chess.Queue.SMS
{
    public interface IChessMoveParser
    {
        bool TryParse(string message, out MoveModel result);
    }
}

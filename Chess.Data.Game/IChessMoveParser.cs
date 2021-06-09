using Chess.Data.Common.Models.V1;

namespace Chess.Data.Game
{
    public interface IChessMoveParser
    {
        bool TryParse(string message, out MoveDtoBase result);
    }
}

using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Interfaces;

namespace Chess.Data.Game.Implementations
{
    public class MoveResignParser: IChessMoveParser<MoveResignDto>
    {
        public bool TryParse(string message, out MoveResignDto result)
        {
            if (message?.Trim().ToLower() == "gg")
            {
                result = new MoveResignDto();
                return true;
            }

            result = null;
            return false;
        }
    }
}

using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Interfaces;
using System.Text.RegularExpressions;

namespace Chess.Data.Game.Implementations
{
    public class MoveCastleParser: IChessMoveParser<MoveCastleDto>
    {
        private static readonly Regex CastlePattern = new Regex(
            "^O-?O-?(?<isQueenSide>O)?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out MoveCastleDto result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            var castleMatch = CastlePattern.Match(message.Trim());
            if (!castleMatch.Success)
            {
                return false;
            }

            result = new MoveCastleDto
            {
                KingSide = !castleMatch.Groups["isQueenSide"].Success
            };

            return true;
        }
    }
}

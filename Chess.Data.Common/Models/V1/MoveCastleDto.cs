using System.Text.RegularExpressions;

namespace Chess.Data.Common.Models.V1
{
    public class MoveCastleDto: MoveDtoBase
    {
        private static readonly Regex CastlePattern = new Regex(
            "^O-?O-?(?<isQueenSide>O)?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool KingSide { get; set; }

        public static bool TryParse(string message, out MoveCastleDto result)
        {
            var castleMatch = CastlePattern.Match(message);
            if (castleMatch.Success)
            {
                result = new MoveCastleDto
                {
                    KingSide = !castleMatch.Groups["isQueenSide"].Success
                };
                return true;
            }

            result = null;
            return false;
        }
    }
}

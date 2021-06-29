using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Interfaces;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Data.Game.Implementations
{
    public class MovePieceParser: IChessMoveParser<MovePieceDto>
    {
        private static readonly Regex MovePattern = new Regex(
            "^(?<piece>[KQRBN])?(?<fromFile>[a-h])?(?<fromRank>[1-8])?(?<capture>[x:])?(?<toFile>[a-h])(?<toRank>[1-8])?(?<check>\\+)?(?<checkmate>[x#])?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out MovePieceDto result)
        {
            result = null;
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }

            var match = MovePattern.Match(message.Trim());
            if (!match.Success)
            {
                return false;
            }

            result = new MovePieceDto
            {
                Piece = match.Groups["piece"].Captures.FirstOrDefault()?.Value?.ToUpper() switch
                {
                    "K" => Piece.King,
                    "Q" => Piece.Queen,
                    "R" => Piece.Rook,
                    "B" => Piece.Bishop,
                    "N" => Piece.Knight,
                    _ => Piece.Empty
                },
                Captures = match.Groups["capture"].Success,
                ToFile = (BoardFile)(match.Groups["toFile"].Captures[0].Value.ToLower()[0] - 'a'),
                Check = match.Groups["check"].Success,
                Checkmate = match.Groups["checkmate"].Success
            };

            if (match.Groups["toRank"].Success)
            {
                result.ToRank = (BoardRank)(match.Groups["toRank"].Captures[0].Value[0] - '1');
            }

            if (match.Groups["fromFile"].Success)
            {
                result.FromFile = (BoardFile)(match.Groups["fromFile"].Captures[0].Value.ToLower()[0] - 'a');
            }

            if (match.Groups["fromRank"].Success)
            {
                result.FromRank = (BoardRank)(match.Groups["fromRank"].Captures[0].Value[0] - '1');
            }

            return true;
        }
    }
}

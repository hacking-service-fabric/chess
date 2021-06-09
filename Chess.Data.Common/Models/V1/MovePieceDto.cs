using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Data.Common.Models.V1
{
    public class MovePieceDto: MoveDtoBase
    {
        private static readonly Regex MovePattern = new Regex(
            "^(?<piece>[KQRBN])?(?<fromFile>[a-h])?(?<fromRank>[1-8])?(?<capture>[x:])?(?<toFile>[a-h])(?<toRank>[1-8])?(?<check>\\+)?(?<checkmate>[x#])?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Piece Piece { get; set; }
        public bool Captures { get; set; }

        public BoardFile? FromFile { get; set; }
        public BoardRank? FromRank { get; set; }

        public BoardFile ToFile { get; set; }
        public BoardRank? ToRank { get; set; }

        public bool Checks { get; set; }
        public bool Checkmates { get; set; }

        public static bool TryParse(string message, out MovePieceDto result)
        {
            result = null;

            var match = MovePattern.Match(message);
            if (!match.Success)
            {
                return false;
            }

            result = new MovePieceDto
            {
                Piece = match.Groups["piece"].Captures.FirstOrDefault()?.Value switch
                {
                    "K" => Piece.King,
                    "Q" => Piece.Queen,
                    "R" => Piece.Rook,
                    "B" => Piece.Bishop,
                    "N" => Piece.Knight,
                    _ => Piece.Empty
                },
                Captures = match.Groups["capture"].Success,
                ToFile = (BoardFile) (match.Groups["toFile"].Captures[0].Value.ToLower()[0] - 'a'),
                ToRank = (BoardRank) (match.Groups["toRank"].Captures[0].Value[0] - '1'),
                Checks = match.Groups["check"].Success,
                Checkmates = match.Groups["checkmate"].Success
            };

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

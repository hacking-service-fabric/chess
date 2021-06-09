using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Data.Common.Models.V1
{
    public class MovePromotionDto: MoveDtoBase
    {
        private static readonly Regex MovePattern = new Regex(
            "^(?<fromFile>[a-h])?(?<fromRank>[1-8])?(?<toFile>[a-h])(?<toRank>[1-8])[/=(]?(?<promotion>[QRBN])\\)?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public BoardFile? FromFile { get; set; }
        public BoardRank? FromRank { get; set; }
        public bool Captures { get; set; }

        public BoardFile ToFile { get; set; }
        public BoardRank ToRank { get; set; }

        public Piece PromotionToPiece { get; set; }
        
        public static bool TryParse(string message, out MovePromotionDto result)
        {
            result = null;

            var match = MovePattern.Match(message);
            if (!match.Success)
            {
                return false;
            }

            result = new MovePromotionDto
            {
                PromotionToPiece = match.Groups["promotion"].Captures.FirstOrDefault()?.Value switch
                {
                    "Q" => Piece.Queen,
                    "R" => Piece.Rook,
                    "B" => Piece.Bishop,
                    "N" => Piece.Knight,
                    _   => Piece.Empty
                },
                ToFile = (BoardFile)(match.Groups["toFile"].Captures[0].Value.ToLower()[0] - 'a'),
                ToRank = (BoardRank)(match.Groups["toRank"].Captures[0].Value[0] - '1')
            };

            if (match.Groups["fromFile"].Success)
            {
                result.FromFile = (BoardFile) (match.Groups["fromFile"].Captures[0].Value.ToLower()[0] - 'a');
            }

            if (match.Groups["fromRank"].Success)
            {
                result.FromRank = (BoardRank) (match.Groups["fromRank"].Captures[0].Value[0] - '1');
            }

            return true;
        }
    }
}

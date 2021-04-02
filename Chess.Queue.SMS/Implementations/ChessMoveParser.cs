using Chess.Queue.Common.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Queue.SMS.Implementations
{
    public class ChessMoveParser: IChessMoveParser
    {
        private static readonly Regex CastlePattern = new Regex(
            "^[0O]-?[0O]-?(?<isQueenSide>[0O]?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex MovePattern = new Regex(
            "^(?<piece>[KQRBN]?)(?<fromPosition>[a-h]?[1-8]?)(?<taking>x?)(?<toPosition>[a-h][1-8]?)(?<promotion>[QRBN]?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out MoveModel result)
        {
            result = null;

            var castleMatch = CastlePattern.Match(message);
            if (castleMatch.Success)
            {
                result = new MoveModel
                {
                    Piece = string.IsNullOrEmpty(castleMatch.Groups["isQueenSide"].Captures.FirstOrDefault()?.Value)
                        ? MovePiece.KingSideCastle
                        : MovePiece.QueenSideCastle
                };
                return true;
            }

            var moveMatch = MovePattern.Match(message);
            if (!moveMatch.Success)
            {
                return false;
            }

            var piece = moveMatch.Groups["piece"].Captures.FirstOrDefault()?.Value switch
            {
                "K" => MovePiece.King,
                "Q" => MovePiece.Queen,
                "R" => MovePiece.Rook,
                "B" => MovePiece.Bishop,
                "N" => MovePiece.Knight,
                _ => moveMatch.Groups["promotion"].Captures.FirstOrDefault()?.Value switch
                {
                    "Q" => MovePiece.PawnToQueen,
                    "R" => MovePiece.PawnToRook,
                    "B" => MovePiece.PawnToBishop,
                    "N" => MovePiece.PawnToKnight,
                    _   => MovePiece.Unknown
                }
            };
            var takes = !string.IsNullOrEmpty(moveMatch.Groups["taking"].Captures.FirstOrDefault()?.Value);
            var fromPosition = ParseMovePosition(moveMatch.Groups["fromPosition"].Captures.FirstOrDefault()?.Value);
            var toPosition = ParseMovePosition(moveMatch.Groups["toPosition"].Captures.FirstOrDefault()?.Value);
            
            result = new MoveModel
            {
                Piece = piece,
                Takes = takes,
                FromPosition = fromPosition,
                ToPosition = toPosition,
                Check = false,
                Checkmate = false
            };
            return true;
        }

        public MovePosition ParseMovePosition(string position)
        {
            if (string.IsNullOrEmpty(position))
            {
                return MovePosition.FileUnknown | MovePosition.RankUnknown;
            }

            var result = MovePosition.Empty;

            var file = position.ToLower().First() - 'a';
            var rank = position.ToLower().Last() - '1';

            result |= file >= 0 && file < 8
                ? (MovePosition) file
                : MovePosition.FileUnknown;

            result |= rank >= 0 && rank < 8
                ? (MovePosition) (rank << 4)
                : MovePosition.RankUnknown;

            return result;
        }
    }
}

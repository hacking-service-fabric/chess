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
            "^(?<piece>[KQRBN]?)(?<fromPosition>[a-h]?[1-8]?)(?<taking>[x:]?)(?<toPosition>[a-h][1-8]?)[/=(]?(?<promotion>[QRBN]?)[)]?(?<check>\\+)?(?<checkmate>[x#])?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out MoveModel result)
        {
            result = new MoveModel();

            if (message.Trim().ToLower() == "resign")
            {
                result.Description = MoveDescription.Resign;
                return true;
            }

            var castleMatch = CastlePattern.Match(message);
            if (castleMatch.Success)
            {
                var castlePiece =
                    string.IsNullOrEmpty(castleMatch.Groups["isQueenSide"].Captures.FirstOrDefault()?.Value)
                        ? MoveDescription.King
                        : MoveDescription.Queen;

                result.Description = MoveDescription.Castle | castlePiece;
                return true;
            }

            var moveMatch = MovePattern.Match(message);
            if (!moveMatch.Success)
            {
                return false;
            }

            result.Description = moveMatch.Groups["piece"].Captures.FirstOrDefault()?.Value switch
            {
                "K" => MoveDescription.King,
                "Q" => MoveDescription.Queen,
                "R" => MoveDescription.Rook,
                "B" => MoveDescription.Bishop,
                "N" => MoveDescription.Knight,
                _ => moveMatch.Groups["promotion"].Captures.FirstOrDefault()?.Value switch
                {
                    "Q" => MoveDescription.Promotion | MoveDescription.Queen,
                    "R" => MoveDescription.Promotion | MoveDescription.Rook,
                    "B" => MoveDescription.Promotion | MoveDescription.Bishop,
                    "N" => MoveDescription.Promotion | MoveDescription.Knight,
                    _   => MoveDescription.Empty
                }
            };

            var takes = !string.IsNullOrEmpty(moveMatch.Groups["taking"].Captures.FirstOrDefault()?.Value);
            if (takes)
            {
                result.Description |= MoveDescription.Takes;
            }

            result.FromPosition = ParseMovePosition(moveMatch.Groups["fromPosition"].Captures.FirstOrDefault()?.Value);
            result.ToPosition = ParseMovePosition(moveMatch.Groups["toPosition"].Captures.FirstOrDefault()?.Value);
            
            return true;
        }

        public MovePosition ParseMovePosition(string position)
        {
            if (string.IsNullOrEmpty(position))
            {
                return MovePosition.Empty;
            }

            var result = MovePosition.Empty;

            var file = position.ToLower().First() - 'a';
            var rank = position.ToLower().Last() - '1';

            if (file >= 0 && file < 8)
            {
                result |= MovePosition.HasFile;
                result |= (MovePosition) (file << 1);
            }

            if (rank >= 0 && rank < 8)
            {
                result |= MovePosition.HasRank;
                result |= (MovePosition)(rank << 5);
            }

            return result;
        }
    }
}

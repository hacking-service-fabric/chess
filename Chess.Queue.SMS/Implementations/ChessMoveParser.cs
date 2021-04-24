using Chess.Queue.Common.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Queue.SMS.Implementations
{
    public class ChessMoveParser: IChessMoveParser
    {
        private static readonly Regex CastlePattern = new Regex(
            "^O-?O-?(?<isQueenSide>O)?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex MovePattern = new Regex(
            "^(?<piece>[KQRBN])?(?<fromPosition>[a-h]?[1-8]?)(?<capture>[x:])?(?<toPosition>[a-h][1-8]?)[/=(]?(?<promotion>[QRBN])?\\)?(?<check>\\+)?(?<checkmate>[x#])?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out MoveModel result)
        {
            result = new MoveModel();

            if (message.Trim().ToLower() == "gg")
            {
                result.Description = MoveDescription.Resign;
                return true;
            }

            var castleMatch = CastlePattern.Match(message);
            if (castleMatch.Success)
            {
                result.Description = castleMatch.Groups["isQueenSide"].Success
                    ? MoveDescription.QueenSideCastle
                    : MoveDescription.KingSideCastle;
                return true;
            }

            var moveMatch = MovePattern.Match(message);
            if (!moveMatch.Success)
            {
                return false;
            }

            var pieceCapture = moveMatch.Groups["piece"].Captures.FirstOrDefault()?.Value;
            if (moveMatch.Groups["promotion"].Success)
            {
                result.Description |= MoveDescription.Promotion;
                pieceCapture = moveMatch.Groups["promotion"].Captures.FirstOrDefault()?.Value;
            }

            result.Description |= pieceCapture switch
            {
                "K" => MoveDescription.King,
                "Q" => MoveDescription.Queen,
                "R" => MoveDescription.Rook,
                "B" => MoveDescription.Bishop,
                "N" => MoveDescription.Knight,
                _   => MoveDescription.Empty
            };
            
            if (moveMatch.Groups["capture"].Success)
            {
                result.Description |= MoveDescription.Capture;
            }

            result.FromPosition = ParseMovePosition(moveMatch.Groups["fromPosition"].Captures.FirstOrDefault()?.Value);
            result.ToPosition = ParseMovePosition(moveMatch.Groups["toPosition"].Captures.FirstOrDefault()?.Value);

            if (moveMatch.Groups["check"].Success)
            {
                result.Description |= MoveDescription.Check;
            }

            if (moveMatch.Groups["checkmate"].Success)
            {
                result.Description |= MoveDescription.Checkmate;
            }
            
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

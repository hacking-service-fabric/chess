using Chess.Common.Models;
using Chess.Queue.Common.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Queue.SMS.Implementations
{
    public class ChessMoveParser: IChessMoveParser
    {
        private static readonly Regex MovePattern = new Regex(
            "^(?<piece>[KQRBN]?)(?<fromPosition>[a-h]?[1-8]?)(?<taking>x?)(?<toPosition>[a-h][1-8]?)(?<promotion>[QRBN]?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out MoveModel result)
        {
            result = null;

            var castlePiece = message switch
            {
                "0-0" => MovePiece.KingSideCastle,
                "O-O" => MovePiece.KingSideCastle,
                "0-0-0" => MovePiece.QueenSideCastle,
                "O-O-O" => MovePiece.QueenSideCastle,
                _ => (MovePiece?) null
            };

            if (castlePiece.HasValue)
            {
                result = new MoveModel
                {
                    Piece = castlePiece.Value
                };
                return true;
            }

            var match = MovePattern.Match(message);
            if (!match.Success)
            {
                return false;
            }

            var piece = match.Groups["piece"].Captures.FirstOrDefault()?.Value switch
            {
                "K" => MovePiece.King,
                "Q" => MovePiece.Queen,
                "R" => MovePiece.Rook,
                "B" => MovePiece.Bishop,
                "N" => MovePiece.Knight,
                _ => match.Groups["promotion"].Captures.FirstOrDefault()?.Value switch
                {
                    "Q" => MovePiece.PawnToQueen,
                    "R" => MovePiece.PawnToRook,
                    "B" => MovePiece.PawnToBishop,
                    "N" => MovePiece.PawnToKnight,
                    _   => MovePiece.Unknown
                }
            };
            var takes = !string.IsNullOrEmpty(match.Groups["taking"].Captures.FirstOrDefault().Value);
            var fromPosition = ParseMovePosition(match.Groups["fromPosition"].Captures.FirstOrDefault()?.Value);
            var toPosition = ParseMovePosition(match.Groups["toPosition"].Captures.FirstOrDefault()?.Value);
            
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
            if (!string.IsNullOrEmpty(position))
            {
                var result = MovePosition.Empty;

                var file = position.ToLower()[0] - 'a';
                int rank;

                if (file >= 0 && file < 8)
                {
                    result |= (MovePosition)file;
                    rank = position[1] - '1';
                }
                else
                {
                    result |= MovePosition.FileUnknown;
                    rank = position[0] - '1';
                }

                if (rank >= 0 && rank < 8)
                {
                    result |= (MovePosition) (rank << 4);
                }
                else
                {
                    result |= MovePosition.RankUnknown;
                }

                return result;
            }

            return MovePosition.FileUnknown | MovePosition.RankUnknown;
        }
    }
}

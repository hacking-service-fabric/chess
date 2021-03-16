using Chess.Common.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chess.Queue.SMS.Implementations
{
    public class ChessMoveParser: IChessMoveParser
    {
        private static readonly Regex MovePattern = new Regex(
            "^(?<piece>[KQRBN]?)(?<fromFile>[a-h]?)(?<taking>x?)(?<fromRank>[1-8]?)(?<toFile>[a-h])(?<toRank>[1-8])$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool TryParse(string message, out ChessMove result)
        {
            result = null;
            var match = MovePattern.Match(message);
            if (!match.Success)
            {
                return false;
            }

            if (!Enum.TryParse<BoardEdge>(match.Groups["toFile"].Captures.FirstOrDefault()?.Value.ToUpper(), out var destinationFile))
            {
                return false;
            }

            if (!int.TryParse(match.Groups["toRank"].Captures.FirstOrDefault()?.Value, out var destinationRank))
            {
                return false;
            }

            var piece = match.Groups["piece"].Captures.FirstOrDefault()?.Value switch
            {
                "K" => ChessPiece.King,
                "Q" => ChessPiece.Queen,
                "R" => ChessPiece.Rook,
                "B" => ChessPiece.Bishop,
                "N" => ChessPiece.Knight,
                _ => (ChessPiece?) null
            };

            var hasStartingFile = Enum.TryParse<BoardEdge>(match.Groups["fromFile"].Captures.FirstOrDefault()?.Value.ToUpper(), out var startingFile);
            var hasStartingRank = int.TryParse(match.Groups["fromRank"].Captures.FirstOrDefault()?.Value, out var startingRank);

            result = new ChessMove
            {
                Piece = piece,
                Takes = match.Groups["taking"].Captures.Any(),
                StartingFile = hasStartingFile ? (BoardEdge?)startingFile : null,
                StartingRank = hasStartingRank ? (BoardEdge?)startingRank : null,
                DestinationFile = destinationFile,
                DestinationRank = (BoardEdge)destinationRank
            };
            return true;
        }
    }
}

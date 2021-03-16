namespace Chess.Common.Models
{
    public class ChessMove
    {
        public ChessPiece? Piece { get; set; }
        public bool Takes { get; set; }

        public BoardEdge? StartingFile { get; set; }
        public BoardEdge? StartingRank { get; set; }

        public BoardEdge DestinationFile { get; set; }
        public BoardEdge DestinationRank { get; set; }
    }
}

namespace Chess.Queue.Common.Models
{
    public class MoveModel
    {
        public MovePiece Piece { get; set; }
        public bool Takes { get; set; }
        public MovePosition FromPosition { get; set; }
        public MovePosition ToPosition { get; set; }
        public bool Check { get; set; }
        public bool Checkmate { get; set; }
    }
}

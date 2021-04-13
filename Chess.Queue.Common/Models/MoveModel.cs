namespace Chess.Queue.Common.Models
{
    public class MoveModel
    {
        public MoveDescription Description { get; set; }
        public MovePosition FromPosition { get; set; }
        public MovePosition ToPosition { get; set; }
    }
}

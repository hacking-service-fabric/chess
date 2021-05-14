namespace Chess.Data.Common.Models.V1
{
    public class MovePieceDto: MoveDtoBase
    {
        public Piece Piece { get; set; }
        public bool Captures { get; set; }

        public BoardFile? FromFile { get; set; }
        public BoardRank? FromRank { get; set; }

        public BoardFile ToFile { get; set; }
        public BoardRank? ToRank { get; set; }

        public bool Checks { get; set; }
        public bool Checkmates { get; set; }
    }
}

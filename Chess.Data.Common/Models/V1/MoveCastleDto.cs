namespace Chess.Data.Common.Models.V1
{
    public class MoveCastleDto: MoveDtoBase
    {
        public bool KingSide { get; set; }

        public bool Checks { get; set; }
        public bool Checkmates { get; set; }
    }
}

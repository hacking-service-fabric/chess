﻿namespace Chess.Data.Common.Models.V1
{
    public class MovePromotionDto: MoveDtoBase
    {
        public BoardFile? FromFile { get; set; }
        public BoardRank? FromRank { get; set; }
        public bool Captures { get; set; }

        public BoardFile ToFile { get; set; }
        public BoardRank ToRank { get; set; }

        public Piece PromotionToPiece { get; set; }

        public bool Check { get; set; }
        public bool Checkmate { get; set; }
    }
}

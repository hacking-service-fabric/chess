using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Queue.Common.Models
{
    public enum MovePiece: byte
    {
        Unknown,
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        PawnToQueen,
        PawnToRook,
        PawnToBishop,
        PawnToKnight,
        KingSideCastle,
        QueenSideCastle
    }
}

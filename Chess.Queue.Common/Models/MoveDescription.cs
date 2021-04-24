using System;

namespace Chess.Queue.Common.Models
{
    [Flags]
    public enum MoveDescription: byte
    {
        // Piece information
        Empty,
        King,
        Queen,
        Rook,
        Bishop,
        Knight,

        // Special states
        KingSideCastle,
        QueenSideCastle,
        Resign,

        // Flags
        Promotion = 1 << 4,
        Capture = 1 << 5,
        Check = 1 << 6,
        Checkmate = 1 << 7
    }
}

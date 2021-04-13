using System;

namespace Chess.Queue.Common.Models
{
    [Flags]
    public enum MoveDescription: byte
    {
        Empty,
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        Castle = 1 << 3,
        Takes = 1 << 4,
        Promotion = 1 << 5,
        Check = 1 << 6,
        Checkmate = 1 << 7,
        Resign = Check | Checkmate
    }
}

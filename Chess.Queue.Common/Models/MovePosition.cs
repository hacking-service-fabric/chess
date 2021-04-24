using System;

namespace Chess.Queue.Common.Models
{
    [Flags]
    public enum MovePosition: byte
    {
        // Serializing options
        HasFile = 1 << 0,
        File001 = 1 << 1,
        File010 = 1 << 2,
        File100 = 1 << 3,

        HasRank = 1 << 4,
        Rank001 = 1 << 5,
        Rank010 = 1 << 6,
        Rank100 = 1 << 7,

        // Members for testing and printing
        Empty = 0,

        FileA = 1,
        FileB = 3,
        FileC = 5,
        FileD = 7,
        FileE = 9,
        FileF = 11,
        FileG = 13,
        FileH = 15,

        Rank1 = 16,
        Rank2 = 48,
        Rank3 = 80,
        Rank4 = 112,
        Rank5 = 144,
        Rank6 = 176,
        Rank7 = 208,
        Rank8 = 240
    }
}

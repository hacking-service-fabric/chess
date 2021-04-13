using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Queue.Common.Models
{
    [Flags]
    public enum MovePosition: byte
    {
        Empty = 0,
        HasFile = 1,
        HasRank = 16,

        // Members for testing and printing
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

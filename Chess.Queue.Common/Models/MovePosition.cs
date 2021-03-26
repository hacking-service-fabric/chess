using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Queue.Common.Models
{
    [Flags]
    public enum MovePosition: byte
    {
        Empty = 0,
        FileAddOne = 1,
        FileAddTwo = 2,
        FileAddFour = 4,
        FileUnknown = 8,
        RankAddOne = 16,
        RankAddTwo = 32,
        RankAddFour = 64,
        RankUnknown = 128
    }
}

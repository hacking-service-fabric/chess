using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Data.Common.Tests.Models.V1
{
    [TestClass]
    public class MovePieceDtoTests
    {
        [DataRow("Bc5", MoveDescription.Bishop, MovePosition.Empty, BoardFile.FileC, BoardRank.Rank5)]
        [DataRow("Nf3", MoveDescription.Knight, MovePosition.Empty, MovePosition.FileF | MovePosition.Rank3)]
        [DataRow("b6", MoveDescription.Empty, MovePosition.Empty, MovePosition.FileB | MovePosition.Rank6)]

        [DataRow("Bxe5", MoveDescription.Bishop | MoveDescription.Capture,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank5)]
        [DataRow("B:e5", MoveDescription.Bishop | MoveDescription.Capture,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank5)]

        [DataRow("exd5", MoveDescription.Capture, MovePosition.FileE, MovePosition.FileD | MovePosition.Rank5)]
        [DataRow("exd", MoveDescription.Capture, MovePosition.FileE, MovePosition.FileD)]
        [DataRow("ed", MoveDescription.Empty, MovePosition.FileE, MovePosition.FileD)]

        [DataRow("Rdf8", MoveDescription.Rook, MovePosition.FileD, MovePosition.FileF | MovePosition.Rank8)]

        [DataRow("R1a3", MoveDescription.Rook, MovePosition.Rank1, MovePosition.FileA | MovePosition.Rank3)]

        [DataRow("Qh4e1", MoveDescription.Queen, MovePosition.FileH | MovePosition.Rank4,
            MovePosition.FileE | MovePosition.Rank1)]
    }
}

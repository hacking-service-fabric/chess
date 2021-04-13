using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Queue.Common.Models;
using Chess.Queue.SMS.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Queue.SMS.Tests.UnitTests.Implementations
{
    [TestClass]
    public class ChessMoveParserTests
    {
        [DataTestMethod]
        [DataRow("Be5", MoveDescription.Bishop, MovePosition.Empty, MovePosition.FileE | MovePosition.Rank5)]
        [DataRow("Nf3", MoveDescription.Knight, MovePosition.Empty, MovePosition.FileF | MovePosition.Rank3)]
        [DataRow("c5", MoveDescription.Empty, MovePosition.Empty, MovePosition.FileC | MovePosition.Rank5)]
        [DataRow("Bxe5", MoveDescription.Bishop | MoveDescription.Takes,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank5)]
        [DataRow("B:e5", MoveDescription.Bishop | MoveDescription.Takes,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank5)]
        [DataRow("exd5", MoveDescription.Takes, MovePosition.FileE, MovePosition.FileD | MovePosition.Rank5)]
        [DataRow("exd", MoveDescription.Takes, MovePosition.FileE, MovePosition.FileD)]
        [DataRow("ed", MoveDescription.Empty, MovePosition.FileE, MovePosition.FileD)]
        [DataRow("Rdf8", MoveDescription.Rook, MovePosition.FileD, MovePosition.FileF | MovePosition.Rank8)]
        [DataRow("R1a3", MoveDescription.Rook, MovePosition.Rank1, MovePosition.FileA | MovePosition.Rank3)]
        [DataRow("Qh4e1", MoveDescription.Queen, MovePosition.FileH | MovePosition.Rank4,
            MovePosition.FileE | MovePosition.Rank1)]
        [DataRow("e8Q", MoveDescription.Promotion | MoveDescription.Queen,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank8)]
        [DataRow("e8=Q", MoveDescription.Promotion | MoveDescription.Queen,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank8)]
        [DataRow("e8(Q)", MoveDescription.Promotion | MoveDescription.Queen,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank8)]
        [DataRow("e8/Q", MoveDescription.Promotion | MoveDescription.Queen,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank8)]
        [DataRow("0-0", MoveDescription.King | MoveDescription.Castle, MovePosition.Empty, MovePosition.Empty)]
        [DataRow("O-O", MoveDescription.King | MoveDescription.Castle, MovePosition.Empty, MovePosition.Empty)]
        [DataRow("OO", MoveDescription.King | MoveDescription.Castle, MovePosition.Empty, MovePosition.Empty)]
        [DataRow("0-0-0", MoveDescription.Queen | MoveDescription.Castle, MovePosition.Empty, MovePosition.Empty)]
        [DataRow("O-O-O", MoveDescription.Queen | MoveDescription.Castle, MovePosition.Empty, MovePosition.Empty)]
        [DataRow("ooo", MoveDescription.Queen | MoveDescription.Castle, MovePosition.Empty, MovePosition.Empty)]
        [DataRow("e8Q+", MoveDescription.Promotion | MoveDescription.Queen | MoveDescription.Check,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank8)]
        [DataRow("e8Q#", MoveDescription.Promotion | MoveDescription.Queen | MoveDescription.Checkmate,
            MovePosition.Empty, MovePosition.FileE | MovePosition.Rank8)]
        [DataRow("e8Qx", MoveDescription.Promotion | MoveDescription.Queen | MoveDescription.Checkmate,
            MovePosition.HasFile | MovePosition.HasRank, MovePosition.FileE | MovePosition.Rank5)]
        [DataRow("resign", MoveDescription.Resign, MovePosition.Empty, MovePosition.Empty)]
        public void TryParseValid(string message, MoveDescription description,
            MovePosition fromPosition, MovePosition toPosition)
        {
            var target = new ChessMoveParser();

            Assert.IsTrue(target.TryParse(message, out var result));

            Assert.AreEqual(description, result.Description);
            Assert.AreEqual(fromPosition, result.FromPosition);
            Assert.AreEqual(toPosition, result.ToPosition);
        }

        [DataTestMethod]
        [DataRow(null, MovePosition.Empty)]
        [DataRow("", MovePosition.Empty)]
        [DataRow("a", MovePosition.FileA)]
        [DataRow("f", MovePosition.FileF)]
        [DataRow("2", MovePosition.Rank2)]
        [DataRow("7", MovePosition.Rank7)]
        [DataRow("G4", MovePosition.FileG | MovePosition.Rank4)]
        public void ParseMovePosition(string input, MovePosition output)
        {
            var target = new ChessMoveParser();
            Assert.AreEqual(output, target.ParseMovePosition(input));
        }
    }
}

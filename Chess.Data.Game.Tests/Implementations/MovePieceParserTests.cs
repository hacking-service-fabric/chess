using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Data.Game.Tests.Implementations
{
    [TestClass]
    public class MovePieceParserTests
    {
        [DataTestMethod]
        [DataRow("c8", BoardFile.FileC, BoardRank.Rank8)]
        [DataRow("a1", BoardFile.FileA, BoardRank.Rank1)]
        [DataRow("e5", BoardFile.FileE, BoardRank.Rank5)]
        public void TryParse_Valid_ToPosition(string message, BoardFile toFile, BoardRank toRank)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Empty, pieceDto.Piece);

            Assert.IsNull(pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(toFile, pieceDto.ToFile);
            Assert.AreEqual(toRank, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [TestMethod]
        public void TryParse_Valid_FileOnly()
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse("ed", out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Empty, pieceDto.Piece);

            Assert.AreEqual(BoardFile.FileE, pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileD, pieceDto.ToFile);
            Assert.IsNull(pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [DataTestMethod]
        [DataRow("Kc8", Piece.King)]
        [DataRow("Qc8", Piece.Queen)]
        [DataRow("Nc8", Piece.Knight)]
        [DataRow("Rc8", Piece.Rook)]
        [DataRow("Bc8", Piece.Bishop)]
        public void TryParse_Valid_Piece(string message, Piece piece)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(piece, pieceDto.Piece);

            Assert.IsNull(pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileC, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank8, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [DataTestMethod]
        [DataRow("de5", BoardFile.FileD, null),
         DataRow("3e5", null, BoardRank.Rank3),
         DataRow("d4e5", BoardFile.FileD, BoardRank.Rank4)]
        public void TryParse_Valid_FromPosition(string message, BoardFile? fromFile, BoardRank? fromRank)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Empty, pieceDto.Piece);

            Assert.AreEqual(fromFile, pieceDto.FromFile);
            Assert.AreEqual(fromRank, pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileE, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank5, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [DataTestMethod]
        [DataRow("dxe2"), DataRow("d:e2")]
        public void TryParse_Valid_Captures(string message)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Empty, pieceDto.Piece);

            Assert.AreEqual(BoardFile.FileD, pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsTrue(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileE, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank2, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [DataTestMethod]
        [DataRow("Ne8"), DataRow("NE8"), DataRow("ne8"), DataRow(" ne8"), DataRow("Ne8 ")]
        public void TryParse_Valid_Format(string message)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Knight, pieceDto.Piece);

            Assert.IsNull(pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileE, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank8, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [TestMethod]
        public void TryParse_Valid_Check()
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse("e8+", out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Empty, pieceDto.Piece);

            Assert.IsNull(pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileE, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank8, pieceDto.ToRank);

            Assert.IsTrue(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [DataTestMethod]
        [DataRow("e8#"), DataRow("e8x")]
        public void TryParse_Valid_Checkmate(string message)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Empty, pieceDto.Piece);

            Assert.IsNull(pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileE, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank8, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsTrue(pieceDto.Checkmate);
        }

        [TestMethod]
        public void TryParse_Valid_FileBParsedAsBishop()
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse("bc5", out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Bishop, pieceDto.Piece);

            Assert.IsNull(pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileC, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank5, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [TestMethod]
        public void TryParse_Valid_FileBAndBishop()
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse("Bbc5", out var pieceDto);

            Assert.IsTrue(parsed);

            Assert.AreEqual(Piece.Bishop, pieceDto.Piece);

            Assert.AreEqual(BoardFile.FileB, pieceDto.FromFile);
            Assert.IsNull(pieceDto.FromRank);
            Assert.IsFalse(pieceDto.Captures);

            Assert.AreEqual(BoardFile.FileC, pieceDto.ToFile);
            Assert.AreEqual(BoardRank.Rank5, pieceDto.ToRank);

            Assert.IsFalse(pieceDto.Check);
            Assert.IsFalse(pieceDto.Checkmate);
        }

        [DataTestMethod]
        [DataRow("garbage"), DataRow("c8K"), DataRow(null), DataRow(""), DataRow(" ")]
        public void TryParse_NotValid(string message)
        {
            var target = new MovePieceParser();
            var parsed = target.TryParse(message, out var result);

            Assert.IsFalse(parsed);
            Assert.IsNull(result);
        }
    }
}

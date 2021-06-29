using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Data.Game.Tests.Implementations
{
    [TestClass]
    public class MovePromotionParserTests
    {
        [DataTestMethod]
        [DataRow("c8Q", BoardFile.FileC, BoardRank.Rank8)]
        [DataRow("a1Q", BoardFile.FileA, BoardRank.Rank1)]
        [DataRow("e8Q", BoardFile.FileE, BoardRank.Rank8)]
        public void TryParse_Valid_QueenToPosition(string message, BoardFile toFile, BoardRank toRank)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var promotion);

            Assert.IsTrue(parsed);

            Assert.IsNull(promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(toFile, promotion.ToFile);
            Assert.AreEqual(toRank, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }

        [DataTestMethod]
        [DataRow("c8Q", Piece.Queen)]
        [DataRow("c8N", Piece.Knight)]
        [DataRow("c8R", Piece.Rook)]
        [DataRow("c8B", Piece.Bishop)]
        public void TryParse_Valid_Piece(string message, Piece piece)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var promotion);

            Assert.IsTrue(parsed);

            Assert.IsNull(promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(BoardFile.FileC, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank8, promotion.ToRank);

            Assert.AreEqual(piece, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }

        [DataTestMethod]
        [DataRow("ba8Q", BoardFile.FileB, null),
         DataRow("7a8Q", null, BoardRank.Rank7),
         DataRow("b7a8Q", BoardFile.FileB, BoardRank.Rank7)]
        public void TryParse_Valid_FromPosition(string message, BoardFile? fromFile, BoardRank? fromRank)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var promotion);

            Assert.IsTrue(parsed);

            Assert.AreEqual(fromFile, promotion.FromFile);
            Assert.AreEqual(fromRank, promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(BoardFile.FileA, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank8, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }

        [TestMethod]
        [DataRow("bxa8Q"), DataRow("b:a8Q")]
        public void TryParse_Valid_Captures(string message)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var promotion);

            Assert.IsTrue(parsed);

            Assert.AreEqual(BoardFile.FileB, promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsTrue(promotion.Captures);

            Assert.AreEqual(BoardFile.FileA, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank8, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }

        [DataTestMethod]
        [DataRow("e8Q"), DataRow("E8q"), DataRow("e8q"), DataRow(" e8q"), DataRow("e8=Q"),
         DataRow("e8(Q)"), DataRow("e8(Q"), DataRow("e8Q)"), DataRow("e8/Q")]
        public void TryParse_Valid_Format(string message)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var promotion);

            Assert.IsTrue(parsed);

            Assert.IsNull(promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(BoardFile.FileE, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank8, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }
        
        [TestMethod]
        public void TryParse_Valid_Check()
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse("e8Q+", out var promotion);

            Assert.IsTrue(parsed);

            Assert.IsNull(promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(BoardFile.FileE, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank8, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsTrue(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }

        [DataTestMethod]
        [DataRow("e8Q#"), DataRow("e8Qx")]
        public void TryParse_Valid_Checkmate(string message)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var promotion);

            Assert.IsTrue(parsed);

            Assert.IsNull(promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(BoardFile.FileE, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank8, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsTrue(promotion.Checkmate);
        }

        [DataTestMethod]
        public void TryParse_Valid_CenterRank()
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse("c3Q", out var promotion);

            Assert.IsTrue(parsed);

            Assert.IsNull(promotion.FromFile);
            Assert.IsNull(promotion.FromRank);
            Assert.IsFalse(promotion.Captures);

            Assert.AreEqual(BoardFile.FileC, promotion.ToFile);
            Assert.AreEqual(BoardRank.Rank3, promotion.ToRank);

            Assert.AreEqual(Piece.Queen, promotion.PromotionToPiece);

            Assert.IsFalse(promotion.Check);
            Assert.IsFalse(promotion.Checkmate);
        }
        
        [DataTestMethod]
        [DataRow("garbage"), DataRow("c8K"), DataRow("e8"), DataRow(null), DataRow(""), DataRow(" ")]
        public void TryParse_NotValid(string message)
        {
            var target = new MovePromotionParser();
            var parsed = target.TryParse(message, out var result);

            Assert.IsFalse(parsed);
            Assert.IsNull(result);
        }
    }
}

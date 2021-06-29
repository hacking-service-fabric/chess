using System;
using System.Collections.Generic;
using System.Text;
using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Implementations;
using Chess.Data.Game.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chess.Data.Game.Tests.Implementations
{
    [TestClass]
    public class ChessMoveParserTests
    {
        private Mock<IChessMoveParser<MoveResignDto>> _mockResignParser;
        private Mock<IChessMoveParser<MoveCastleDto>> _mockCastleParser;
        private Mock<IChessMoveParser<MovePromotionDto>> _mockPromotionParser;
        private Mock<IChessMoveParser<MovePieceDto>> _mockPieceParser;

        private ChessMoveParser _target;

        [TestInitialize]
        public void SetupTarget()
        {
            _mockResignParser = new Mock<IChessMoveParser<MoveResignDto>>();
            _mockCastleParser = new Mock<IChessMoveParser<MoveCastleDto>>();
            _mockPromotionParser = new Mock<IChessMoveParser<MovePromotionDto>>();
            _mockPieceParser = new Mock<IChessMoveParser<MovePieceDto>>();

            _target = new ChessMoveParser(_mockResignParser.Object, _mockCastleParser.Object,
                _mockPromotionParser.Object, _mockPieceParser.Object);
        }

        [TestMethod]
        public void MoveResign_First()
        {
            var expected = new MoveResignDto();
            _mockResignParser
                .Setup(p => p.TryParse("a", out expected))
                .Returns(true);

            var parsed = _target.TryParse("a", out var actual);

            Assert.IsTrue(parsed);
            Assert.AreEqual(expected, actual);

            MoveResignDto verifyResignDto;
            _mockResignParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyResignDto), Times.Once);
            _mockResignParser.VerifyNoOtherCalls();

            _mockCastleParser.VerifyNoOtherCalls();
            _mockPromotionParser.VerifyNoOtherCalls();
            _mockPieceParser.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void MoveCastle_Second()
        {
            var expected = new MoveCastleDto();
            _mockCastleParser
                .Setup(p => p.TryParse("a", out expected))
                .Returns(true);

            var parsed = _target.TryParse("a", out var actual);

            Assert.IsTrue(parsed);
            Assert.AreEqual(expected, actual);

            MoveResignDto verifyResignDto;
            _mockResignParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyResignDto), Times.Once);
            _mockResignParser.VerifyNoOtherCalls();

            MoveCastleDto verifyCastleDto;
            _mockCastleParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyCastleDto), Times.Once);
            _mockCastleParser.VerifyNoOtherCalls();

            _mockPromotionParser.VerifyNoOtherCalls();
            _mockPieceParser.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void MovePromotion_Third()
        {
            var expected = new MovePromotionDto();
            _mockPromotionParser
                .Setup(p => p.TryParse("a", out expected))
                .Returns(true);

            var parsed = _target.TryParse("a", out var actual);

            Assert.IsTrue(parsed);
            Assert.AreEqual(expected, actual);

            MoveResignDto verifyResignDto;
            _mockResignParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyResignDto), Times.Once);
            _mockResignParser.VerifyNoOtherCalls();

            MoveCastleDto verifyCastleDto;
            _mockCastleParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyCastleDto), Times.Once);
            _mockCastleParser.VerifyNoOtherCalls();

            MovePromotionDto verifyPromotionDto;
            _mockPromotionParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyPromotionDto), Times.Once);
            _mockPromotionParser.VerifyNoOtherCalls();

            _mockPieceParser.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void MovePiece_Fourth()
        {
            var expected = new MovePieceDto();
            _mockPieceParser
                .Setup(p => p.TryParse("a", out expected))
                .Returns(true);

            var parsed = _target.TryParse("a", out var actual);

            Assert.IsTrue(parsed);
            Assert.AreEqual(expected, actual);

            MoveResignDto verifyResignDto;
            _mockResignParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyResignDto), Times.Once);
            _mockResignParser.VerifyNoOtherCalls();

            MoveCastleDto verifyCastleDto;
            _mockCastleParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyCastleDto), Times.Once);
            _mockCastleParser.VerifyNoOtherCalls();

            MovePromotionDto verifyPromotionDto;
            _mockPromotionParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyPromotionDto), Times.Once);
            _mockPromotionParser.VerifyNoOtherCalls();

            MovePieceDto verifyPieceDto;
            _mockPieceParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyPieceDto), Times.Once);
            _mockPieceParser.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void MoveInvalid_Last()
        {
            var parsed = _target.TryParse("a", out var actual);

            Assert.IsFalse(parsed);
            Assert.IsNull(actual);

            MoveResignDto verifyResignDto;
            _mockResignParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyResignDto), Times.Once);
            _mockResignParser.VerifyNoOtherCalls();

            MoveCastleDto verifyCastleDto;
            _mockCastleParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyCastleDto), Times.Once);
            _mockCastleParser.VerifyNoOtherCalls();

            MovePromotionDto verifyPromotionDto;
            _mockPromotionParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyPromotionDto), Times.Once);
            _mockPromotionParser.VerifyNoOtherCalls();

            MovePieceDto verifyPieceDto;
            _mockPieceParser.Verify(p => p.TryParse(It.IsAny<string>(), out verifyPieceDto), Times.Once);
            _mockPieceParser.VerifyNoOtherCalls();
        }
    }
}

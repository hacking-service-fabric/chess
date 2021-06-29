using Chess.Data.Game.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Data.Game.Tests.Implementations
{
    [TestClass]
    public class MoveCastleParserTests
    {
        [DataTestMethod]
        [DataRow("O-O", true)]
        [DataRow("OO", true)]
        [DataRow("O-O-O", false)]
        [DataRow("ooo", false)]
        [DataRow("  ooo ", false)]
        public void TryParse_Valid(string message, bool kingSide)
        {
            var target = new MoveCastleParser();
            var parsed = target.TryParse(message, out var castleDto);

            Assert.IsTrue(parsed);
            Assert.AreEqual(kingSide, castleDto.KingSide);
        }

        [DataTestMethod]
        [DataRow("garbage"), DataRow("o"), DataRow("o-"), DataRow(null), DataRow(""), DataRow(" ")]
        public void TryParse_NotValid(string message)
        {
            var target = new MoveCastleParser();
            var parsed = target.TryParse(message, out var castleDto);

            Assert.IsFalse(parsed);
            Assert.IsNull(castleDto);
        }
    }
}

using Chess.Data.Game.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Data.Game.Tests.Implementations
{
    [TestClass]
    public class MoveResignParserTests
    {
        [DataTestMethod]
        [DataRow("gg"), DataRow("Gg"), DataRow("GG")]
        public void TryParse_Valid(string message)
        {
            var target = new MoveResignParser();
            var parsed = target.TryParse(message, out var result);

            Assert.IsTrue(parsed);
            Assert.IsNotNull(result);
        }

        [DataTestMethod]
        [DataRow("not gg"), DataRow(null), DataRow(""), DataRow(" ")]
        public void TryParse_NotValid(string message)
        {
            var target = new MoveResignParser();
            var parsed = target.TryParse(message, out var result);

            Assert.IsFalse(parsed);
            Assert.IsNull(result);
        }
    }
}

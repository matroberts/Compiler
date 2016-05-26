using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void Token_HasValueEquality()
        {
            var token = new Token(SymbolName.BooleanFalse, "false", 0);

            // null
            Assert.That(token.Equals(null), Is.False);
            // different type
            Assert.That(token.Equals("false"), Is.False);
            // different name
            Assert.That(token.Equals(new Token(SymbolName.Identifier, "false", 0)), Is.False);
            // different value
            Assert.That(token.Equals(new Token(SymbolName.BooleanFalse, "false", 0)), Is.True);

            // same object
            Assert.That(token.Equals(token), Is.True);
            // same name and value
            Assert.That(new Token(SymbolName.Identifier, "false", 0).Equals(new Token(SymbolName.Identifier, "true", 0)), Is.False);
        }

        [Test]
        public void ErrorToken_Equality_ConsidersTheMessage_AsWellAsValue()
        {
            var token = new ErrorToken("{", 0, "message");

            Assert.That(token.Equals(null), Is.False);
            Assert.That(token.Equals("identifier"), Is.False);
            Assert.That(token.Equals(new Token(SymbolName.Identifier, "{", 0)), Is.False);

            Assert.That(token.Equals(token), Is.True);
            Assert.That(token.Equals(new ErrorToken("{", 0, "different message")), Is.False);
            Assert.That(token.Equals(new ErrorToken("/", 0, "message")), Is.False);
            Assert.That(token.Equals(new ErrorToken("{", 0, "message")), Is.True);
        }
    }
}
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
            var token = new IdentifierToken("identifier", 0);

            Assert.That(token.Equals(null), Is.False);
            Assert.That(token.Equals("identifier"), Is.False);
            Assert.That(token.Equals(new IdentifierToken("different", 0)), Is.False);

            Assert.That(token.Equals(token), Is.True);
            Assert.That(token.Equals(new IdentifierToken("identifier", 0)), Is.True);
        }

        [Test]
        public void Token_Equality_ShouldAccountForSubType()
        {
            var token1 = new IdentifierToken("word", 0);
            var token2 = new BlockCommentToken("word", 0);

            Assert.That(token1.Equals(token2), Is.False);
            Assert.That(token2.Equals(token1), Is.False);
        }

        [Test]
        public void ErrorToken_Equality_ConsidersTheMessage_AsWellAsValue()
        {
            var token = new ErrorToken("{", 0, "message");

            Assert.That(token.Equals(null), Is.False);
            Assert.That(token.Equals("identifier"), Is.False);
            Assert.That(token.Equals(new IdentifierToken("{", 0)), Is.False);

            Assert.That(token.Equals(token), Is.True);
            Assert.That(token.Equals(new ErrorToken("{", 0, "different message")), Is.False);
            Assert.That(token.Equals(new ErrorToken("/", 0, "message")), Is.False);
            Assert.That(token.Equals(new ErrorToken("{", 0, "message")), Is.True);
        }
    }
}
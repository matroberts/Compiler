using System;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests.Lexer
{
    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void Token_HasValueEquality()
        {
            var token = new IdentifierToken("identifier");

            Assert.That(token.Equals(null), Is.False);
            Assert.That(token.Equals("identifier"), Is.False);
            Assert.That(token.Equals(new IdentifierToken("different")), Is.False);

            Assert.That(token.Equals(token), Is.True);
            Assert.That(token.Equals(new IdentifierToken("identifier")), Is.True);
        }

        [Test]
        public void Token_Equality_ShouldAccountForSubType()
        {
            var token1 = new IdentifierToken("word");
            var token2 = new KeywordToken("word");

            Assert.That(token1.Equals(token2), Is.False);
            Assert.That(token2.Equals(token1), Is.False);
        }
    }
}
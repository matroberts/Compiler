using System;
using System.Linq;
using System.Runtime.InteropServices;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests.Lexer
{
    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void GetTokens_GivenAnEmptyString_ReturnsEmptyList()
        {
            var token = new Tokenizer(string.Empty).GetNextToken();

            Assert.That(token, Is.Null);
        }

        // Single white space

        [Test]
        public void ASingleCharacter_IsAnIdentifier()
        {
            var input = "a";

            var token = new Tokenizer(input).GetNextToken();

            Assert.That(token, Is.InstanceOf<IdentifierToken>());
            Assert.That(token.Value, Is.EqualTo("a"));
        }

        [Test]
        public void AStringOfCharacters_IsAnIdentifier()
        {
            var input = "aa";

            var token = new Tokenizer(input).GetNextToken();

            Assert.That(token, Is.InstanceOf<IdentifierToken>());
            Assert.That(token.Value, Is.EqualTo("aa"));
        }

        [Test]
        public void WhiteSpace_ShouldBeThrownAway()
        {
            var input = " a";

            var token = new Tokenizer(input).GetNextToken();

            Assert.That(token, Is.InstanceOf<IdentifierToken>());
            Assert.That(token.Value, Is.EqualTo("a"));
        }


        // "a b"

        // throw away input which is not tokens
    }
}
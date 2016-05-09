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
            var tokens = new Tokenizer().GetTokens(String.Empty);

            Assert.That(tokens, Is.Empty);
        }

        // Single white space

        [Test]
        public void ASingleCharacter_IsAnIdentifier()
        {
            var input = "a";

            var tokens = new Tokenizer().GetTokens(input);

            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0], Is.InstanceOf<IdentifierToken>());
            Assert.That(tokens[0].Value, Is.EqualTo("a"));
        }

        [Test]
        public void AStringOfCharacters_IsAnIdentifier()
        {
            var input = "aa";

            var tokens = new Tokenizer().GetTokens(input);

            Assert.That(tokens.Count, Is.EqualTo(1));
            Assert.That(tokens[0], Is.InstanceOf<IdentifierToken>());
            Assert.That(tokens[0].Value, Is.EqualTo("aa"));
        }

        // "a b"

        // throw away input which is not tokens
    }
}
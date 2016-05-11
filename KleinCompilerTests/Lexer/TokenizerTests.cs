﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests.Lexer
{
    [TestFixture]
    public class TokenizerTests
    {
        #region empty

        [Test]
        public void GetNextToken_ReturnsNull_WhenThereAreNoMoreTokens()
        {
            var input = string.Empty;

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void GetNextToken_ReturnsNull_WhenTheInputIsWhitespace()
        {
            var input = "    ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.Null);
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region identifiers

        [Test]
        public void Identifier_ASingleCharacter_IsAnIdentifier()
        {
            var input = "a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("a")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_AStringOfCharacters_IsAnIdentifier()
        {
            var input = "aa";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("aa")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_LeadingWhiteSpace_ShouldBeThrownAway()
        {
            var input = " a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("a")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_TrailingWhiteSpace_ShouldBeThrownAway()
        {
            var input = "abc ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("abc")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_SeriesOfIdentifiers_SeparatedByWhiteSpace()
        {
            var input = " aa bb c ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("aa")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("bb")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("c")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region keywords

        [Test]
        public void IntegerKeyword_ShouldBeRecognised()
        {
            var input = "integer";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken("integer")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByEndOfFile_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("intege")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByWhitespace_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("intege")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

    }
}
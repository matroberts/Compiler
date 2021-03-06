﻿using System;
using System.Linq;
using NUnit.Framework;

namespace TextTemplating
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void AStringLiteral_IsParsedAs_SingleLiteral()
        {
            var tokens = Parser.Parse("this is a literal.");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'this is a literal.'"));
        }

        [Test]
        public void ASingleOpenCurley_IsParsedAs_SingleLiteral()
        {
            var tokens = Parser.Parse("{");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'{'"));
        }

        [Test]
        public void ASingleOpenCurleyAndALetter_IsParsedAs_SingleLiteral()
        {
            var tokens = Parser.Parse("{a");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'{a'"));
        }

        [Test]
        public void ALiteralEndingWithAnOpenCurly_IsParsedAs_TwoLiterals()
        {
            var tokens = Parser.Parse("text{");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'text', L:'{'"));
        }

        [Test]
        public void ALiteralWithAClosingCurleyInIt_IsParsedAs_SingleLiterals()
        {
            var tokens = Parser.Parse("text}moretext");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'text}moretext'"));
        }

        [Test]
        public void AVariable_IsParsedAs_ButHasAnEmptyTrailingLiteral()
        {
            var tokens = Parser.Parse("{{name}");

            Assert.That(tokens.ToString(), Is.EqualTo("V:'{{name}', L:''"));
        }

        [Test]
        public void StartWithVariable_ThenLiteral_ShouldParseCorrectly()
        {
            var tokens = Parser.Parse("{{name}hello");

            Assert.That(tokens.ToString(), Is.EqualTo("V:'{{name}', L:'hello'"));
        }

        [Test]
        public void StartWithLiteral_ThenVariable_ShouldParseCorrectly()
        {
            var tokens = Parser.Parse("hello{{name}");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'hello', V:'{{name}', L:''"));
        }

        [Test]
        public void ATemplateWithMultipleParts_ShouldParseCorrectly()
        {
            var tokens = Parser.Parse("Hello, {{name}.");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'Hello, ', V:'{{name}', L:'.'"));
        }

        [Test]
        public void TemplateParser_ShouldWorkWithUnicodeCharacters()
        {
            var tokens = Parser.Parse("Pound£, Knifeナイフ");

            Assert.That(tokens.ToString(), Is.EqualTo("L:'Pound£, Knifeナイフ'"));
        }

        [Test]
        public void NestedVariableTags_ShouldParseAsASingleVariable_IeOnceStartParsingAVariableCarryOnUntilClosingCurley()
        {
            var tokens = Parser.Parse("{{var1{{var2}}");

            Assert.That(tokens.ToString(), Is.EqualTo("V:'{{var1{{var2}', L:'}'"));
        }

        [Test]
        public void AnOpenTag_ShouldBeParsedAs_AnOpenTag()
        {
            var tokens = Parser.Parse("{?name}more text");

            Assert.That(tokens.ToString(), Is.EqualTo("O:'{?name}', L:'more text'"));
        }

        [Test]
        public void ACloseTag_ShouldBeParsedAs_ACloseTag()
        {
            var tokens = Parser.Parse("{!name}more text");

            Assert.That(tokens.ToString(), Is.EqualTo("C:'{!name}', L:'more text'"));
        }
    }
}
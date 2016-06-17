using System;
using System.IO;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parser_ShouldCorrectlyParse_SuperSimpleFile()
        {
            // arrange
            var input = @"
main () : boolean
    true";

            // act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Parser_ShouldIgnore_Comments()
        {
            //arrange
            var input = @"
//line comment should be ignored
main () : boolean
    true";

            //act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Parser_ShouldHalt_OnLexicalErrors()
        {
            //arrange
            var input = @"
// ! is an illegal token
main () : boolean
    !true";

            //act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.False);
            Assert.That(parser.Error.Message, Is.EqualTo($"Unknown character '!'"));
        }

        [Test]
        public void Parser_AnErrorIsRaised_WhenNonTerminalAtTopOfTheSymbolStack_ButTheParsingTableHasNoRuleForTheNextTokenInTheStream()
        {
            // arrange
            var input = "";

            // act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.False);
            Assert.That(parser.Error.Message, Is.EqualTo($"Syntax Error:  Attempting to parse symbol 'Program' found token End"));
        }

        [Test]
        public void Parser_AnErrorIsRaised_WhenTokenAtTopOfTheSymbolStack_AndTheNextTokenInStreamDoesNotMatch()
        {
            // arrange
            var input = "main secondary";

            // act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.False);
            Assert.That(parser.Error.Message, Is.EqualTo($"Syntax Error:  Attempting to parse symbol 'OpenBracket' found token Identifier 'secondary'"));
        }

        [Test]
        public void ParserShould_ParseSlightlyMoreComplexProgram()
        {
            // arrange
            var input = @"
main(x: integer):integer
    circularPrimesTo(x)
circularPrimesTo(x: integer):integer
    true";

            // act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void Parser_ShouldParse_AllOfTheValidSampleKleinPrograms()
        {
            var start = DateTime.UtcNow;
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\fullprograms");
            var files = Directory.GetFiles(folder, "*.kln");
            foreach (var file in files)
            {
                var parser = new Parser(new ParsingTable());
                var isValid = parser.Parse(new Tokenizer(File.ReadAllText(file)));
                Assert.That(isValid, Is.True, $"File {Path.GetFileName(file)} is invalid, {parser.Error}");
            }
            Console.WriteLine(DateTime.UtcNow - start);
        }
    }
}
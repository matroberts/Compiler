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
            Assert.That(result, Is.True, parser.Error);
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
            Assert.That(result, Is.False, parser.Error);
            Console.WriteLine(parser.Error);
            Console.WriteLine(parser.SymbolStackTrace);
        }

        [Test]
        public void Parser_ShouldReport_GrammarErrors()
        {
            // arrange
            var input = "";

            // act
            var parser = new Parser(new ParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.False);
            Console.WriteLine(parser.Error);
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
            Assert.That(result, Is.True, parser.Error);

        }

        [Test]
        public void Parser_ShouldParse_AllOfTheValidSampleKleinPrograms()
        {
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\fullprograms");
            var files = Directory.GetFiles(folder, "*.kln");
            foreach (var file in files)
            {
                var parser = new Parser(new ParsingTable());
                var isValid = parser.Parse(new Tokenizer(File.ReadAllText(file)));

                var filename = Path.GetFileName(file);
                if (isValid)
                {
                    Console.WriteLine($"File {filename} valid");
                }
                else
                {
                    Console.WriteLine($"File {filename} invalid");
                    Assert.That(isValid, Is.True, $"File {Path.GetFileName(file)} is invalid, {parser.Error}");
                }
            }
        }

        // parser should halt on lexical errors
    }
}
using System;
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
            var input = "main () : boolean";

            // act
            var parser = new Parser(new ReducedParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Parser_ShouldIgnore_LineComment()
        {
            //arrange
            var input = @"//line comment should be ignored
                        main () : boolean";

            //act
            var parser = new Parser(new ReducedParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.True, parser.Error);
        }

        [Test]
        public void Parser_ShouldReport_GrammarErrors()
        {
            // arrange
            var input = "";

            // act
            var parser = new Parser(new ReducedParsingTable());
            var result = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(result, Is.False);
            Console.WriteLine(parser.Error);
        }


    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ParserTests
    {
        #region original tests

        [Test]
        public void Parser_ShouldCorrectlyParse_SuperSimpleFile()
        {
            // arrange
            var input = @"
main () : boolean
    true";

            // act
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.Not.Null);
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
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.Not.Null);
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
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.Null);
            Assert.That(parser.Error.Message, Is.EqualTo($"Unknown character '!'"));
        }

        [Test]
        public void Parser_AnErrorIsRaised_WhenNonTerminalAtTopOfTheSymbolStack_ButTheParsingTableHasNoRuleForTheNextTokenInTheStream()
        {
            // arrange
            var input = "";

            // act
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.Null);
            Assert.That(parser.Error.Message, Is.EqualTo($"Syntax Error:  Attempting to parse symbol 'Program' found token End"));
        }

        [Test]
        public void Parser_AnErrorIsRaised_WhenTokenAtTopOfTheSymbolStack_AndTheNextTokenInStreamDoesNotMatch()
        {
            // arrange
            var input = "main secondary";

            // act
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.Null);
            Assert.That(parser.Error.Message, Is.EqualTo($"Syntax Error:  Attempting to parse symbol 'OpenBracket' found token Identifier 'secondary'"));
        }

        [Test, Ignore("ignored until a lot more grammar is done")]
        public void ParserShould_ParseSlightlyMoreComplexProgram()
        {
            // arrange
            var input = @"
main(x: integer):integer
    circularPrimesTo(x)
circularPrimesTo(x: integer):integer
    true";

            // act
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.Not.Null);
        }

        [Test, Ignore("ignored until gast complete")]
        public void Parser_ShouldParse_AllOfTheValidSampleKleinPrograms()
        {
            var start = DateTime.UtcNow;
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\fullprograms");
            var files = Directory.GetFiles(folder, "*.kln");
            foreach (var file in files)
            {
                var parser = new Parser();
                var ast = parser.Parse(new Tokenizer(File.ReadAllText(file)));
                Assert.That(ast, Is.Not.Null, $"File {Path.GetFileName(file)} is invalid, {parser.Error}");
            }
            Console.WriteLine(DateTime.UtcNow - start);
        }

        #endregion

        #region DeclarationGrammarTests

        [Test]
        public void SimplestPossibleProgram_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"main() : boolean
                             true";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new Program(
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType(KType.Boolean),
                                                        formals: new List<Formal>()
                                                    ))));
        }

        [Test]
        public void Definition_WithOneFormal_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"main(arg1 : integer) : boolean
                              true";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new Program(
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType(KType.Boolean),
                                                        formals: new List<Formal> { new Formal(new Identifier("arg1"), new KleinType(KType.Integer)) }
                                                    ))));
        }

        [Test]
        public void Definition_WithTwoFormals_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"main(arg1 : integer, arg2 : boolean) : boolean
                              true";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new Program(
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType(KType.Boolean),
                                                        formals: new List<Formal>
                                                        {
                                                            new Formal(new Identifier("arg1"), new KleinType(KType.Integer)),
                                                            new Formal(new Identifier("arg2"), new KleinType(KType.Boolean)),
                                                        }
                                                    ))));
        }

        [Test]
        public void Program_WithTwoDefinitions_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"
main() : boolean
    true                      
subsidiary() : integer
    1";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new Program(
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType(KType.Boolean),
                                                        formals: new List<Formal>()
                                                    ),
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("subsidiary"),
                                                        type: new KleinType(KType.Integer),
                                                        formals: new List<Formal>()
                                                    ))));
        }

        #endregion
    }
}
﻿using System;
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
        #region Test the Parser class itself (Simplest Possible Program and Error Handling)

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
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(true))
                                                    ))));
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
            Assert.That(ast, Is.AstEqual(new Program(
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType(KType.Boolean),
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(true))
                                                    ))));
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

        #region Declaration Grammar Tests

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
                                                        formals: new List<Formal> { new Formal(new Identifier("arg1"), new KleinType(KType.Integer)) },
                                                        body: new Body(expr: new BooleanLiteral(true))
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
                                                        },
                                                        body: new Body(expr: new BooleanLiteral(true))
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
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(true))
                                                    ),
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("subsidiary"),
                                                        type: new KleinType(KType.Integer),
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new IntegerLiteral("1"))
                                                    ))));
        }

        #endregion

        #region Expression Grammar

        [Test]
        public void ParserShould_GenerateAstFor_Addition()
        {
            // arrange
            var input = @"main(x: integer, y : integer) : integer x + y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Console.WriteLine(parser.StackTrace);
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new BinaryOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              op: BOp.Plus,
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }
/*
        [Test]
        public void ParserShould_GenerateAstFor_Multiplication()
        {
            // arrange
            var input = @"x * y";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new BinaryOperator
                                                    (
                                                        left: new Identifier("x"),
                                                        op: BOp.Times,
                                                        right: new Identifier("y")
                                                    )
                                                ));
        }

        [Test]
        public void ParserShould_ParseExpression_WithBrackets()
        {
            // arrange
            var input = @"(x)";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new Identifier("x")));
        }

        [Test]
        public void ParserShould_GetPrecedence_OfMultiplcationAndAdditionCorrect_1()
        {
            // arrange
            var input = @"x + y * z";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new BinaryOperator
                                                    (
                                                        left: new Identifier("x"),
                                                        op: BOp.Plus,
                                                        right: new BinaryOperator
                                                                   (
                                                                        left: new Identifier("y"),
                                                                        op: BOp.Times,
                                                                        right: new Identifier("z"))
                                                                   )
                                                    ));
        }

        [Test]
        public void ParserShould_GetPrecedence_OfMultiplcationAndAdditionCorrect_2()
        {
            // arrange
            var input = @"x * y + z";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new BinaryOperator
                                                    (
                                                        left: new BinaryOperator
                                                                  (
                                                                      left: new Identifier("x"),
                                                                      op: BOp.Times,
                                                                      right: new Identifier("y")
                                                                  ),
                                                        op: BOp.Plus,
                                                        right: new Identifier("z")
                                                    )));
        }

        [Test]
        public void ParserShould_GetPrecedence_OfBracketedExpressionCorrect()
        {
            // arrange
            var input = @"(x + y) * z";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new BinaryOperator
                                                    (
                                                        left: new BinaryOperator
                                                                  (
                                                                      left: new Identifier("x"),
                                                                      op: BOp.Plus,
                                                                      right: new Identifier("y")
                                                                  ),
                                                        op: BOp.Times,
                                                        right: new Identifier("z")
                                                    )));
        }

        [Test]
        public void ParserShould_GenerateAst_WithLeftAssociativeMultiplication()
        {
            // arrange
            var input = @"x * y * z";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new BinaryOperator
                                                    (
                                                        left: new BinaryOperator
                                                                  (
                                                                      left: new Identifier("x"),
                                                                      op: BOp.Times,
                                                                      right: new Identifier("y")
                                                                  ),
                                                        op: BOp.Times,
                                                        right: new Identifier("z")
                                                    )));
        }

        [Test]
        public void ParserShould_GenerateAst_WithLeftAssociativeAddition()
        {
            // arrange
            var input = @"x + y + z";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create(), new ArithmeticGrammarAstFactory()) { EnableStackTrace = true };
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new BinaryOperator
                                                    (
                                                        left: new BinaryOperator
                                                                  (
                                                                      left: new Identifier("x"),
                                                                      op: BOp.Plus,
                                                                      right: new Identifier("y")
                                                                  ),
                                                        op: BOp.Plus,
                                                        right: new Identifier("z")
                                                    )));
        }
*/
        #endregion 
    }
}
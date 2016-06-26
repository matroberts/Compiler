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

        #region Declaration Grammar

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

        #region Binary Operators

        [TestCase("<", BOp.LessThan)] // R16
        [TestCase("=", BOp.Equals)]   // R17
        [TestCase("or", BOp.Or)]      // R20
        [TestCase("+", BOp.Plus)]     // R21
        [TestCase("-", BOp.Minus)]    // R22
        [TestCase("and", BOp.And)]    // R25
        [TestCase("*", BOp.Times)]    // R26
        [TestCase("/", BOp.Divide)]   // R27
        public void ParserShould_GenerateAstForAllBinaryOperators(string op, BOp bop)
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x {op} y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new BinaryOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              op: bop,
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [TestCase("<", BOp.LessThan)] // R16
        [TestCase("=", BOp.Equals)]   // R17
        [TestCase("or", BOp.Or)]      // R20
        [TestCase("+", BOp.Plus)]     // R21
        [TestCase("-", BOp.Minus)]    // R22
        [TestCase("and", BOp.And)]    // R25
        [TestCase("*", BOp.Times)]    // R26
        [TestCase("/", BOp.Divide)]   // R27
        public void ParserShould_BinaryOperators_ShouldBeLeftAssociative(string op, BOp bop)
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x {op} y {op} z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new BinaryOperator
                                                                        (
                                                                            left: new BinaryOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            op: bop,
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            op: bop,
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void ParserShould_ParseExpression_WithBrackets_R34()
        {
            // arrange
            var input = @"main(x: integer) : integer (x)";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new Identifier("x")));
        }

        [Test]
        public void ParserShould_GetPrecedence_OfMultiplcationAndAdditionCorrect_A_R21_R26()
        {
            // arrange
            var input = @"main(x: integer, y : integer, z : integer) : integer x + y * z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new BinaryOperator
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
        public void ParserShould_GetPrecedence_OfMultiplcationAndAdditionCorrect_B_R21_R26()
        {
            // arrange
            var input = @"main(x: integer, y : integer, z : integer) : integer x * y + z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new BinaryOperator
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
        public void ParserShould_GetPrecedence_OfBracketedExpressionCorrect_R34()
        {
            // arrange
            var input = @"main(x: integer, y : integer, z : integer) : integer (x + y) * z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new BinaryOperator
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

        #endregion

        #region Unary Operators

        [TestCase("not", UOp.Not)]    // R30
        [TestCase("-", UOp.Negate)]   // R33
        public void ParserShould_GenerateAstForAllUnaryOperators(string op, UOp uop)
        {
            // arrange
            var input = $"main(x: boolean) : boolean {op} x";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new UnaryOperator
                                                                          (
                                                                              op: uop,
                                                                              right: new Identifier("x")
                                                                          )
                                                                      ));
        }

        #endregion

        #region IfThenElse

        [Test]
        public void ParseShould_GenerateAstForIfThenElse_R29()
        {
            // arrange
            var input = @"main(x: boolean, y : integer, z : integer) : integer if x then y else z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            if (program == null) Console.WriteLine(parser.StackTrace);
            Assert.That(program, Is.Not.Null, parser.Error.Message);
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new IfThenElse
                                                                            (
                                                                                ifExpr: new Identifier("x"),
                                                                                thenExpr: new Identifier("y"),
                                                                                elseExpr: new Identifier("z")) 
                                                                            ));
        }

        #endregion
    }
}
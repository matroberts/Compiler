using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;
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
                                                        identifier: new Identifier(0, "main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(0, true))
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
                                                        identifier: new Identifier(0, "main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(0, true))
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
            Assert.That(parser.Error.Message, Is.EqualTo($"Attempting to parse symbol 'Program' found token End"));
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
            Assert.That(parser.Error.Message, Is.EqualTo($"Attempting to parse symbol 'OpenBracket' found token Identifier 'secondary'"));
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
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(ast, Is.AstEqual(new Program
                                         (
                                            new Definition
                                            (
                                                new Identifier(0, "main"),
                                                new IntegerTypeDeclaration(), 
                                                new List<Formal>
                                                {
                                                    new Formal(new Identifier(0, "x"), new IntegerTypeDeclaration())
                                                },
                                                new Body
                                                (
                                                    new FunctionCall
                                                    (
                                                        new Identifier(0, "circularPrimesTo"),
                                                        new List<Actual> { new Actual(new Identifier(0, "x")) } 
                                                    )
                                                )
                                            ),
                                            new Definition
                                            (
                                                new Identifier(0, "circularPrimesTo"),
                                                new IntegerTypeDeclaration(), 
                                                new List<Formal>
                                                {
                                                    new Formal(new Identifier(0, "x"), new IntegerTypeDeclaration())
                                                },
                                                new Body
                                                (
                                                    new BooleanLiteral(0, true)
                                                )
                                            )
                                         )));
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
                                                        identifier: new Identifier(0, "main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal> { new Formal(new Identifier(0, "arg1"), new IntegerTypeDeclaration()) },
                                                        body: new Body(expr: new BooleanLiteral(0, true))
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
                                                        identifier: new Identifier(0, "main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal>
                                                        {
                                                            new Formal(new Identifier(0, "arg1"), new IntegerTypeDeclaration()),
                                                            new Formal(new Identifier(0, "arg2"), new BooleanTypeDeclaration()),
                                                        },
                                                        body: new Body(expr: new BooleanLiteral(0, true))
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
                                                        identifier: new Identifier(0, "main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(0, true))
                                                    ),
                                                    new Definition
                                                    (
                                                        identifier: new Identifier(0, "subsidiary"),
                                                        typeDeclaration: new IntegerTypeDeclaration(), 
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new IntegerLiteral(0, "1"))
                                                    ))));
        }

        #endregion

        #region Binary Operators

        public static IEnumerable<TestCaseData> AllBinaryOperators
        {
            get
            {
                yield return new TestCaseData("<", new LessThanOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("=", new EqualsOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("or", new OrOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("+", new PlusOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("-", new MinusOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("and", new AndOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("*", new TimesOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
                yield return new TestCaseData("/", new DivideOperator(0, left: new Identifier(0, "x"), right: new Identifier(0, "y")));
            }
        }

        [TestCaseSource(typeof(ParserTests), nameof(AllBinaryOperators))]
        public void ParserShould_GenerateAst_ForAllBinaryOperators(string op, BinaryOperator bop)
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x {op} y";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(bop));
        }

        public static IEnumerable<TestCaseData> LeftAssocativeBinaryOperator
        {
            get
            {
                yield return new TestCaseData("<", new LessThanOperator(0, new LessThanOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("=", new EqualsOperator(0, new EqualsOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("or", new OrOperator(0, new OrOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("+", new PlusOperator(0, new PlusOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("-", new MinusOperator(0, new MinusOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("and", new AndOperator(0, new AndOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("*", new TimesOperator(0, new TimesOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
                yield return new TestCaseData("/", new DivideOperator(0, new DivideOperator(0, new Identifier(0, "x"), new Identifier(0, "y")), new Identifier(0, "z")));
            }
        }

        [TestCaseSource(typeof(ParserTests), nameof(LeftAssocativeBinaryOperator))]
        public void BinaryOperators_ShouldBeLeftAssociative(string op, BinaryOperator bop)
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x {op} y {op} z";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(bop));
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new Identifier(0, "x")));
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new PlusOperator
                                                                            (
                                                                                position: 0,
                                                                                left: new Identifier(0, "x"),
                                                                                right: new TimesOperator
                                                                                            (
                                                                                                position: 0,
                                                                                                left: new Identifier(0, "y"),
                                                                                                right: new Identifier(0, "z"))
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new PlusOperator
                                                                        (
                                                                            position: 0,
                                                                            left: new TimesOperator
                                                                                        (
                                                                                            position: 0,
                                                                                            left: new Identifier(0, "x"),
                                                                                            right: new Identifier(0, "y")
                                                                                        ),
                                                                            right: new Identifier(0, "z")
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new TimesOperator
                                                                        (
                                                                            position: 0,
                                                                            left: new PlusOperator
                                                                                        (
                                                                                            position: 0,
                                                                                            left: new Identifier(0, "x"),
                                                                                            right: new Identifier(0, "y")
                                                                                        ),
                                                                            right: new Identifier(0, "z")
                                                                        )));
        }

        #endregion

        #region Unary Operators

        [Test]    
        public void ParserShould_GenerateAstForNotOperator_R30()
        {
            // arrange
            var input = $"main(x: boolean) : boolean not x";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new NotOperator
                                                                          (
                                                                              position: 0,
                                                                              right: new Identifier(0, "x")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstForNegateOperators_R33()
        {
            // arrange
            var input = $"main(x: boolean) : boolean - x";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new NegateOperator
                                                                          (
                                                                              position: 0,
                                                                              right: new Identifier(0, "x")
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new IfThenElse
                                                                            (
                                                                                position: 0,
                                                                                ifExpr: new Identifier(0, "x"),
                                                                                thenExpr: new Identifier(0, "y"),
                                                                                elseExpr: new Identifier(0, "z")) 
                                                                            ));
        }

        #endregion

        #region Function Call

        [Test]
        public void ParserShould_GenerateAstFor_FunctionCallWithNoActuals_R36()
        {
            // arrange
            var input = @"
main() : integer 
    secondary()
secondary() : integer
    1";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new FunctionCall
                                                                          (
                                                                              identifier: new Identifier(0, "secondary"),
                                                                              actuals: new List<Actual>() 
                                                                          )));
        }

        [Test]
        public void ParserShould_GenerateAstFor_FunctionCallWithOneActual_R40()
        {
            // arrange
            var input = @"
main(x : integer) : integer 
    secondary(x)
secondary(x : integer) : integer
    1";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new FunctionCall
                                                                          (
                                                                              identifier: new Identifier(0, "secondary"),
                                                                              actuals: new List<Actual>
                                                                              {
                                                                                  new Actual(new Identifier(0, "x"))
                                                                              }
                                                                          )));
        }

        [Test]
        public void ParserShould_GenerateAstFor_FunctionCallWithTwoActuals_R41()
        {
            // arrange
            var input = @"
main(x : integer, y : integer) : integer 
    secondary(x, y)
secondary(x : integer, y : integer) : integer
    1";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new FunctionCall
                                                                          (
                                                                              identifier: new Identifier(0, "secondary"),
                                                                              actuals: new List<Actual>
                                                                              {
                                                                                  new Actual(new Identifier(0, "x")),
                                                                                  new Actual(new Identifier(0, "y"))
                                                                              }
                                                                          )));
        }

        #endregion

        #region Print

        [Test]
        public void ParserShould_GenerateMethod_WithOnePrintExpression()
        {
            // arrange
            var input = @"
main(x: integer) : integer 
    print(x)
    x+1";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            ConsoleWriteLine.If(program == null, parser.Error.ToString());
            Assert.That(program.Definitions[0].Body.Prints, Is.AstEqual(new ReadOnlyCollection<Print>(new List<Print>
                                                                                                        {
                                                                                                            new Print(0, new Identifier(0, "x"))
                                                                                                        })));
        }

        [Test]
        public void ParserShould_GenerateMethod_WithTwoPrintExpression()
        {
            // arrange
            var input = @"
main(x: integer, y : integer) : integer 
    print(x)
    print(y)
    x+1";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            ConsoleWriteLine.If(program == null, parser.Error.ToString());
            Assert.That(program.Definitions[0].Body.Prints, Is.AstEqual(new ReadOnlyCollection<Print>(new List<Print>
                                                                                                        {
                                                                                                            new Print(0, new Identifier(0, "x")),
                                                                                                            new Print(0, new Identifier(0, "y")),
                                                                                                        })));
        }

        #endregion

        #region Sample Programs

        [Test]
        public void Parser_ShouldParse_AllOfTheValidSampleKleinPrograms()
        {
            var start = DateTime.UtcNow;
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\fullprograms");
            var files = Directory.GetFiles(folder, "*.kln");
            bool allPass = true;
            var result = new StringBuilder();
            foreach (var file in files)
            {
                var parser = new Parser();
                var ast = parser.Parse(new Tokenizer(File.ReadAllText(file)));
                if (ast == null)
                {
                    allPass = false;
                    result.AppendLine($"Fail {Path.GetFileName(file)}");
                }
                else
                {
                    result.AppendLine($"Pass {Path.GetFileName(file)}");
                }
            }
            ConsoleWriteLine.If(allPass != true, result.ToString());
            Assert.That(allPass, Is.True);
//            Console.WriteLine(DateTime.UtcNow - start);
        }

        #endregion

        #region LineNumbers

        /*
         *  In the table driven parser line numbers get lost for non-terminal ast nodes.
         *  This is because the creation of the non-terminal does not happen immediatly after the token is processed
         *  For example with the AndOperator defined by Rule 25
         *  R25     <FactorTail>          ::= and <Factor> MakeAnd <FactorTail>
         *  - the 'and' token is processed, 
         *  - then a bunch of other stuff happens to process the <Factor>, 
         *  - then finally the MakeAnd symbol is process to make the ast node AndOperator
         *  
         *  So to capture the position of the 'and' token, add an extra symbol
         *  R25     <FactorTail>          ::= and PushPosition <Factor> MakeAnd <FactorTail>
         *  and use the PushPosition symbol to caputre the position of the 'and' token on a new stack
         *  Then when MakeAnd runs it can pop the position off the stack to get the postion of the symbol
         */

        [Test]
        public void PostionShouldWorkCorrectlyWithNestedOperators()
        {
            // arrange
            var input =
@"main(x: integer, y : integer) : boolean (x < 3) = (y < 4)";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            var equalsOperator = program.Definitions[0].Body.Expr as EqualsOperator;
            var left = equalsOperator.Left as LessThanOperator;
            var right = equalsOperator.Right as LessThanOperator;

            Assert.That(equalsOperator.Position, Is.EqualTo(48));
            Assert.That(left.Position, Is.EqualTo(43));
            Assert.That(right.Position, Is.EqualTo(53));
        }

        [TestCase("<")]
        [TestCase("=")]
        [TestCase("or")]
        [TestCase("+")]
        [TestCase("-")]
        [TestCase("and")]
        [TestCase("*")]
        [TestCase("/")]
        public void BinaryOperators_SupportPosition(string op)
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x {op} y";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as BinaryOperator).Position, Is.EqualTo(42));
        }

        [TestCase("-")]
        [TestCase("not")]
        public void UnaryOperators_SupportPosition(string op)
        {
            // arrange
            var input = $"main(x: integer) : integer {op} x";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as UnaryOperator).Position, Is.EqualTo(27));
        }

        [TestCase]
        public void IfThenElse_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer if true then false else false";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as IfThenElse).Position, Is.EqualTo(27));
        }

        [Test]
        public void Identifier_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer x";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as Identifier).Position, Is.EqualTo(27));
        }

        [Test]
        public void IntegerLiteral_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer 1";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as IntegerLiteral).Position, Is.EqualTo(27));
        }

        [Test]
        public void BooleanLiteralTrue_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer true";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as BooleanLiteral).Position, Is.EqualTo(27));
        }

        [Test]
        public void BooleanLiteralFalse_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer false";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as BooleanLiteral).Position, Is.EqualTo(27));
        }

        [Test]
        public void Print_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer print(false) false";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Prints[0] as Print).Position, Is.EqualTo(27));
        }

        [Test]
        public void FunctionCall_SupportsPosition()
        {
            // arrange
            var input = $"main(x: integer) : integer secondary(1) secondary(y : integer) : integer 2";

            // act
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            //41
            // assert
            Assert.That((program.Definitions[0].Body.Expr as FunctionCall).Position, Is.EqualTo(27));
        }


        #endregion
    }
}
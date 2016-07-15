using System;
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
                                                        identifier: new Identifier("main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
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
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
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
                                                new Identifier("main"),
                                                new IntegerTypeDeclaration(), 
                                                new List<Formal>
                                                {
                                                    new Formal(new Identifier("x"), new IntegerTypeDeclaration())
                                                },
                                                new Body
                                                (
                                                    new FunctionCall
                                                    (
                                                        new Identifier("circularPrimesTo"),
                                                        new List<Actual> { new Actual(new Identifier("x")) } 
                                                    )
                                                )
                                            ),
                                            new Definition
                                            (
                                                new Identifier("circularPrimesTo"),
                                                new IntegerTypeDeclaration(), 
                                                new List<Formal>
                                                {
                                                    new Formal(new Identifier("x"), new IntegerTypeDeclaration())
                                                },
                                                new Body
                                                (
                                                    new BooleanLiteral(true)
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
                                                        identifier: new Identifier("main"),
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal> { new Formal(new Identifier("arg1"), new IntegerTypeDeclaration()) },
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
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal>
                                                        {
                                                            new Formal(new Identifier("arg1"), new IntegerTypeDeclaration()),
                                                            new Formal(new Identifier("arg2"), new BooleanTypeDeclaration()),
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
                                                        typeDeclaration: new BooleanTypeDeclaration(), 
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new BooleanLiteral(true))
                                                    ),
                                                    new Definition
                                                    (
                                                        identifier: new Identifier("subsidiary"),
                                                        typeDeclaration: new IntegerTypeDeclaration(), 
                                                        formals: new List<Formal>(),
                                                        body: new Body(expr: new IntegerLiteral("1"))
                                                    ))));
        }

        #endregion

        #region Binary Operators

        [Test]
        public void ParserShould_GenerateAstFor_LessThan()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x < y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new LessThanOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_Equals()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x = y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new EqualsOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_Or()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x or y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new OrOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_Minus()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x - y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new MinusOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_Plus()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x + y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new PlusOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_And()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x and y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new AndOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_Times()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x * y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new TimesOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void ParserShould_GenerateAstFor_Divide()
        {
            // arrange
            var input = $"main(x: integer, y : integer) : integer x / y";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new DivideOperator
                                                                          (
                                                                              left: new Identifier("x"),
                                                                              right: new Identifier("y")
                                                                          )
                                                                      ));
        }

        [Test]
        public void LessThanOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x < y < z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new LessThanOperator
                                                                        (
                                                                            left: new LessThanOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void EqualsOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x = y = z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new EqualsOperator
                                                                        (
                                                                            left: new EqualsOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void OrOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x or y or z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new OrOperator
                                                                        (
                                                                            left: new OrOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void MinusOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x - y - z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new MinusOperator
                                                                        (
                                                                            left: new MinusOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void AndOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x and y and z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new AndOperator
                                                                        (
                                                                            left: new AndOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void TimesOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x * y * z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new TimesOperator
                                                                        (
                                                                            left: new TimesOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void DivideOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x / y / z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new DivideOperator
                                                                        (
                                                                            left: new DivideOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
                                                                        )));
        }

        [Test]
        public void PlusOperators_ShouldBeLeftAssociative()
        {
            // arrange
            var input = $"main(x: integer, y : integer, z : integer) : integer x + y + z";

            // act
            var parser = new Parser() { EnableStackTrace = true };
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new PlusOperator
                                                                        (
                                                                            left: new PlusOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new PlusOperator
                                                                            (
                                                                                left: new Identifier("x"),
                                                                                right: new TimesOperator
                                                                                            (
                                                                                                left: new Identifier("y"),
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new PlusOperator
                                                                        (
                                                                            left: new TimesOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new TimesOperator
                                                                        (
                                                                            left: new PlusOperator
                                                                                        (
                                                                                            left: new Identifier("x"),
                                                                                            right: new Identifier("y")
                                                                                        ),
                                                                            right: new Identifier("z")
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
                                                                              right: new Identifier("x")
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
            Assert.That(program.Definitions[0].Body.Expr, Is.AstEqual(new IfThenElse
                                                                            (
                                                                                ifExpr: new Identifier("x"),
                                                                                thenExpr: new Identifier("y"),
                                                                                elseExpr: new Identifier("z")) 
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
                                                                              identifier: new Identifier("secondary"),
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
                                                                              identifier: new Identifier("secondary"),
                                                                              actuals: new List<Actual>
                                                                              {
                                                                                  new Actual(new Identifier("x"))
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
                                                                              identifier: new Identifier("secondary"),
                                                                              actuals: new List<Actual>
                                                                              {
                                                                                  new Actual(new Identifier("x")),
                                                                                  new Actual(new Identifier("y"))
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
                                                                                                            new Print(new Identifier("x"))
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
                                                                                                            new Print(new Identifier("x")),
                                                                                                            new Print(new Identifier("y")),
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
    }
}
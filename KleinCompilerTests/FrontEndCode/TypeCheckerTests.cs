using System.Collections.Generic;
using System.Linq;
using KleinCompiler.AbstractSyntaxTree;
using KleinCompiler.FrontEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.FrontEndCode
{
    [TestFixture]
    public class TypeCheckerTests
    {
        #region Program

        [Test]
        public void IfProgramContainsDuplicateFunctionNames_ATypeErrorIsRaised()
        {
            // arrange
            var input = @" main() : boolean
                              true
                          main() : integer
                              1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Program contains duplicate function name 'main'"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfProgramDoesNotContainAFunction_main_ATypeErrorIsRaised()
        {
            // arrange
            var input = @" secondary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Program must contain a function 'main'"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfProgramContainsAFunctionCalled_print_AnErrorIsRaised_ButItIsAParseError_NotATypeError()
        {
            // arrange
            var input = @"print() : boolean
                              true";
            var parser = new Parser();

            // act
            var errorRecord = new ErrorRecord(input);
            var program = (Program)parser.Parse(new Tokenizer(input), errorRecord);

            // assert
            Assert.That(program, Is.Null);
            Assert.That(errorRecord.Message, Is.EqualTo("Attempting to parse symbol 'Program' found token PrintKeyword 'print'"));
            Assert.That(errorRecord.ErrorType, Is.EqualTo(ErrorTypeEnum.Syntax));
        }

        [Test]
        public void TheTypeOfProgram_ShouldBeEqualToTheTypeOfMain()
        {
            // arrange
            var input = @"main() : integer
                              1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.False);
            Assert.That(program.Type, Is.EqualTo(new IntegerType()));
        }

        #endregion

        #region Definition

        [Test]
        public void IfTypeOfFunction_AndTypeOfBody_Match_FunctionIsValid()
        {
            // arrange
            var input = @"main() : boolean
                              true";
            var parser = new Parser();
            var program = (Program) parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.False);
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(new FunctionType(new BooleanType())));
            Assert.That(program.Definitions[0].TypeDeclaration.Type, Is.EqualTo(new BooleanType()));
        }
        
        [Test]
        public void IfTypeOfFunction_AndTypeItsBody_DoNotMatch_ATypeErrorIsRaised()
        {
            // arrange
            var input = @" main() : boolean
                              1";
            var parser = new Parser();
            var program = (Program) parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function 'main' has a return type 'boolean', but its body has a type 'integer'"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void TypesShouldBeAppliedToTheFormals_OfAFunction()
        {
            // arrange
            var input = @"main(x: integer, y:boolean) : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.False);
            Assert.That(program.Definitions[0].Formals[0].Type, Is.EqualTo(new IntegerType()));
            Assert.That(program.Definitions[0].Formals[1].Type, Is.EqualTo(new BooleanType()));
        }

        [Test]
        public void IfFormalIdentifiers_AreRepeated_ATypeErrorIsRaised()
        {
            // arrange
            var input = @" main(x: integer, x:integer) : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function 'main' has duplicate formal 'x'"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region FunctionCall and Actuals

        [Test]
        public void TypeOfFunctionCall_ShouldBeDerivedFromDefinition_ViaTheSymbolTable()
        {
            // arrange
            var input = @"main() : boolean
                              secondary()
                          secondary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program) parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            var functionCall = program.Definitions[0].Body.Expr as FunctionCall;
            Assert.That(functionCall.Type, Is.EqualTo(new BooleanType()));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfFunctionCallIdentifier_IsNotInSymbolTable_ATypeErrorShouldBeRaised()
        {
            // arrange
            var input = @"main() : boolean
                              notexists()
                          secondary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function 'notexists' has no definition"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void InAFunctionCall_IfTheActualHasTypeError_ATypeErrorShouldBeRaised()
        {
            // arrange
            var input = @"main() : boolean
                              secondary(- true)
                          secondary(x : boolean) : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void AnActualsType_IsSetEqualToTheTypeOfTheActualsExpression()
        {
            // arrange
            var input = @"main() : boolean
                              secondary(not true)
                          secondary(x : boolean) : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            var functionCall = program.Definitions[0].Body.Expr as FunctionCall;
            Assert.That(functionCall.Actuals[0].Type, Is.EqualTo(new BooleanType()));
            Assert.That(result.HasError, Is.False, result.Message);
        }

        [Test]
        public void IfDefinition_AndFunctionCall_HaveDifferentNumberOfArguments_AnErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              secondary()
                          secondary(x : boolean) : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function secondary(boolean):boolean called with mismatched arguments secondary()"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region SymbolTable

        [Test]
        public void SymbolTable_ShouldRecordTheCallerOfAFunction()
        {
            // arrange
            var input = @"main() : boolean
                              secondary()
                          secondary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));
            program.CheckType();

            // act
            var functionInfos = Ast.SymbolTable.FunctionInfos.ToList();

            // assert
            Assert.That(functionInfos.Count, Is.EqualTo(2));
            Assert.That(functionInfos.Single(fi => fi.Name=="main").Callers, Is.Empty);
            Assert.That(functionInfos.Single(fi => fi.Name=="secondary").Callers, Is.EquivalentTo(new [] {"main"} ));
        }

        [Test]
        public void SymbolTable_ShouldRecordTheCallerOfAFunction2()
        {
            // arrange
            var input = @"main() : boolean
                              secondary(tertiary())
                          secondary(x:boolean) : boolean
                              not x
                          tertiary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));
            program.CheckType();

            // act
            var functionInfos = Ast.SymbolTable.FunctionInfos.ToList();

            // assert
            Assert.That(functionInfos.Count, Is.EqualTo(3));
            Assert.That(functionInfos.Single(fi => fi.Name == "main").Callers, Is.Empty);
            Assert.That(functionInfos.Single(fi => fi.Name == "secondary").Callers, Is.EquivalentTo(new[] { "main" }));
            Assert.That(functionInfos.Single(fi => fi.Name == "tertiary").Callers, Is.EquivalentTo(new[] { "main" }));
        }

        #endregion

        #region Identifier and visible identifiers in a function - note that identifiers in the ast only refer to variables

        [Test]
        public void AnIdentifier_DerivesItsType_FromTheFormalInTheFunctionDeclaration()
        {
            // arrange
            var input = @"main(x : boolean) : boolean
                              secondary(1)
                          secondary(x : integer) : boolean
                              x < 2";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.False);
            var identifier = (program.Definitions[1].Body.Expr as BinaryOperator).Left as Identifier;
            Assert.That(identifier.Type, Is.EqualTo(new IntegerType()));
        }

        [Test]
        public void IfABody_UsesAVariableWhichIsNotDefinedInTheDeclaration_AnErrorIsRaised()
        {
            // arrange
            var input = @"main(x : boolean) : boolean
                              secondary(1)
                          secondary(y : integer) : boolean
                              x < 2";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Use of undeclared identifier x in function secondary"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region NotOperator

        [Test]
        public void NotOperator_HasBooleanType()
        {
            // arrange
            var input = @"main() : boolean
                              not false";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That((program.Definitions[0].Body.Expr as NotOperator).Type, Is.EqualTo(new BooleanType()));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfExprOfNotOperator_IsNotABoolean_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              not 1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Not operator called with expression which is not boolean"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfExprOfNotOperator_HasATypeError_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              not (1 < true)";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("LessThan right expression is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region NegateOperator

        [Test]
        public void NegateOperator_HasIntegerType()
        {
            // arrange
            var input = @"main() : integer
                              - 1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That((program.Definitions[0].Body.Expr as NegateOperator).Type, Is.EqualTo(new IntegerType()));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfExprOfNegateOperator_IsNotAnInteger_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : integer
                              - true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfExprOfNegateOperator_HasATypeError_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : integer
                              - (0 < true)";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("LessThan right expression is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region BinaryOperators

        public static IEnumerable<TestCaseData> AllBinaryOperators
        {
            get
            {
                yield return new TestCaseData("main() : boolean 0 < 1", new BooleanType());
                yield return new TestCaseData("main() : boolean 0 = 1", new BooleanType());
                yield return new TestCaseData("main() : boolean true or false", new BooleanType());
                yield return new TestCaseData("main() : boolean true and false", new BooleanType());
                yield return new TestCaseData("main() : integer 1 + 1", new IntegerType());
                yield return new TestCaseData("main() : integer 1 - 1", new IntegerType());
                yield return new TestCaseData("main() : integer 1 * 1", new IntegerType());
                yield return new TestCaseData("main() : integer 1 / 1", new IntegerType());
            }
        }

        [TestCaseSource(typeof(TypeCheckerTests), nameof(AllBinaryOperators))]
        public void BinaryOperators_ShouldHaveCorrectType(string input, PrimitiveType type)
        {
            // arrange
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.False, result.Message);
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(type));
        }


        // Less Than
        [TestCase("main() : boolean true < 1", "LessThan left expression is not integer")]
        [TestCase("main() : boolean (-true) < 1", "Negate operator called with expression which is not integer")]
        [TestCase("main() : boolean 0 < true", "LessThan right expression is not integer")]
        [TestCase("main() : boolean 0 < (-true)", "Negate operator called with expression which is not integer")]
        // Equals
        [TestCase("main() : boolean true = 1", "Equals left expression is not integer")]
        [TestCase("main() : boolean (-true) = 1", "Negate operator called with expression which is not integer")]
        [TestCase("main() : boolean 0 = true", "Equals right expression is not integer")]
        [TestCase("main() : boolean 0 = (-true)", "Negate operator called with expression which is not integer")]
        // Or
        [TestCase("main() : boolean 1 or false", "Or left expression is not boolean")]
        [TestCase("main() : boolean (-true) or false", "Negate operator called with expression which is not integer")]
        [TestCase("main() : boolean true or 1", "Or right expression is not boolean")]
        [TestCase("main() : boolean true or (-true)", "Negate operator called with expression which is not integer")]
        // And
        [TestCase("main() : boolean 1 and false", "And left expression is not boolean")]
        [TestCase("main() : boolean (-true) and false", "Negate operator called with expression which is not integer")]
        [TestCase("main() : boolean true and 1", "And right expression is not boolean")]
        [TestCase("main() : boolean true and (-true)", "Negate operator called with expression which is not integer")]
        // Plus
        [TestCase("main() : integer true + 1", "Plus left expression is not integer")]
        [TestCase("main() : integer (-true) + 1", "Negate operator called with expression which is not integer")]
        [TestCase("main() : integer 1 + true", "Plus right expression is not integer")]
        [TestCase("main() : integer 1 + (-true)", "Negate operator called with expression which is not integer")]
        // Minus
        [TestCase("main() : integer true - 1", "Minus left expression is not integer")]
        [TestCase("main() : integer (-true) - 1", "Negate operator called with expression which is not integer")]
        [TestCase("main() : integer 1 - true", "Minus right expression is not integer")]
        [TestCase("main() : integer 1 - (-true)", "Negate operator called with expression which is not integer")]
        // Times
        [TestCase("main() : integer true * 1", "Times left expression is not integer")]
        [TestCase("main() : integer (-true) * 1", "Negate operator called with expression which is not integer")]
        [TestCase("main() : integer 1 * true", "Times right expression is not integer")]
        [TestCase("main() : integer 1 * (-true)", "Negate operator called with expression which is not integer")]
        // Divide
        [TestCase("main() : integer true / 1", "Divide left expression is not integer")]
        [TestCase("main() : integer (-true) / 1", "Negate operator called with expression which is not integer")]
        [TestCase("main() : integer 1 / true", "Divide right expression is not integer")]
        [TestCase("main() : integer 1 / (-true)", "Negate operator called with expression which is not integer")]
        // Error should be raise if:
        //   Left expression is wrong type
        //   Left expression has type error
        //   Right expression is wrong type
        //   Right expression has type error
        public void BinaryOperators_ShouldDetectTypeErrors(string input, string errormessage)
        {
            // arrange
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo(errormessage));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region IfThenElse

        [Test]
        public void IfThenElseOperator_HasTypeEqualToTheThenAndElseExpressions1()
        {
            // arrange
            var input = @"main() : boolean
                              if 
                                  true
                              then 
                                  false
                              else
                                  false";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(new BooleanType()));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfThenElseOperator_HasTypeEqualToTheThenAndElseExpressions2()
        {
            // arrange
            var input = @"main() : integer
                              if 
                                  true
                              then 
                                  1
                              else
                                  2";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(new IntegerType()));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfThenElseOperator_IfIfExpressionIsNotBoolean_AnErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              if 
                                  1
                              then 
                                  false
                              else
                                  false";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("IfThenElseOperator must have a boolean if expression"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfThenElseOperator_IfIfExpressionHasTypeError_AnErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              if 
                                  0 < (-true)
                              then 
                                  false
                              else
                                  false";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfThenElseOperator_IfThenExpressionHasATypeError_AnErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              if
                                  true
                              then
                                  0 < (-true)
                              else
                                  false";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfThenElseOperator_IfElseExpressionHasATypeError_AnErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              if
                                  true
                              then
                                  false
                              else
                                  0 < (-true)";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfThenElseOperator_IfTypeOfThenAndElseAreDifferent_AnErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              if
                                  true
                              then
                                  false
                              else
                                  1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("IfThenElse, type of then and else expression must be the same"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        #endregion

        #region Print

        [Test]
        public void TypeOfPrint_IsSetEqualToTypeOfPrintsExpression()
        {
            // arrange
            var input = @"main() : boolean
                              print(17)
                              print(true)
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(program.Definitions[0].Body.Prints[0].Type, Is.EqualTo(new IntegerType()));
            Assert.That(program.Definitions[0].Body.Prints[1].Type, Is.EqualTo(new BooleanType()));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfPrint_HasATypeError_ItIsReported()
        {
            // arrange
            var input = @"main() : boolean
                              print(-true)
                              true";

            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
            Assert.That(result.Position, Is.GreaterThan(0));
        }

        [Test]
        public void IfPrint_OccursInWrongPlaceInProgram_ItIsReported_ButItIsAParseError()
        {
            // arrange
            var input = @"main() : boolean
                              true
                              print(1)
                              true";

            var parser = new Parser();

            // act
            var errorRecord = new ErrorRecord(input);
            var program = (Program)parser.Parse(new Tokenizer(input), errorRecord);

            // assert
            Assert.That(program, Is.Null);
            Assert.That(errorRecord.Message, Is.EqualTo("Attempting to parse symbol 'FactorTail' found token PrintKeyword 'print'"));
            Assert.That(errorRecord.ErrorType, Is.EqualTo(ErrorTypeEnum.Syntax));
        }

        #endregion

        // unused identifier in function definition
        // unused functions

    }
}
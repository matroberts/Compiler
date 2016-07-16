using System;
using System.Linq;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class TypeCheckerTests
    {
        #region Program

        [Test]
        public void IfProgramContainsDuplicateFunctionNames_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              true
                          main() : integer
                              1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Program contains duplicate function name 'main'"));
        }

        [Test]
        public void IfProgramDoesNotContainAFunction_main_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"secondary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Program must contain a function 'main'"));
        }

        [Test]
        public void IfProgramContainsAFunctionCalled_print_AnErrorIsRaised_ButItIsAParseError_NotATypeError()
        {
            // arrange
            var input = @"print() : boolean
                              true";
            var parser = new Parser();

            // act
            var program = (Program)parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(program, Is.Null);
            Assert.That(parser.Error.ToString(), Is.EqualTo("Syntax Error: Attempting to parse symbol 'Program' found token PrintKeyword 'print'"));
        }

        [Test]
        public void TheTypeOfProgram_ShouldBeEqualToTheTypeOfMain()
        {
            // arrange
            var input = @"main() : integer
                              1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

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
            var program = (Program) parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.False);
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(new FunctionType(new BooleanType())));
        }
        
        [Test]
        public void IfTypeOfFunction_AndTypeItsBody_DoNotMatch_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              1";
            var parser = new Parser();
            var program = (Program) parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function 'main' has a return type 'boolean', but its body has a type 'integer'"));
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
            var program = (Program) parser.Parse(new Tokenizer(input));

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
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function 'notexists' has no definition"));
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
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
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
            var program = (Program)parser.Parse(new Tokenizer(input));

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
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function secondary(boolean):boolean called with mismatched arguments ()"));
        }

        #endregion

        #region Identifier and visible identifiers in a function - note that identifiers in the ast only refer to variables

        [Test, Ignore("")]
        public void AnIdentifier_DerivesItsType_FromTheFormalInTheFunctionDeclaration()
        {
            // arrange
            var input = @"main(x : boolean) : boolean
                              secondary(1)
                          secondary(x : integer) : boolean
                              x=2";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();
        }

        [Test]
        public void IfABody_UsesAVariableWhichIsNotDefinedInTheDeclaration_AnErrorIsRaised()
        {
        }

        #endregion

        #region UnaryOperators

        [Test]
        public void IfExprOfNotOperator_IsNotABoolean_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              not 1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Not operator called with expression which is not boolean"));
        }

        [Test]
        public void IfExprOfNotOperator_IsABoolean_NoErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              not false";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That((program.Definitions[0].Body.Expr as NotOperator).Right.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(new BooleanType()));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(new FunctionType(new BooleanType())));
            Assert.That(result.HasError, Is.False);
        }

        [Test]
        public void IfExprOfNegateOperator_IsNotAnInteger_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : integer
                              - true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Negate operator called with expression which is not integer"));
        }

        [Test]
        public void IfExprOfNegateOperator_IsAnInteger_NoErrorIsRaised()
        {
            // arrange
            var input = @"main() : integer
                              - 1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That((program.Definitions[0].Body.Expr as NegateOperator).Right.Type, Is.EqualTo(new IntegerType()));
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(new IntegerType()));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(new IntegerType()));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(new FunctionType(new IntegerType())));
            Assert.That(result.HasError, Is.False);
        }

        #endregion

        #region BinaryOperators

        #endregion

        // not and negate - nested errors not tested

        // error line numbers

    }
}
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
            Assert.That(program.Type, Is.EqualTo(KType.Integer));
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
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(KType.Boolean));
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
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(KType.Integer));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(KType.Integer));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(KType.Boolean));
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message,
                Is.EqualTo("Function 'main' has a type 'Boolean', but its body has a type 'Integer'"));
        }

        #endregion


        [Test]
        public void TypeChecking_TypeOfFunctionCall_ShouldBeDerivedFromDefinition_ViaTheSymbolTable()
        {
            // arrange
            var input = @"main() : boolean
                              secondary()
                          secondary() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            // assert
            Assert.That(program.Definitions[0].Body.Expr.Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[0].Body.Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[0].Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[1].Body.Expr.Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[1].Body.Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[1].Type, Is.EqualTo(KType.Boolean));
            Assert.That(program.Type, Is.EqualTo(KType.Boolean));
            Assert.That(result.HasError, Is.False);
        }



        // type of identifier should be derived from formals, via the symbol table
        // type of function call should be derviced from declaration, via the symbol table

        // must check function call exists

    }
}
using System;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class TypeCheckerTests
    {
        [Test]
        public void TypeChecking_SuperSimpleProgram_ShouldCheckTypesCorrectly()
        {
            // arrange
            var input = @"main() : boolean
                              true";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            
            // act
            var result = program.CheckType();

            Assert.That(result.HasError, Is.False);
            Assert.That(program.Definitions[0].Body.Expr.TypeExpr, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[0].Body.TypeExpr, Is.EqualTo(KType.Boolean));
            Assert.That(program.Definitions[0].TypeExpr, Is.EqualTo(KType.Boolean));
        }

        [Test]
        public void TypeChecking_IfTypeOfFunction_AndTypeItsBody_DoNotMatch_ATypeErrorIsRaised()
        {
            // arrange
            var input = @"main() : boolean
                              1";
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));

            // act
            var result = program.CheckType();

            Assert.That(program.Definitions[0].Body.Expr.TypeExpr, Is.EqualTo(KType.Integer));
            Assert.That(program.Definitions[0].Body.TypeExpr, Is.EqualTo(KType.Integer));
            Assert.That(program.Definitions[0].TypeExpr, Is.EqualTo(KType.Boolean));
            Assert.That(result.HasError, Is.True);
            Assert.That(result.Message, Is.EqualTo("Function 'main' has a type 'Boolean', but its body has a type 'Integer'"));
        }
    }
}
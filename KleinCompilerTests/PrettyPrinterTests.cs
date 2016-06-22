using System;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class PrettyPrinterTests
    {
        [Test]
        public void BinaryOperator_ShouldPrintCorrectly()
        {
            var ast = new BinaryOperator()
            {
                Left = new Identifier("x"),
                Operator = "+",
                Right = new Identifier("y")
            };

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"BinaryOperator(+)
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void KleinType_ShouldPrintCorrectly()
        {
            var ast = new KleinType("boolean");
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo("Type(boolean)\r\n"));
        }

        [Test]
        public void KleinDefinition_ShouldPrint()
        {
            var ast = new Definition()
            {
                Identifier = new Identifier("main"),
                Type = new KleinType("boolean"),
            };

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Definition
    Identifier(main)
    Type(boolean)
"));

        }
    }
}
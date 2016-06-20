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
                Left = new Identifier() {Value = "x"},
                Operator = "+",
                Right = new Identifier() {Value = "y"}
            };

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"BinaryOperator(+)
    Identifier(x)
    Identifier(y)
"));
        }
    }
}
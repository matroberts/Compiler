using System;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;
using System.Collections.Generic;

namespace KleinCompilerTests
{
    [TestFixture]
    public class PrettyPrinterTests
    {
        [Test]
        public void BinaryOperator_ShouldPrintCorrectly()
        {
            var ast = new BinaryOperator
                          (
                              left: new Identifier("x"),
                              op: "+",
                              right: new Identifier("y")
                          );

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
        public void Formal_ShouldPrintCorrectly()
        {
            var ast = new Formal(identifier: new Identifier("arg1"), type: new KleinType("boolean"));
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Formal
    Identifier(arg1)
    Type(boolean)
"                
                ));
        }
        [Test]
        public void KleinDefinition_ShouldPrint()
        {
            var ast = new Definition
                          (
                              identifier: new Identifier("main"),
                              type: new KleinType("boolean"),
                              formals: new List<Formal>
                              {
                                  new Formal(new Identifier("arg1"), new KleinType("boolean")),
                                  new Formal(new Identifier("arg2"), new KleinType("integer")),
                              }
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Definition
    Identifier(main)
    Type(boolean)
    Formals
        Formal
            Identifier(arg1)
            Type(boolean)
        Formal
            Identifier(arg2)
            Type(integer)
"));

        }
    }
}
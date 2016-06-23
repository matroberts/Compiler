﻿using System;
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
            var ast = new KleinType(KType.Boolean);
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo("Type(Boolean)\r\n"));
        }

        [Test]
        public void Formal_ShouldPrintCorrectly()
        {
            var ast = new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean));
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Formal
    Identifier(arg1)
    Type(Boolean)
"                
                ));
        }
        [Test]
        public void KleinDefinition_ShouldPrint()
        {
            var ast = new Definition
                          (
                              identifier: new Identifier("main"),
                              type: new KleinType(KType.Boolean),
                              formals: new List<Formal>
                              {
                                  new Formal(new Identifier("arg1"), new KleinType(KType.Boolean)),
                                  new Formal(new Identifier("arg2"), new KleinType(KType.Integer)),
                              }
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Definition
    Identifier(main)
    Type(Boolean)
    Formals
        Formal
            Identifier(arg1)
            Type(Boolean)
        Formal
            Identifier(arg2)
            Type(Integer)
"));

        }
    }
}
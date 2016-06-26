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
        public void KleinProgram_ShouldPrint()
        {
            var ast = new Program(
                new Definition
                (
                    identifier: new Identifier("main"),
                    type: new KleinType(KType.Boolean),
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                ),
                new Definition
                (
                    identifier: new Identifier("subsidiary"),
                    type: new KleinType(KType.Integer),
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                )
            );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Program
    Definition
        Identifier(main)
        Type(Boolean)
        Formals
        Body
            Expr
                Boolean(False)
    Definition
        Identifier(subsidiary)
        Type(Integer)
        Formals
        Body
            Expr
                Boolean(False)
"));
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
                              },
                              body: new Body(expr: new BooleanLiteral(false))
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
    Body
        Expr
            Boolean(False)
"));
        }

        [Test]
        public void KleinType_ShouldPrint()
        {
            var ast = new KleinType(KType.Boolean);
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo("Type(Boolean)\r\n"));
        }

        [Test]
        public void Formal_ShouldPrint()
        {
            var ast = new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean));
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Formal
    Identifier(arg1)
    Type(Boolean)
"));
        }

        [Test]
        public void Body_ShouldPrint()
        {
            var ast = new Body
                      (
                          new IntegerLiteral("123")
                      );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Body
    Expr
        Integer(123)
"));
        }

        [Test]
        public void IfThenElse_ShouldPrint()
        {
            var ast = new IfThenElse
                          (
                              ifExpr: new Identifier("x"),
                              thenExpr: new Identifier("y"),
                              elseExpr: new Identifier("z")
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"If
    Identifier(x)
Then
    Identifier(y)
Else
    Identifier(z)
"));
        }

        [Test]
        public void BinaryOperator_ShouldPrint()
        {
            var ast = new BinaryOperator
                          (
                              left: new Identifier("x"),
                              op: BOp.Plus,
                              right: new Identifier("y")
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"BinaryOperator(+)
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void AllTheValues_InTheBOpEnum_ShouldHaveAnOpTextAttribute()
        {
            foreach (BOp op in Enum.GetValues(typeof(BOp)).Cast<BOp>())
            {
                Assert.That(() => op.ToOpText(), Throws.Nothing);
            }
        }

        [Test]
        public void UnaryOperator_ShouldPrint()
        {
            var ast = new UnaryOperator
                          (
                              op: UOp.Not,
                              right: new Identifier("y")
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"UnaryOperator(not)
    Identifier(y)
"));
        }

        [Test]
        public void AllTheValues_InTheUOpEnum_ShouldHaveAnOpTextAttribute()
        {
            foreach (UOp op in Enum.GetValues(typeof(UOp)).Cast<UOp>())
            {
                Assert.That(() => op.ToOpText(), Throws.Nothing);
            }
        }

        [Test]
        public void BooleanLiteral_ShouldPrint()
        {
            var ast = new BooleanLiteral(true);
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Boolean(True)
"));
        }

        [Test]
        public void IntegerLiteral_ShouldPrint()
        {
            var ast = new IntegerLiteral("123");
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Integer(123)
"));
        }

        [Test]
        public void FunctionCall_ShouldPrint()
        {
            var ast = new FunctionCall(
                                            new Identifier("func"),
                                            new List<Expr>()
                                            {
                                                new Identifier("x"),
                                                new Identifier("y")
                                            } 
                                      );
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"FunctionCall(func)
    Actual
        Identifier(x)
    Actual
        Identifier(y)
"));
        }

        [Test]
        public void Actual_ShouldPrint()
        {
            var ast = new Actual(new Identifier("x"));

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Actual
    Identifier(x)
"));
        }
    }
}
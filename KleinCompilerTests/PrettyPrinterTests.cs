using System;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;
using System.Collections.Generic;
using KleinCompiler.AbstractSyntaxTree;

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
                    kleinType: new KleinType(KType.Boolean),
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                ),
                new Definition
                (
                    identifier: new Identifier("subsidiary"),
                    kleinType: new KleinType(KType.Integer),
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                )
            );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Program
    Definition(main)
        Type(Boolean)
        Formals
        Body
            Expr
                Boolean(False)
    Definition(subsidiary)
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
                              kleinType: new KleinType(KType.Boolean),
                              formals: new List<Formal>
                              {
                                  new Formal(new Identifier("arg1"), new KleinType(KType.Boolean)),
                                  new Formal(new Identifier("arg2"), new KleinType(KType.Integer)),
                              },
                              body: new Body(expr: new BooleanLiteral(false))
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Definition(main)
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
            var ast = new Formal(identifier: new Identifier("arg1"), kleinType: new KleinType(KType.Boolean));
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
        public void Body_ShouldPrint_Prints()
        {
            var ast = new Body
                      (
                          new IntegerLiteral("123"),
                          new List<Print>
                          {
                              new Print(new Identifier("x")),
                              new Print(new Identifier("y"))
                          }
                      );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Body
    Print
        Identifier(x)
    Print
        Identifier(y)
    Expr
        Integer(123)
"));
        }

        [Test]
        public void Print_ShouldPrint()
        {
            var ast = new Print
                          (
                              new IntegerLiteral("123")
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Print
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
        public void NotOperator_ShouldPrint()
        {
            var ast = new NotOperator
                          (
                              right: new Identifier("y")
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Not
    Identifier(y)
"));
        }

        [Test]
        public void NegateOperator_ShouldPrint()
        {
            var ast = new NegateOperator
                          (
                              right: new Identifier("y")
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Negate
    Identifier(y)
"));
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
                                            new List<Actual>()
                                            {
                                                new Actual(new Identifier("x")),
                                                new Actual(new Identifier("y"))
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
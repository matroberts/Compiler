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
                    typeDeclaration: new BooleanTypeDeclaration(), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                ),
                new Definition
                (
                    identifier: new Identifier("subsidiary"),
                    typeDeclaration: new IntegerTypeDeclaration(), 
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
                              typeDeclaration: new BooleanTypeDeclaration(), 
                              formals: new List<Formal>
                              {
                                  new Formal(new Identifier("arg1"), new BooleanTypeDeclaration()),
                                  new Formal(new Identifier("arg2"), new IntegerTypeDeclaration()),
                              },
                              body: new Body(expr: new BooleanLiteral(false))
                          );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Definition(main)
    Type(Boolean)
    Formals
        Formal(arg1)
            Type(Boolean)
        Formal(arg2)
            Type(Integer)
    Body
        Expr
            Boolean(False)
"));
        }

        [Test]
        public void BooleanTypeDeclaration_ShouldPrint()
        {
            var ast = new BooleanTypeDeclaration();
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo("Type(Boolean)\r\n"));
        }

        [Test]
        public void IntegerTypeDeclaration_ShouldPrint()
        {
            var ast = new IntegerTypeDeclaration();
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo("Type(Integer)\r\n"));
        }

        [Test]
        public void Formal_ShouldPrint()
        {
            var ast = new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration());
            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Formal(arg1)
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

        #region BinaryOperator

        [Test]
        public void LessThanOperator_ShouldPrint()
        {
            var ast = new LessThanOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"LessThan
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void EqualsOperator_ShouldPrint()
        {
            var ast = new EqualsOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Equals
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void OrOperator_ShouldPrint()
        {
            var ast = new OrOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Or
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void PlusOperator_ShouldPrint()
        {
            var ast = new PlusOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Plus
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void MinusOperator_ShouldPrint()
        {
            var ast = new MinusOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Minus
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void AndOperator_ShouldPrint()
        {
            var ast = new AndOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"And
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void TimesOperator_ShouldPrint()
        {
            var ast = new TimesOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Times
    Identifier(x)
    Identifier(y)
"));
        }

        [Test]
        public void DivideOperator_ShouldPrint()
        {
            var ast = new DivideOperator
                (
                position: 0,
                left: new Identifier("x"),
                right: new Identifier("y")
                );

            Assert.That(PrettyPrinter.ToString(ast), Is.EqualTo(
@"Divide
    Identifier(x)
    Identifier(y)
"));
        }

        #endregion

        [Test]
        public void NotOperator_ShouldPrint()
        {
            var ast = new NotOperator
                          (
                              position: 0,
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
                              position: 0,
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
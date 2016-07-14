using System;
using System.Collections.Generic;
using System.Linq;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class AstTests
    {
        [Test]
        public void Program_ShouldImplement_ValueEquality()
        {
            var program = new Program(
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

            Assert.That(program.Equals(null), Is.False);
            Assert.That(program.Equals(new Identifier("main")), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier("main"),
                    typeDeclaration: new BooleanTypeDeclaration(), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                )
            )), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier("wrong"),
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
            )), Is.False);

            Assert.That(program.Equals(new Program(
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
            )), Is.True);
        }

        [Test]
        public void Definition_ShouldImplement_ValueEquality()
        {
            var definition = new Definition
                                 (
                                     identifier: new Identifier("def"),
                                     typeDeclaration: new BooleanTypeDeclaration(), 
                                     formals: new List<Formal>(),
                                     body: new Body(expr: new BooleanLiteral(false))
                                 );

            Assert.That(definition.Equals(null), Is.False);
            Assert.That(definition.Equals(new Identifier("a")), Is.False);
            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("wrong"),
                                                  typeDeclaration: new BooleanTypeDeclaration(),
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(false))
                                              )), Is.False);
            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("def"),
                                                  typeDeclaration: new IntegerTypeDeclaration(), 
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(false))
                                              )), Is.False);

            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("def"),
                                                  typeDeclaration: new BooleanTypeDeclaration(), 
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(true))
                                              )), Is.False);

            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("def"),
                                                  typeDeclaration: new BooleanTypeDeclaration(), 
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(false))
                                              )), Is.True);
        }

        [Test]
        public void Definition_ValueEquality_ShouldCompareFormals()
        {
            var definition = new Definition
                             (
                                 identifier: new Identifier("def"),
                                 typeDeclaration: new BooleanTypeDeclaration(), 
                                 formals: new List<Formal>
                                 {
                                     new Formal(new Identifier("arg1"), new IntegerTypeDeclaration()),
                                     new Formal(new Identifier("arg2"), new BooleanTypeDeclaration()),
                                 },
                                 body: new Body(expr: new BooleanLiteral(false))
                             );

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              typeDeclaration: new BooleanTypeDeclaration(), 
                                              formals: new List<Formal>
                                                       {
                                                           new Formal(new Identifier("arg1"), new IntegerTypeDeclaration()),
                                                           new Formal(new Identifier("arg2"), new BooleanTypeDeclaration()),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.True);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              typeDeclaration: new BooleanTypeDeclaration(), 
                                              formals: new List<Formal>(),
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              typeDeclaration: new BooleanTypeDeclaration(), 
                                              formals: new List<Formal>
                                                       {
                                                                       new Formal(new Identifier("arg1"), new IntegerTypeDeclaration()),
                                                                       new Formal(new Identifier("arg2"), new BooleanTypeDeclaration()),
                                                                       new Formal(new Identifier("arg3"), new BooleanTypeDeclaration()),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              typeDeclaration: new BooleanTypeDeclaration(), 
                                              formals: new List<Formal>
                                                       {
                                                                       new Formal(new Identifier("wrong"), new IntegerTypeDeclaration()),
                                                                       new Formal(new Identifier("arg2"), new BooleanTypeDeclaration()),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              typeDeclaration: new BooleanTypeDeclaration(), 
                                              formals: new List<Formal>
                                                       {
                                                                                   new Formal(new Identifier("arg1"), new IntegerTypeDeclaration()),
                                                                                   new Formal(new Identifier("arg2"), new IntegerTypeDeclaration()),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);
        }

        [Test]
        public void BooleanTypeDeclaration_ShouldImplement_ValueEquality()
        {
            Assert.That(new BooleanTypeDeclaration().Equals(null), Is.False);
            Assert.That(new BooleanTypeDeclaration().Equals(new IntegerTypeDeclaration()), Is.False);
            Assert.That(new BooleanTypeDeclaration().Equals(new BooleanTypeDeclaration()), Is.True);
        }

        [Test]
        public void IntegerTypeDeclaration_ShouldImplement_ValueEquality()
        {
            Assert.That(new IntegerTypeDeclaration().Equals(null), Is.False);
            Assert.That(new IntegerTypeDeclaration().Equals(new BooleanTypeDeclaration()), Is.False);
            Assert.That(new IntegerTypeDeclaration().Equals(new IntegerTypeDeclaration()), Is.True);
        }

        [Test]
        public void Formal_ShouldImplement_ValueEquality()
        {
            Assert.That(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration()).Equals(null), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration()).Equals(new Identifier("a")), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration()).Equals(new Formal(identifier: new Identifier("wrong"), typeDeclaration: new BooleanTypeDeclaration())), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration()).Equals(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new IntegerTypeDeclaration())), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration()).Equals(new Formal(identifier: new Identifier("arg1"), typeDeclaration: new BooleanTypeDeclaration())), Is.True);
        }

        [Test]
        public void Body_ShouldImplement_ValueEquality()
        {
            var body = new Body
                       (
                           new IntegerLiteral("123")
                       );
            Assert.That(body.Equals(null), Is.False);
            Assert.That(body.Equals(new IntegerLiteral("123")), Is.False);
            Assert.That(body.Equals(new Body(new BooleanLiteral(true))), Is.False);
            Assert.That(body.Equals(new Body(new IntegerLiteral("456"))), Is.False);
            Assert.That(body.Equals(new Body(new IntegerLiteral("123"))), Is.True);
        }

        [Test]
        public void Body_ValueEquality_ShouldIncludePrints()
        {
            var body = new Body
                       (
                           new IntegerLiteral("123"),
                           new List<Print>
                           {
                               new Print(new Identifier("x")),
                               new Print(new Identifier("y"))
                           }
                       );
            Assert.That(body.Equals(new Body(new IntegerLiteral("123"))), Is.False);
            Assert.That(body.Equals(new Body
                                       (
                                           new IntegerLiteral("123"),
                                           new List<Print>
                                           {
                                               new Print(new Identifier("x")),
                                               new Print(new Identifier("wrong"))
                                           }
                                       )), Is.False);
            Assert.That(body.Equals(new Body
                                       (
                                           new IntegerLiteral("123"),
                                           new List<Print>
                                           {
                                               new Print(new Identifier("x")),
                                               new Print(new Identifier("y"))
                                           }
                                       )), Is.True);
        }

        [Test]
        public void Print_ShouldImplement_ValueEquality()
        {
            var print = new Print
                            (
                                new IntegerLiteral("123")
                            );
            Assert.That(print.Equals(null), Is.False);
            Assert.That(print.Equals(new IntegerLiteral("123")), Is.False);
            Assert.That(print.Equals(new Print(new IntegerLiteral("456"))), Is.False);
            Assert.That(print.Equals(new Print(new IntegerLiteral("123"))), Is.True);
        }

        [Test]
        public void Identifier_ShouldImplement_ValueEquality()
        {
            Assert.That(new Identifier("a").Equals(null), Is.False);
            Assert.That(new Identifier("a").Equals(new BooleanTypeDeclaration()), Is.False);
            Assert.That(new Identifier("a").Equals(new Identifier("b")), Is.False);
            Assert.That(new Identifier("a").Equals(new Identifier("a")), Is.True);
        }

        [Test]
        public void IfThenElse_ShouldImplement_ValueEquality()
        {
            var ifthenelse = new IfThenElse
                                  (
                                      ifExpr: new Identifier("x"),
                                      thenExpr: new Identifier("y"),
                                      elseExpr: new Identifier("z")
                                  );

            Assert.That(ifthenelse.Equals(null), Is.False);
            Assert.That(ifthenelse.Equals(new Identifier("x")), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              ifExpr: new Identifier("wrong"),
                                              thenExpr: new Identifier("y"),
                                              elseExpr: new Identifier("z")
                                          )), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              ifExpr: new Identifier("x"),
                                              thenExpr: new Identifier("wrong"),
                                              elseExpr: new Identifier("z")
                                          )), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              ifExpr: new Identifier("x"),
                                              thenExpr: new Identifier("y"),
                                              elseExpr: new Identifier("wrong")
                                          )), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              ifExpr: new Identifier("x"),
                                              thenExpr: new Identifier("y"),
                                              elseExpr: new Identifier("z")
                                          )), Is.True);
        }

        [Test]
        public void BinaryOperator_ShouldImplement_ValueEquality()
        {
            var binaryOperator = new BinaryOperator
                                    (
                                        left: new Identifier("left"),
                                        op: BOp.Times, 
                                        right: new Identifier("right")
                                    );

            Assert.That(binaryOperator.Equals(null), Is.False);
            Assert.That(binaryOperator.Equals(new Identifier("a")), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator
                                                  (
                                                       left: new Identifier("wrong"),
                                                       op: BOp.Times, 
                                                       right: new Identifier("right")
                                                  )), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator
                                                  (
                                                       left: new Identifier("left"),
                                                       op: BOp.Plus, 
                                                       right: new Identifier("right")
                                                  )), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator
                                                  (
                                                       left: new Identifier("left"),
                                                       op: BOp.Times, 
                                                       right: new Identifier("wrong")
                                                  )), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator
                                                  (
                                                       left: new Identifier("left"),
                                                       op: BOp.Times, 
                                                       right: new Identifier("right")
                                                  )), Is.True);
        }

        #region UnaryOperator

        [Test]
        public void NotOperator_ShouldImplement_ValueEquality()
        {
            var unaryOperatory = new NotOperator
                                     (
                                         right: new Identifier("right")
                                     );

            Assert.That(unaryOperatory.Equals(null), Is.False);
            Assert.That(unaryOperatory.Equals(new NegateOperator
                                                  (
                                                       right: new Identifier("right")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NotOperator
                                                  (
                                                       right: new Identifier("wrong")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NotOperator
                                                  (
                                                       right: new Identifier("right")
                                                  )), Is.True);
        }

        [Test]
        public void NegateOperator_ShouldImplement_ValueEquality()
        {
            var unaryOperatory = new NegateOperator
                                     (
                                         right: new Identifier("right")
                                     );

            Assert.That(unaryOperatory.Equals(null), Is.False);
            Assert.That(unaryOperatory.Equals(new NotOperator
                                                  (
                                                       right: new Identifier("right")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NegateOperator
                                                  (
                                                       right: new Identifier("wrong")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NegateOperator
                                                  (
                                                       right: new Identifier("right")
                                                  )), Is.True);
        }

        #endregion

        [Test]
        public void BooleanLiteral_ShouldImplement_ValueEquality()
        {
            Assert.That(new BooleanLiteral(true).Equals(null), Is.False);
            Assert.That(new BooleanLiteral(true).Equals(true), Is.False);
            Assert.That(new BooleanLiteral(true).Equals(new BooleanLiteral(false)), Is.False);
            Assert.That(new BooleanLiteral(true).Equals(new BooleanLiteral(true)), Is.True);
        }

        [Test]
        public void IntegerLiteral_ShouldImplement_ValueEquality()
        {
            Assert.That(new IntegerLiteral("123").Equals(null), Is.False);
            Assert.That(new IntegerLiteral("123").Equals("123"), Is.False);
            Assert.That(new IntegerLiteral("123").Equals(new IntegerLiteral("456")), Is.False);
            Assert.That(new IntegerLiteral("123").Equals(new IntegerLiteral("123")), Is.True);
        }

        [Test]
        public void FunctionCall_ShouldImplement_ValueEquality()
        {
            var functioncall = new FunctionCall(
                                    new Identifier("func"),
                                    new List<Actual>()
                                    {
                                        new Actual(new Identifier("x")),
                                        new Actual(new Identifier("y"))
                                    }
                              );

            Assert.That(functioncall.Equals(null), Is.False);
            Assert.That(functioncall.Equals(new Identifier("func")), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier("wrong"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier("x")),
                                                    new Actual(new Identifier("y"))
                                                }
                                            )), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier("func"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier("x")),
                                                }
                                            )), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier("func"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier("x")),
                                                    new Actual(new Identifier("wrong"))
                                                }
                                            )), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier("func"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier("x")),
                                                    new Actual(new Identifier("y"))
                                                }
                                            )), Is.True);
        }

        [Test]
        public void Actual_ShouldImplement_ValueEquality()
        {
            var actual = new Actual(new Identifier("x"));

            Assert.That(actual.Equals(null), Is.False);
            Assert.That(actual.Equals(new Identifier("x")), Is.False);
            Assert.That(actual.Equals(new Actual(new Identifier("y"))), Is.False);
            Assert.That(actual.Equals(new Actual(new Identifier("x"))), Is.True);
        }
    }
}
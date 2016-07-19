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
                    identifier: new Identifier(0, "main"),
                    typeDeclaration: new BooleanTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                ),
                new Definition
                (
                    identifier: new Identifier(0, "subsidiary"),
                    typeDeclaration: new IntegerTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                )
            );

            Assert.That(program.Equals(null), Is.False);
            Assert.That(program.Equals(new Identifier(0, "main")), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier(0, "main"),
                    typeDeclaration: new BooleanTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                )
            )), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier(0, "wrong"),
                    typeDeclaration: new BooleanTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                ),
                new Definition
                (
                    identifier: new Identifier(0, "subsidiary"),
                    typeDeclaration: new IntegerTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                )
            )), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier(0, "main"),
                    typeDeclaration: new BooleanTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                ),
                new Definition
                (
                    identifier: new Identifier(0, "subsidiary"),
                    typeDeclaration: new IntegerTypeDeclaration(0), 
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(0, false))
                )
            )), Is.True);
        }

        [Test]
        public void Definition_ShouldImplement_ValueEquality()
        {
            var definition = new Definition
                                 (
                                     identifier: new Identifier(0, "def"),
                                     typeDeclaration: new BooleanTypeDeclaration(0), 
                                     formals: new List<Formal>(),
                                     body: new Body(expr: new BooleanLiteral(0, false))
                                 );

            Assert.That(definition.Equals(null), Is.False);
            Assert.That(definition.Equals(new Identifier(0, "a")), Is.False);
            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier(0, "wrong"),
                                                  typeDeclaration: new BooleanTypeDeclaration(0),
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(0, false))
                                              )), Is.False);
            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier(0, "def"),
                                                  typeDeclaration: new IntegerTypeDeclaration(0), 
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(0, false))
                                              )), Is.False);

            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier(0, "def"),
                                                  typeDeclaration: new BooleanTypeDeclaration(0), 
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(0, true))
                                              )), Is.False);

            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier(0, "def"),
                                                  typeDeclaration: new BooleanTypeDeclaration(0), 
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(0, false))
                                              )), Is.True);
        }

        [Test]
        public void Definition_ValueEquality_ShouldCompareFormals()
        {
            var definition = new Definition
                             (
                                 identifier: new Identifier(0, "def"),
                                 typeDeclaration: new BooleanTypeDeclaration(0), 
                                 formals: new List<Formal>
                                 {
                                     new Formal(new Identifier(0, "arg1"), new IntegerTypeDeclaration(0)),
                                     new Formal(new Identifier(0, "arg2"), new BooleanTypeDeclaration(0)),
                                 },
                                 body: new Body(expr: new BooleanLiteral(0, false))
                             );

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier(0, "def"),
                                              typeDeclaration: new BooleanTypeDeclaration(0), 
                                              formals: new List<Formal>
                                                       {
                                                           new Formal(new Identifier(0, "arg1"), new IntegerTypeDeclaration(0)),
                                                           new Formal(new Identifier(0, "arg2"), new BooleanTypeDeclaration(0)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(0, false))
                                          )), Is.True);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier(0, "def"),
                                              typeDeclaration: new BooleanTypeDeclaration(0), 
                                              formals: new List<Formal>(),
                                              body: new Body(expr: new BooleanLiteral(0, false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier(0, "def"),
                                              typeDeclaration: new BooleanTypeDeclaration(0), 
                                              formals: new List<Formal>
                                                       {
                                                                       new Formal(new Identifier(0, "arg1"), new IntegerTypeDeclaration(0)),
                                                                       new Formal(new Identifier(0, "arg2"), new BooleanTypeDeclaration(0)),
                                                                       new Formal(new Identifier(0, "arg3"), new BooleanTypeDeclaration(0)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(0, false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier(0, "def"),
                                              typeDeclaration: new BooleanTypeDeclaration(0), 
                                              formals: new List<Formal>
                                                       {
                                                                       new Formal(new Identifier(0, "wrong"), new IntegerTypeDeclaration(0)),
                                                                       new Formal(new Identifier(0, "arg2"), new BooleanTypeDeclaration(0)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(0, false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier(0, "def"),
                                              typeDeclaration: new BooleanTypeDeclaration(0), 
                                              formals: new List<Formal>
                                                       {
                                                                                   new Formal(new Identifier(0, "arg1"), new IntegerTypeDeclaration(0)),
                                                                                   new Formal(new Identifier(0, "arg2"), new IntegerTypeDeclaration(0)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(0, false))
                                          )), Is.False);
        }

        [Test]
        public void BooleanTypeDeclaration_ShouldImplement_ValueEquality()
        {
            Assert.That(new BooleanTypeDeclaration(0).Equals(null), Is.False);
            Assert.That(new BooleanTypeDeclaration(0).Equals(new IntegerTypeDeclaration(0)), Is.False);
            Assert.That(new BooleanTypeDeclaration(0).Equals(new BooleanTypeDeclaration(0)), Is.True);
        }

        [Test]
        public void IntegerTypeDeclaration_ShouldImplement_ValueEquality()
        {
            Assert.That(new IntegerTypeDeclaration(0).Equals(null), Is.False);
            Assert.That(new IntegerTypeDeclaration(0).Equals(new BooleanTypeDeclaration(0)), Is.False);
            Assert.That(new IntegerTypeDeclaration(0).Equals(new IntegerTypeDeclaration(0)), Is.True);
        }

        [Test]
        public void Formal_ShouldImplement_ValueEquality()
        {
            Assert.That(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new BooleanTypeDeclaration(0)).Equals(null), Is.False);
            Assert.That(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new BooleanTypeDeclaration(0)).Equals(new Identifier(0, "a")), Is.False);
            Assert.That(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new BooleanTypeDeclaration(0)).Equals(new Formal(identifier: new Identifier(0, "wrong"), typeDeclaration: new BooleanTypeDeclaration(0))), Is.False);
            Assert.That(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new BooleanTypeDeclaration(0)).Equals(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new IntegerTypeDeclaration(0))), Is.False);
            Assert.That(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new BooleanTypeDeclaration(0)).Equals(new Formal(identifier: new Identifier(0, "arg1"), typeDeclaration: new BooleanTypeDeclaration(0))), Is.True);
        }

        [Test]
        public void Body_ShouldImplement_ValueEquality()
        {
            var body = new Body
                       (
                           new IntegerLiteral(0, "123")
                       );
            Assert.That(body.Equals(null), Is.False);
            Assert.That(body.Equals(new IntegerLiteral(0, "123")), Is.False);
            Assert.That(body.Equals(new Body(new BooleanLiteral(0, true))), Is.False);
            Assert.That(body.Equals(new Body(new IntegerLiteral(0, "456"))), Is.False);
            Assert.That(body.Equals(new Body(new IntegerLiteral(0, "123"))), Is.True);
        }

        [Test]
        public void Body_ValueEquality_ShouldIncludePrints()
        {
            var body = new Body
                       (
                           new IntegerLiteral(0, "123"),
                           new List<Print>
                           {
                               new Print(0, new Identifier(0, "x")),
                               new Print(0, new Identifier(0, "y"))
                           }
                       );
            Assert.That(body.Equals(new Body(new IntegerLiteral(0, "123"))), Is.False);
            Assert.That(body.Equals(new Body
                                       (
                                           new IntegerLiteral(0, "123"),
                                           new List<Print>
                                           {
                                               new Print(0, new Identifier(0, "x")),
                                               new Print(0, new Identifier(0, "wrong"))
                                           }
                                       )), Is.False);
            Assert.That(body.Equals(new Body
                                       (
                                           new IntegerLiteral(0, "123"),
                                           new List<Print>
                                           {
                                               new Print(0, new Identifier(0, "x")),
                                               new Print(0, new Identifier(0, "y"))
                                           }
                                       )), Is.True);
        }

        [Test]
        public void Print_ShouldImplement_ValueEquality()
        {
            var print = new Print
                            (
                                0,
                                new IntegerLiteral(0, "123")
                            );
            Assert.That(print.Equals(null), Is.False);
            Assert.That(print.Equals(new IntegerLiteral(0, "123")), Is.False);
            Assert.That(print.Equals(new Print(0, new IntegerLiteral(0, "456"))), Is.False);
            Assert.That(print.Equals(new Print(0, new IntegerLiteral(0, "123"))), Is.True);
        }

        [Test]
        public void Identifier_ShouldImplement_ValueEquality()
        {
            Assert.That(new Identifier(0, "a").Equals(null), Is.False);
            Assert.That(new Identifier(0, "a").Equals(new BooleanTypeDeclaration(0)), Is.False);
            Assert.That(new Identifier(0, "a").Equals(new Identifier(0, "b")), Is.False);
            Assert.That(new Identifier(0, "a").Equals(new Identifier(0, "a")), Is.True);
        }

        [Test]
        public void IfThenElse_ShouldImplement_ValueEquality()
        {
            var ifthenelse = new IfThenElse
                                  (
                                      position: 0,
                                      ifExpr: new Identifier(0, "x"),
                                      thenExpr: new Identifier(0, "y"),
                                      elseExpr: new Identifier(0, "z")
                                  );

            Assert.That(ifthenelse.Equals(null), Is.False);
            Assert.That(ifthenelse.Equals(new Identifier(0, "x")), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              position: 0,
                                              ifExpr: new Identifier(0, "wrong"),
                                              thenExpr: new Identifier(0, "y"),
                                              elseExpr: new Identifier(0, "z")
                                          )), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              position: 0,
                                              ifExpr: new Identifier(0, "x"),
                                              thenExpr: new Identifier(0, "wrong"),
                                              elseExpr: new Identifier(0, "z")
                                          )), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              position: 0,
                                              ifExpr: new Identifier(0, "x"),
                                              thenExpr: new Identifier(0, "y"),
                                              elseExpr: new Identifier(0, "wrong")
                                          )), Is.False);
            Assert.That(ifthenelse.Equals(new IfThenElse
                                          (
                                              position: 0,
                                              ifExpr: new Identifier(0, "x"),
                                              thenExpr: new Identifier(0, "y"),
                                              elseExpr: new Identifier(0, "z")
                                          )), Is.True);
        }

        #region BinaryOperator

        [Test]
        public void BinaryOperator_ShouldImplement_ValueEquality()
        {
            var binaryOperator = new TimesOperator
                (
                position: 0,
                left: new Identifier(0, "left"),
                right: new Identifier(0, "right")
                );

            Assert.That(binaryOperator.Equals(null), Is.False);
            Assert.That(binaryOperator.Equals(new Identifier(0, "a")), Is.False);
            Assert.That(binaryOperator.Equals(new TimesOperator
                (
                position: 0,
                left: new Identifier(0, "wrong"),
                right: new Identifier(0, "right")
                )), Is.False);
            Assert.That(binaryOperator.Equals(new PlusOperator
                (
                position: 0,
                left: new Identifier(0, "left"),
                right: new Identifier(0, "right")
                )), Is.False);
            Assert.That(binaryOperator.Equals(new TimesOperator
                (
                position: 0,
                left: new Identifier(0, "left"),
                right: new Identifier(0, "wrong")
                )), Is.False);
            Assert.That(binaryOperator.Equals(new TimesOperator
                (
                position: 0,
                left: new Identifier(0, "left"),
                right: new Identifier(0, "right")
                )), Is.True);
        }

        #endregion

        #region UnaryOperator

        [Test]
        public void NotOperator_ShouldImplement_ValueEquality()
        {
            var unaryOperatory = new NotOperator
                                     (
                                         position: 0,
                                         right: new Identifier(0, "right")
                                     );

            Assert.That(unaryOperatory.Equals(null), Is.False);
            Assert.That(unaryOperatory.Equals(new NegateOperator
                                                  (
                                                       position: 0,
                                                       right: new Identifier(0, "right")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NotOperator
                                                  (
                                                       position: 0,
                                                       right: new Identifier(0, "wrong")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NotOperator
                                                  (
                                                       position: 0,
                                                       right: new Identifier(0, "right")
                                                  )), Is.True);
        }

        [Test]
        public void NegateOperator_ShouldImplement_ValueEquality()
        {
            var unaryOperatory = new NegateOperator
                                     (
                                         position: 0,
                                         right: new Identifier(0, "right")
                                     );

            Assert.That(unaryOperatory.Equals(null), Is.False);
            Assert.That(unaryOperatory.Equals(new NotOperator
                                                  (
                                                       position: 0,
                                                       right: new Identifier(0, "right")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NegateOperator
                                                  (
                                                       position: 0,
                                                       right: new Identifier(0, "wrong")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new NegateOperator
                                                  (
                                                       position: 0,
                                                       right: new Identifier(0, "right")
                                                  )), Is.True);
        }

        #endregion

        [Test]
        public void BooleanLiteral_ShouldImplement_ValueEquality()
        {
            Assert.That(new BooleanLiteral(0, true).Equals(null), Is.False);
            Assert.That(new BooleanLiteral(0, true).Equals(true), Is.False);
            Assert.That(new BooleanLiteral(0, true).Equals(new BooleanLiteral(0, false)), Is.False);
            Assert.That(new BooleanLiteral(0, true).Equals(new BooleanLiteral(0, true)), Is.True);
        }

        [Test]
        public void IntegerLiteral_ShouldImplement_ValueEquality()
        {
            Assert.That(new IntegerLiteral(0, "123").Equals(null), Is.False);
            Assert.That(new IntegerLiteral(0, "123").Equals("123"), Is.False);
            Assert.That(new IntegerLiteral(0, "123").Equals(new IntegerLiteral(0, "456")), Is.False);
            Assert.That(new IntegerLiteral(0, "123").Equals(new IntegerLiteral(0, "123")), Is.True);
        }

        [Test]
        public void FunctionCall_ShouldImplement_ValueEquality()
        {
            var functioncall = new FunctionCall(
                                    new Identifier(0, "func"),
                                    new List<Actual>()
                                    {
                                        new Actual(new Identifier(0, "x")),
                                        new Actual(new Identifier(0, "y"))
                                    }
                              );

            Assert.That(functioncall.Equals(null), Is.False);
            Assert.That(functioncall.Equals(new Identifier(0, "func")), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier(0, "wrong"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier(0, "x")),
                                                    new Actual(new Identifier(0, "y"))
                                                }
                                            )), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier(0, "func"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier(0, "x")),
                                                }
                                            )), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier(0, "func"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier(0, "x")),
                                                    new Actual(new Identifier(0, "wrong"))
                                                }
                                            )), Is.False);
            Assert.That(functioncall.Equals(new FunctionCall(
                                                new Identifier(0, "func"),
                                                new List<Actual>()
                                                {
                                                    new Actual(new Identifier(0, "x")),
                                                    new Actual(new Identifier(0, "y"))
                                                }
                                            )), Is.True);
        }

        [Test]
        public void Actual_ShouldImplement_ValueEquality()
        {
            var actual = new Actual(new Identifier(0, "x"));

            Assert.That(actual.Equals(null), Is.False);
            Assert.That(actual.Equals(new Identifier(0, "x")), Is.False);
            Assert.That(actual.Equals(new Actual(new Identifier(0, "y"))), Is.False);
            Assert.That(actual.Equals(new Actual(new Identifier(0, "x"))), Is.True);
        }
    }
}
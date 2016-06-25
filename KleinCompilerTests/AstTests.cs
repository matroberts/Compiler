using System;
using System.Collections.Generic;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class AstTests
    {
        [Test]
        public void Identifier_Equals_ShouldWorkCorrectly()
        {
            Assert.That(new Identifier("a").Equals(null), Is.False);
            Assert.That(new Identifier("a").Equals(new KleinType(KType.Boolean)), Is.False);
            Assert.That(new Identifier("a").Equals(new Identifier("b")), Is.False);
            Assert.That(new Identifier("a").Equals(new Identifier("a")), Is.True);
        }

        [Test]
        public void KleinType_Equals_ShouldWorkCorrectly()
        {
            Assert.That(new KleinType(KType.Integer).Equals(null), Is.False);
            Assert.That(new KleinType(KType.Integer).Equals(new Identifier("integer")), Is.False);
            Assert.That(new KleinType(KType.Integer).Equals(new KleinType(KType.Boolean)), Is.False);
            Assert.That(new KleinType(KType.Integer).Equals(new KleinType(KType.Integer)), Is.True);
        }

        [Test]
        public void Formal_Equals_ShouldWorkCorrectly()
        {
            Assert.That(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean)).Equals(null), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean)).Equals(new Identifier("a")), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean)).Equals(new Formal(identifier: new Identifier("wrong"), type: new KleinType(KType.Boolean))), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean)).Equals(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Integer))), Is.False);
            Assert.That(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean)).Equals(new Formal(identifier: new Identifier("arg1"), type: new KleinType(KType.Boolean))), Is.True);
        }

        [Test]
        public void BinaryOperator_Equals_ShouldWorkCorrectly()
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

        [Test]
        public void UnaryOperator_Equals_ShouldWorkCorrectly()
        {
            var unaryOperatory = new UnaryOperator
                                     (
                                         op: UOp.Not,
                                         right: new Identifier("right")
                                     );

            Assert.That(unaryOperatory.Equals(null), Is.False);
            Assert.That(unaryOperatory.Equals(new Identifier("a")), Is.False);
            Assert.That(unaryOperatory.Equals(new UnaryOperator
                                                  (
                                                       op: UOp.Negate,
                                                       right: new Identifier("right")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new UnaryOperator
                                                  (
                                                       op: UOp.Not,
                                                       right: new Identifier("wrong")
                                                  )), Is.False);
            Assert.That(unaryOperatory.Equals(new UnaryOperator
                                                  (
                                                       op: UOp.Not,
                                                       right: new Identifier("right")
                                                  )), Is.True);
        }

        [Test]
        public void Definition_Equals_ShouldWorkCorrectly()
        {
            var definition = new Definition
                                 (
                                     identifier: new Identifier("def"),
                                     type: new KleinType(KType.Boolean),
                                     formals: new List<Formal>(),
                                     body: new Body(expr: new BooleanLiteral(false))
                                 );

            Assert.That(definition.Equals(null), Is.False);
            Assert.That(definition.Equals(new Identifier("a")), Is.False);
            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("wrong"),
                                                  type: new KleinType(KType.Boolean),
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(false))
                                              )), Is.False);
            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("def"),
                                                  type: new KleinType(KType.Integer),
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(false))
                                              )), Is.False);

            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("def"),
                                                  type: new KleinType(KType.Boolean),
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(true))
                                              )), Is.False);

            Assert.That(definition.Equals(new Definition
                                              (
                                                  identifier: new Identifier("def"),
                                                  type: new KleinType(KType.Boolean),
                                                  formals: new List<Formal>(),
                                                  body: new Body(expr: new BooleanLiteral(false))
                                              )), Is.True);
        }

        [Test]
        public void Definition_Equals_ShouldCompareFormals()
        {
            var definition = new Definition
                             (
                                 identifier: new Identifier("def"),
                                 type: new KleinType(KType.Boolean),
                                 formals: new List<Formal>
                                 {
                                     new Formal(new Identifier("arg1"), new KleinType(KType.Integer)),
                                     new Formal(new Identifier("arg2"), new KleinType(KType.Boolean)),
                                 },
                                 body: new Body(expr: new BooleanLiteral(false))
                             );

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              type: new KleinType(KType.Boolean),
                                              formals: new List<Formal>
                                                       {
                                                           new Formal(new Identifier("arg1"), new KleinType(KType.Integer)),
                                                           new Formal(new Identifier("arg2"), new KleinType(KType.Boolean)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.True);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              type: new KleinType(KType.Boolean),
                                              formals: new List<Formal>(),
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              type: new KleinType(KType.Boolean),
                                              formals: new List<Formal>
                                                       {
                                                                       new Formal(new Identifier("arg1"), new KleinType(KType.Integer)),
                                                                       new Formal(new Identifier("arg2"), new KleinType(KType.Boolean)),
                                                                       new Formal(new Identifier("arg3"), new KleinType(KType.Boolean)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              type: new KleinType(KType.Boolean),
                                              formals: new List<Formal>
                                                       {
                                                                       new Formal(new Identifier("wrong"), new KleinType(KType.Integer)),
                                                                       new Formal(new Identifier("arg2"), new KleinType(KType.Boolean)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);

            Assert.That(definition.Equals(new Definition
                                          (
                                              identifier: new Identifier("def"),
                                              type: new KleinType(KType.Boolean),
                                              formals: new List<Formal>
                                                       {
                                                                                   new Formal(new Identifier("arg1"), new KleinType(KType.Integer)),
                                                                                   new Formal(new Identifier("arg2"), new KleinType(KType.Integer)),
                                                       },
                                              body: new Body(expr: new BooleanLiteral(false))
                                          )), Is.False);
        }

        [Test]
        public void Program_Equals_ShouldWorkCorrectly()
        {
            var program = new Program(
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

            Assert.That(program.Equals(null), Is.False);
            Assert.That(program.Equals(new Identifier("main")), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier("main"),
                    type: new KleinType(KType.Boolean),
                    formals: new List<Formal>(),
                    body: new Body(expr: new BooleanLiteral(false))
                )
            )), Is.False);

            Assert.That(program.Equals(new Program(
                new Definition
                (
                    identifier: new Identifier("wrong"),
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
            )), Is.False);

            Assert.That(program.Equals(new Program(
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
            )), Is.True);
        }

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


    }
}
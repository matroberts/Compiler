using System;
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
            Assert.That(new Identifier() {Value = "a"}.Equals(null), Is.False);
            Assert.That(new Identifier() {Value = "a"}.Equals(new BinaryOperator()), Is.False);
            Assert.That(new Identifier() {Value = "a"}.Equals(new Identifier() {Value = "b"}), Is.False);
            Assert.That(new Identifier() {Value = "a"}.Equals(new Identifier() {Value = "a"}), Is.True);
        }

        [Test]
        public void KleinType_Equals_ShouldWorkCorrectly()
        {
            Assert.That(new KleinType() { Value = "a" }.Equals(null), Is.False);
            Assert.That(new KleinType() { Value = "a" }.Equals(new Identifier() {Value = "a"}), Is.False);
            Assert.That(new KleinType() { Value = "a" }.Equals(new KleinType() { Value = "b" }), Is.False);
            Assert.That(new KleinType() { Value = "a" }.Equals(new KleinType() { Value = "a" }), Is.True);
        }

        [Test]
        public void BinaryOperator_Equals_ShouldWorkCorrectly()
        {
            var binaryOperator = new BinaryOperator()
            {
                Left = new Identifier() { Value = "left" },
                Operator = "*",
                Right = new Identifier() {Value = "right"}
            };

            Assert.That(binaryOperator.Equals(null), Is.False);
            Assert.That(binaryOperator.Equals(new Identifier()), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator()
            {
                Left = new Identifier() { Value = "wrong" },
                Operator = "*",
                Right = new Identifier() { Value = "right" }
            }), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator()
            {
                Left = new Identifier() { Value = "left" },
                Operator = "wrong",
                Right = new Identifier() { Value = "right" }
            }), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator()
            {
                Left = new Identifier() { Value = "left" },
                Operator = "*",
                Right = new Identifier() { Value = "wrong" }
            }), Is.False);
            Assert.That(binaryOperator.Equals(new BinaryOperator()
            {
                Left = new Identifier() { Value = "left" },
                Operator = "*",
                Right = new Identifier() { Value = "right" }
            }), Is.True);
        }
        
        [Test]
        public void Definition_Equals_ShouldWorkCorrectly()
        {
            var definition = new Definition()
            {
                Identifier = new Identifier() { Value = "def" },
                Type = new KleinType() {Value = "boolean"},
            };

            Assert.That(definition.Equals(null), Is.False);
            Assert.That(definition.Equals(new Identifier()), Is.False);
            Assert.That(definition.Equals(new Definition()
            {
                Identifier = new Identifier() { Value = "wrong" },
                Type = new KleinType() { Value = "boolean" },
            }), Is.False);
            Assert.That(definition.Equals(new Definition()
            {
                Identifier = new Identifier() { Value = "def" },
                Type = new KleinType() { Value = "integer" },
            }), Is.False);

            Assert.That(definition.Equals(new Definition()
            {
                Identifier = new Identifier() { Value = "def" },
                Type = new KleinType() { Value = "boolean" },
            }), Is.True);

            // not implemented Next() yet....don't know if I should
        }
    }
}
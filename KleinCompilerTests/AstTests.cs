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
    }
}
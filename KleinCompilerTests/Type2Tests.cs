using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KleinCompiler.AbstractSyntaxTree;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class Type2Tests
    {
        [Test]
        public void BooleanType_ShouldImplement_ValueEquality()
        {
            var booleanType = new BooleanType();

            Assert.That(booleanType.Equals(null), Is.False);
            Assert.That(booleanType.Equals(new IntegerType()), Is.False);
            Assert.That(booleanType.Equals(new BooleanType()), Is.True);
        }

        [Test]
        public void IntegerType_ShouldImplement_ValueEquality()
        {
            var integerType = new IntegerType();

            Assert.That(integerType.Equals(null), Is.False);
            Assert.That(integerType.Equals(new BooleanType()), Is.False);
            Assert.That(integerType.Equals(new IntegerType()), Is.True);
        }

        [Test]
        public void FunctionType_CheckArgs_ShouldReturnFalse_IfTheNumberOfArgsIsDifferent()
        {
            var functionType = new FunctionType(new IntegerType(), new IntegerType(), new BooleanType());

            Assert.That(functionType.CheckArgs(new List<PrimitiveType> { new IntegerType(), new IntegerType(), new BooleanType() }), Is.False);
        }

        [Test]
        public void FunctionType_CheckArgs_ShouldReturnFalse_IfTheArgsAreDifferent()
        {
            var functionType = new FunctionType(new IntegerType(), new IntegerType(), new BooleanType());

            Assert.That(functionType.CheckArgs(new List<PrimitiveType> { new BooleanType(), new IntegerType(), }), Is.False);
        }

        [Test]
        public void FunctionType_CheckArgs_ShouldReturnTrue_IfTheArgsAreTheSame()
        {
            var functionType = new FunctionType(new IntegerType(), new IntegerType(), new BooleanType());

            Assert.That(functionType.CheckArgs(new List<PrimitiveType> { new IntegerType(), new BooleanType(),  }), Is.True);
        }

        [Test]
        public void FunctionType_ShouldImplement_ValueEquality()
        {
            var functionType = new FunctionType(new IntegerType(), new IntegerType(), new BooleanType());

            Assert.That(functionType.Equals(null), Is.False);
            Assert.That(functionType.Equals(new BooleanType()), Is.False);
            Assert.That(functionType.Equals(new FunctionType(new BooleanType(), new IntegerType(), new BooleanType())), Is.False);
            Assert.That(functionType.Equals(new FunctionType(new IntegerType(), new BooleanType(), new IntegerType())), Is.False);
            Assert.That(functionType.Equals(new FunctionType(new IntegerType(), new IntegerType(), new BooleanType())), Is.True);
        }

        [Test]
        public void ToString_ShouldProduceUsefulRepresentation()
        {
            Assert.That(new IntegerType().ToString(), Is.EqualTo("integer"));
            Assert.That(new BooleanType().ToString(), Is.EqualTo("boolean"));
            Assert.That(new FunctionType(new BooleanType()).ToString(), Is.EqualTo("() : boolean"));
            Assert.That(new FunctionType(new BooleanType(), new IntegerType()).ToString(), Is.EqualTo("(integer) : boolean"));
            Assert.That(new FunctionType(new BooleanType(), new IntegerType(), new BooleanType()).ToString(), Is.EqualTo("(integer, boolean) : boolean"));
        }
    }
}
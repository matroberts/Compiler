using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class StackOffsetTests
    {
        [Test]
        public void StackOffset()
        {
            var offset = new StackOffset();
            var negative = new NegativeStackOffset();

            Assert.That(offset.TopOfStack + negative.ReturnValue, Is.EqualTo(offset.ReturnValue));
            Assert.That(offset.TopOfStack + negative.Register0, Is.EqualTo(offset.Register0));
            Assert.That(offset.TopOfStack + negative.Register1, Is.EqualTo(offset.Register1));
            Assert.That(offset.TopOfStack + negative.Register2, Is.EqualTo(offset.Register2));
            Assert.That(offset.TopOfStack + negative.Register3, Is.EqualTo(offset.Register3));
            Assert.That(offset.TopOfStack + negative.Register4, Is.EqualTo(offset.Register4));
            Assert.That(offset.TopOfStack + negative.Register5, Is.EqualTo(offset.Register5));
            Assert.That(offset.TopOfStack + negative.Register6, Is.EqualTo(offset.Register6));
        }
    }
}
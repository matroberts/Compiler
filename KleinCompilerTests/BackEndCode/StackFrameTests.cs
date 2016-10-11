using System;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class StackFrameTests
    {

        [Test]
        public void NewStackFrame_IsUsedToCalculateAddresses_InANewStackFrame_ForACaller()
        {
            // new stackframe beginning at address 20, with 3 arguments
            var offset = new NewStackFrame(20,3);  
            Assert.That(offset.ReturnValue, Is.EqualTo(20));
            Assert.That(offset.Argn(0), Is.EqualTo(21));
            Assert.That(offset.Argn(1), Is.EqualTo(22));
            Assert.That(offset.Argn(2), Is.EqualTo(23));
            Assert.That(() => offset.Argn(3), Throws.Exception.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(offset.ReturnAddress, Is.EqualTo(24));
            Assert.That(offset.Register0, Is.EqualTo(25));
            Assert.That(offset.Register1, Is.EqualTo(26));
            Assert.That(offset.Register2, Is.EqualTo(27));
            Assert.That(offset.Register3, Is.EqualTo(28));
            Assert.That(offset.Register4, Is.EqualTo(29));
            Assert.That(offset.Register5, Is.EqualTo(30));
            Assert.That(offset.Register6, Is.EqualTo(31));
            Assert.That(offset.NewStackPointer, Is.EqualTo(32));
        }

        [Test]
        public void StackFrame_IsUsedToCalculateAddresses_InTheCurrentStackFrame_RelativeToTheStackFramePointer_r6()
        {
            var stackFrame = new StackFrame(3);
            Assert.That(stackFrame.Register6, Is.EqualTo(-1));
            Assert.That(stackFrame.Register5, Is.EqualTo(-2));
            Assert.That(stackFrame.Register4, Is.EqualTo(-3));
            Assert.That(stackFrame.Register3, Is.EqualTo(-4));
            Assert.That(stackFrame.Register2, Is.EqualTo(-5));
            Assert.That(stackFrame.Register1, Is.EqualTo(-6));
            Assert.That(stackFrame.Register0, Is.EqualTo(-7));
            Assert.That(stackFrame.ReturnAddress, Is.EqualTo(-8));
            Assert.That(stackFrame.Argn(2), Is.EqualTo(-9));
            Assert.That(stackFrame.Argn(1), Is.EqualTo(-10));
            Assert.That(stackFrame.Argn(0), Is.EqualTo(-11));
            Assert.That(stackFrame.ReturnValue, Is.EqualTo(-12));

            Assert.That(stackFrame.Address("t0"), Is.EqualTo(0));
            Assert.That(stackFrame.Address("t1"), Is.EqualTo(1));
            Assert.That(stackFrame.Address("t2"), Is.EqualTo(2));

            Assert.That(stackFrame.Address("arg0"), Is.EqualTo(stackFrame.Argn(0)));
            Assert.That(stackFrame.Address("arg1"), Is.EqualTo(stackFrame.Argn(1)));
            Assert.That(stackFrame.Address("arg2"), Is.EqualTo(stackFrame.Argn(2)));
        }

        [Test]
        public void NegativeStackOffset_IsUsedCalculateAddresses_InTheExistingStackFrame()
        {
            var offset = new NewStackFrame(0, 3);
            var negative = new StackFrame(3);
            
            Assert.That(offset.NewStackPointer + negative.ReturnValue, Is.EqualTo(offset.ReturnValue));
            Assert.That(offset.NewStackPointer + negative.Register0, Is.EqualTo(offset.Register0));
            Assert.That(offset.NewStackPointer + negative.Register1, Is.EqualTo(offset.Register1));
            Assert.That(offset.NewStackPointer + negative.Register2, Is.EqualTo(offset.Register2));
            Assert.That(offset.NewStackPointer + negative.Register3, Is.EqualTo(offset.Register3));
            Assert.That(offset.NewStackPointer + negative.Register4, Is.EqualTo(offset.Register4));
            Assert.That(offset.NewStackPointer + negative.Register5, Is.EqualTo(offset.Register5));
            Assert.That(offset.NewStackPointer + negative.Register6, Is.EqualTo(offset.Register6));
        }

    }
}
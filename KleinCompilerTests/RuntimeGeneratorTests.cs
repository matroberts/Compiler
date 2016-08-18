using System;
using System.IO;
using KleinCompiler.CodeGenerator;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class RuntimeGeneratorTests
    {
        [Test]
        public void Test()
        {
            int linenumber = 0;
            int functionAddress = 1000;
            string output = RuntimeGenerator.CallingProcedureCallingSequence(ref linenumber, functionAddress);

            Assert.That(linenumber, Is.EqualTo(10));
            Console.WriteLine(output);

        }

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

        [Test]
        public void TestSettingRegisters()
        {
            // 0: jump to main method
            // 1: function
            // n: main
             
            int linenumber = 1;
            string func = RuntimeGenerator.SetRegisters1To5(ref linenumber, 17) +
                          RuntimeGenerator.CalledProcedureReturnSequence(ref linenumber);
            int addressOfMain = linenumber;
            string main = RuntimeGenerator.SetRegisters1To5(ref linenumber, 19) +
                          RuntimeGenerator.CallingProcedureCallingSequence(ref linenumber, 1) +
                          RuntimeGenerator.CallingProcedureReturnSequence(ref linenumber) +
                          RuntimeGenerator.PrintRegisters(ref linenumber) + 
                          RuntimeGenerator.Halt(ref linenumber);
            string jump = RuntimeGenerator.InitialJump(addressOfMain);

            File.WriteAllText(@"C:\github\Compiler\TinyMachine\bin\Debug\Test.tm", jump+func+main);
        }
    }
}
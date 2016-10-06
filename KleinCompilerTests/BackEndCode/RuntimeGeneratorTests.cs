using System.IO;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class RuntimeGeneratorTests
    {
        public string ExeName => "TinyMachine.exe";
        public string ExePath => Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\TinyMachineGo\bin\Debug\", ExeName);

        public string TestFile => "Test.tm";
        public string TestFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, TestFile);

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
                          RuntimeGenerator.CallProcedure(ref linenumber, 1, "func") +
                          RuntimeGenerator.PrintRegisters(ref linenumber) + 
                          RuntimeGenerator.Halt(ref linenumber);
            string jump = RuntimeGenerator.InitialJump(addressOfMain);

//            File.WriteAllText(TestFilePath, jump+func+main);
//            string[] output = new TinyMachine(ExePath).Execute(TestFilePath);
//            Assert.That(output, Is.EqualTo(new [] { "0", "19", "19", "19", "19", "19", "0" }));
            // r0 = 0     because not changed to another value...this register always has zero in it
            // r1-r5 = 19 because thats what we set it too
            // r6 = 0     because this is the value of the top of the stack, when the procedure call has finised
        }
        // be able to generate programs which just call series of zero argument functions, and print stuff
    }
}
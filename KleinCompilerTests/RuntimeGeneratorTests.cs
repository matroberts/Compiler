using System;
using System.Diagnostics;
using System.IO;
using KleinCompiler.CodeGenerator;
using NUnit.Framework;

namespace KleinCompilerTests
{
    public class TinyMachine
    {
        private readonly string exePath;

        public TinyMachine(string exePath)
        {
            this.exePath = exePath;
        }

        public string[] Execute(string path)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = path,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Trim().Split(new []{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }
    }

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
            Console.WriteLine(ExePath);
            Console.WriteLine(TestFilePath);
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

            File.WriteAllText(TestFilePath, jump+func+main);
            string[] output = new TinyMachine(ExePath).Execute(TestFilePath);
            Assert.That(output, Is.EqualTo(new [] { "0", "19", "19", "19", "19", "23", "0" }));
            // r0 = 0     because not changed to another value...this register always has zero in it
            // r1-r4 = 19 because thats what we set it too
            // r5 = 23    becasue r5 is used for address manipulation, and this contained the return address of the method during the procedure call setup
            // r6 = 0     because this is the value of the top of the stack, when the procedure call has finised
        }
        // be able to generate programs which just call series of zero argument functions, and print stuff
    }
}
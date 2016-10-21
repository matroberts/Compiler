using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KleinCompiler;
using KleinCompiler.BackEndCode;
using KleinCompilerTests.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.Programs
{
    [TestFixture]
    public class FullProgramTests
    {
        public string ExeName => "TinyMachine.exe";
        public string ExePath => Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\TinyMachineGo\bin\Debug\", ExeName);

        public string TestFile => "Test.tm";
        public string TestFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, TestFile);

        [Test]
        public void Compiler_Execute_AllOfTheValidSampleKleinPrograms()
        {
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\fullprograms");
            var files = Directory.GetFiles(folder, "*.kln");
            Assert.That(files.Length, Is.GreaterThan(0));

            foreach (var file in files)
            {
                var input = File.ReadAllText(file);
                var frontEnd = new FrontEnd();
                var program = frontEnd.Compile(input);
                Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());
                var tacs = new ThreeAddressCodeFactory().Generate(program);
                var output = new CodeGenerator().Generate(tacs);

                foreach (var testDatum in TestDatum.GetTestData(input))
                {
                    Console.WriteLine($"{Path.GetFileName(file)}      {testDatum}");
                    var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, testDatum.Args);
                    Console.WriteLine(tinyOut);
                    Assert.That(tinyOut, Is.EquivalentTo(testDatum.Asserts), Path.GetFileName(file));
                }
            }
        }
    }
}
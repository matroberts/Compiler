using System;
using System.IO;
using KleinCompiler;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class ThreeAddressCodeFactoryTests
    {
        public string ExeName => "TinyMachine.exe";
        public string ExePath => Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\TinyMachineGo\bin\Debug\", ExeName);

        public string TestFile => "Test.tm";
        public string TestFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, TestFile);

        //        var input = @"main() : integer
        //                              1
        //                          secondary() : integer
        //                              tertiary()
        //                          tertiary() : integer
        //                              17";



        [Test]
        public void SimplestPossibleProgram_ShouldCallMain_AndShouldPrintMainsResult()
        {
            // arrange
            var input = @"main() : integer
                              1";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);

            // assert
            Assert.That(tacs.ToString(), Is.EqualTo(@"
Init 0 
t0 := BeginCall main 0
Call main 
t1 := BeginCall print 1
Param t0 
Call print 
Halt  

BeginFunc print 1
PrintVariable arg0 
EndFunc print 

BeginFunc main 0
t0 := 1
Return t0 
EndFunc main 
"));
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void Homework5_ProgramShouldRunCorrectly()
        {
            // arrange
            var input = @"main() : integer
                              print(1)
                              1";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());


            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);
            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1", "1" }));
        }
    }
}
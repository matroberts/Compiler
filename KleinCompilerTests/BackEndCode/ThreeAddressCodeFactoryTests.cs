using System;
using KleinCompiler;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class ThreeAddressCodeFactoryTests
    {
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
            var tac = new ThreeAddressCodeFactory().Generate(program);
            Console.WriteLine(tac);
            // assert
            Assert.That(tac.ToString(), Is.EqualTo(@"
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
        }

//        PrettyPrinter.ToConsole(program);
//            Console.WriteLine(tac.ToString());


    }
}
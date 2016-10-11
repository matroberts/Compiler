﻿using KleinCompiler;
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

            // assert
            Assert.That(tac.ToString(), Is.EqualTo(@"
BeginCall
t0 := Call main
BeginCall
Param t0
t1 := Call print
Halt 

BeginFunc print
PrintVariable arg0
Return arg0
EndFunc print

BeginFunc main
t2 := 1
Return t2
EndFunc main
"));
        }

//        PrettyPrinter.ToConsole(program);
//            Console.WriteLine(tac.ToString());


    }
}
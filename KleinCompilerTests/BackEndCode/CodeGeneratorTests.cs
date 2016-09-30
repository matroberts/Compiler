using System;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;
using KleinCompiler.BackEndCode;
using KleinCompiler.FrontEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class CodeGeneratorTests
    {
//        var input = @"main() : integer
//                              1
//                          secondary() : integer
//                              tertiary()
//                          tertiary() : integer
//                              17";

        // the result returned by main is printed to stdout
        [Test]
        public void Test()
        {
            // arrange
            var input = @"main() : integer
                              1";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            PrettyPrinter.ToConsole(program);
            var tac = new ThreeAddressCodeFactory().Generate(program);
            Console.WriteLine(tac.ToString());
            Assert.That(tac.ToString(), Is.EqualTo(@"BeginCall
t0 := Call main
BeginCall
Param t0
t1 := Call print
Stop 

Begin print
DoPrint arg0
Return arg0
End print

Begin main
t2 := 1
Return t2
End main
"));
        }
    }
}
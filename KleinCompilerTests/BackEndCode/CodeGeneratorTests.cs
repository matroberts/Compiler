using System;
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
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), new ErrorRecord(input));
            var result = program.CheckType();
            Assert.That(result.HasError, Is.False, result.Message);
            PrettyPrinter.ToConsole(program);
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            Console.WriteLine(tacs.ToString2());
            Assert.That(tacs.ToString2(), Is.EqualTo(@"BeginCall
t0 := Call main
BeginCall
Param t0
t1 := Call print
Stop 

Begin print
DoPrint arg0
Return arg0
End 

Begin main
t2 := 1
Return t2
End main
"));
        }
    }
}
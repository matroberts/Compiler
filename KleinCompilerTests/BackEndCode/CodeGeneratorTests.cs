using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class CodeGeneratorTests
    {
        public string ExeName => "TinyMachine.exe";
        public string ExePath => Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\TinyMachineGo\bin\Debug\", ExeName);

        public string TestFile => "Test.tm";
        public string TestFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, TestFile);

        [Test]
        public void CodeGenerator_ShouldFillInAddressOfFunctionCallCorrectly_InObjectCode()
        {
            // Arrange
            var tacs = new Tacs
            {
                Tac.Init(0),
                Tac.Call("main", "t0"),
                Tac.BeginFunc("main", 0),
                Tac.EndFunc("main")
            };

            // Act
            var output = new CodeGenerator().Generate(tacs);

            // Assert

            // jump to function is like this
            //   10: LDA 7, 18(0)  ; jump to function
            // where the jump is too address 18
            var jumpLineNumber = Regex.Match(output, @"LDA 7, (\d+)\(0\)").Groups[1].Value;

            // the beginning of the function looks like this:
            //   * BeginFunc 'main'
            //   18: ST 2, -9(6) ; store result of function r2, in result postion in stack frame
            // so want to find the linenumber after the begin function comment
            var functionLineNumber = Regex.Match(output, @"\* BeginFunc 'main'\r\n(\d+):").Groups[1].Value;

            Assert.That(jumpLineNumber, Is.EqualTo(functionLineNumber));
        }

        #region StackFrame Tests

        [Test]
        public void TestStackFrameReturnAddress_WhenFunctionIsCalled_ExecutionShouldJumpToTheFunction_WhenFunctionFinishes_ExectionShouldReturn()
        {
            // Arrange
            var tacs = new Tacs
            {
                Tac.Init(0),
                Tac.PrintValue(1),
                Tac.Call("main", "t0"),
                Tac.PrintValue(2),
                Tac.Halt(),
                Tac.BeginFunc("main", 0),
                Tac.PrintValue(3),
                Tac.EndFunc("main")
            };
            var output = new CodeGenerator().Generate(tacs);

            // Act
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // Assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1", "3", "2" }));
        }

        [Test]
        public void TestStackFrameRegisterState_WhenReturningFromAFunctionCall_TheRegistersShouldBeRestoredToTheirPreCallValues()
        {
            // Arrange
            var tacs = new Tacs
            {
                Tac.Init(0),
                Tac.SetRegisterValue(1, 11),
                Tac.SetRegisterValue(2, 11),
                Tac.SetRegisterValue(3, 11),
                Tac.SetRegisterValue(4, 11),
                Tac.SetRegisterValue(5, 11),
                Tac.SetRegisterValue(6, 11),
                Tac.Call("main", "t0"),
                Tac.PrintRegisters(),
                Tac.Halt(),
                Tac.BeginFunc("main", 0),
                Tac.SetRegisterValue(1, 22),
                Tac.SetRegisterValue(2, 22),
                Tac.SetRegisterValue(3, 22),
                Tac.SetRegisterValue(4, 22),
                Tac.SetRegisterValue(5, 22),
                // cant mess with r6 because this is the stack pointer
                Tac.EndFunc("main")
            };
            var output = new CodeGenerator().Generate(tacs);

            // Act
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // Assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0", "11", "11", "11", "11", "11", "11"}), tinyOut.ToString());
        }

        [Test]
        public void TestStackFrameArguments_ArgumentsToTheFunctionCallShouldBeStoredInTheStackFrame_AndBeAvailableWithinTheFunction()
        {
            // Arrange
            var tacs = new Tacs()
            {
                Tac.Init(0),
                Tac.Assign("13", "t0"),
                Tac.BeginCall(),
                Tac.Param("t0"),
                Tac.Call("print", "t1"),
                Tac.Halt(),
                Tac.BeginFunc("print", 1),
                Tac.PrintVariable("arg0"),
                Tac.EndFunc("print")
            };

            var output = new CodeGenerator().Generate(tacs);

            // Act
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // Assert
            Assert.That(tinyOut, Is.EqualTo(new[] {"13"}), tinyOut.ToString());
        }

        [Test]
        public void TestStackFrameArguments2_ArgumentsToTheFunctionCallShouldBeStoredInTheStackFrame_AndBeAvailableWithinTheFunction()
        {
            // Arrange
            var tacs = new Tacs()
            {
                Tac.Init(0),
                Tac.Assign("13", "t0"),
                Tac.Assign("17", "t1"),
                Tac.Assign("23", "t2"),
                Tac.BeginCall(),
                Tac.Param("t0"),
                Tac.Param("t1"),
                Tac.Param("t2"),
                Tac.Call("print", "t4"),
                Tac.Halt(),
                Tac.BeginFunc("print", 3),
                Tac.PrintVariable("arg0"),
                Tac.PrintVariable("arg1"),
                Tac.PrintVariable("arg2"),
                Tac.EndFunc("print")
            };

            var output = new CodeGenerator().Generate(tacs);

            // Act
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // Assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "13", "17", "23" }), tinyOut.ToString());
        }

        [Test]
        public void TestStackFrameReturnValue_TheValueReturnedFromTheFunction_ShouldBeStoredInTheStackFrame_AnIsAutomaticallyAssignedToTheReturnVariable()
        {
            // Arrange
            var tacs = new Tacs()
            {
                Tac.Init(0),
                Tac.BeginCall(),
                Tac.Call("returnthirteen", "t0"),
                Tac.PrintVariable("t0"),
                Tac.Halt(),
                Tac.BeginFunc("returnthirteen", 0),
                Tac.Assign("13", "t0"),
                Tac.Return("t0"),
                Tac.EndFunc("returnthirteen")
            };

            var output = new CodeGenerator().Generate(tacs);

            // Act
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // Assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "13" }), tinyOut.ToString());
        }

        [Test]
        public void TestStackFrameCommandLineArguments_TheInitialStackFrameIsSetUp_SoThatCommandLineArguments_FallInTheArgumentSlotsOfTheInitalStackFrame()
        {
            // Arrange
            var tacs = new Tacs()
            {
                Tac.Init(3),

                Tac.BeginCall(),
                Tac.Param("arg0"),
                Tac.Param("arg1"),
                Tac.Param("arg2"),
                Tac.Call("main", "t0"),
                Tac.BeginCall(),
                Tac.Param("t0"),
                Tac.Call("print", "t1"),
                Tac.Halt(),

                Tac.BeginFunc("print", 1),
                Tac.PrintVariable("arg0"),
                Tac.EndFunc("print"),

                Tac.BeginFunc("main", 3),
                Tac.PrintVariable("arg0"),
                Tac.PrintVariable("arg1"),
                Tac.PrintVariable("arg2"),
                Tac.Assign("13", "t0"),
                Tac.Return("t0"),
                Tac.EndFunc("main")
            };

            var output = new CodeGenerator().Generate(tacs);
            Console.WriteLine(tacs);
            Console.WriteLine(output);

            // Act
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, 29, 31, 37);

            // Assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "29", "31", "37", "13" }), tinyOut.ToString());
        }
        #endregion
    }
}
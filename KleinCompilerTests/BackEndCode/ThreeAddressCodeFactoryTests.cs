﻿using System;
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

        #region functions

        [Test]
        public void TheValueReturnedFromMain_ShouldBeSentToStdOut()
        {
            // Tests Visit Program, Definition, Body and IntegerLiteral

            // arrange
            var input = @"main() : integer
                              1";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            Console.WriteLine(tacs);
            // assert
            Assert.That(tacs.ToString(), Is.EqualTo(@"
Init 0  
BeginCall main 0 t0
t0 := Call main 
BeginCall print 1 t1
Param t0  
t1 := Call print 
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
        public void PrintExpressions_ProgramShouldSendTheirValueToStdOut()
        {
            // Tests Visit Print

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

        [Test]
        public void NestedFunctionCalls_ShouldWorkCorrectly()
        {
            // Test Visit Function Call

            // arrange
            var input = @"main() : integer
                              secondary()
                          secondary() : integer
                              tertiary()
                          tertiary() : integer
                              17";            

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);
            
            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "17" }));
        }

        [Test]
        public void ArgumentsShouldBePassedThrouh_NestedFunctionCalls()
        {
            // Tests Visit FunctionCall and Identifier
            // arrange
            var input = @"main(n : integer) : integer
                              secondary(n)
                          secondary(n: integer) : integer
                              tertiary(n)
                          tertiary(n : integer) : integer
                              n";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "19");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "19" }));
        }

        #endregion

        #region arithmatic

        [Test]
        public void Plus_ShouldAddTheTwoVaraibles()
        {
            // Tests Visit Add

            // arrange
            var input = @"main(n : integer) : integer
                              n+1";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "19");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "20" }));
        }

        [Test]
        public void Minus_ShouldSubtractTheTwoVaraibles()
        {
            // Tests Visit Minus

            // arrange
            var input = @"main(n : integer) : integer
                              n-1";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "19");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "18" }));
        }

        [Test]
        public void Times_ShouldMultiplyTheTwoVariable()
        {
            // Tests Visit Times

            // arrange
            var input = @"main(n : integer) : integer
                              n*2";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "19");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "38" }));
        }

        [Test]
        public void Divide_ShouldDivideTheTwoVariable()
        {
            // Tests Visit Times

            // arrange
            var input = @"main(n : integer) : integer
                              n/2";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "19");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "9" }));
        }

        [Test]
        public void Negate_ShouldNegateTheVariable()
        {
            // Tests Visit Negate

            // arrange
            var input = @"main(n : integer) : integer
                              -n";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "19");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "-19" }));
        }

        [Test]
        public void AllArithmaticTogether_ShouldNestAndAll_ThatAndMiraculouslyWork()
        {
            // arrange
            var input = @"main(n : integer, m : integer) : integer
                              -(n-1)/2 * (m+1)";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output, "9 11");

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "-48" }));
        }

        #endregion

        #region boolean logic

        [Test]
        public void BooleanLiteral_ShouldBeAssigned()
        {
            // arrange
            var input = @"main() : boolean
                              true";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void Equality_IfTheNumbersAreDifferent_0_ShouldBeReturned()
        {
            // arrange
            var input = @"main() : boolean
                              1 = 0";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0" }));
        }

        [Test]
        public void Equality_IfTheNumbersAreTheSame_1_ShouldBeReturned()
        {
            // arrange
            var input = @"main() : boolean
                              2 = 2";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void LessThan_IfLeftIsLessThanRight_1_ShouldBeReturned()
        {
            // arrange
            var input = @"main() : boolean
                              7 < 9";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void LessThan_IfLeftEqualToRight_0_ShouldBeReturned()
        {
            // arrange
            var input = @"main() : boolean
                              7 < 7";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0" }));
        }

        [Test]
        public void Not_IfArgIsTrue_0_ShouldBeReturned()
        {
            // arrange
            var input = @"main() : boolean
                              not true";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0" }));
        }

        [Test]
        public void Not_IfArgIsFalse_1_ShouldBeReturned()
        {
            // arrange
            var input = @"main() : boolean
                              not false";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void And_IfLeftIsFalse_ShouldNotEvaluateRight_AndReturn0()
        {
            // arrange
            var input = @"main() : boolean
                              false and true";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0" }));
        }

        [Test]
        public void And_IfRightIsFalse_AndReturn0()
        {
            // arrange
            var input = @"main() : boolean
                              true and false";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0" }));
        }

        [Test]
        public void And_LeftAndRightAreTrue_AndReturn1()
        {
            // arrange
            var input = @"main() : boolean
                              true and true";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void Or_IfLeftIsTrue_ShouldNotEvaluateRight_AndReturn1()
        {
            // arrange
            var input = @"main() : boolean
                              true or false";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void Or_IfRightIsTrue_ShouldReturn1()
        {
            // arrange
            var input = @"main() : boolean
                              false or true";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "1" }));
        }

        [Test]
        public void Or_IfBothAreFalse_ShouldReturn0()
        {
            // arrange
            var input = @"main() : boolean
                              false or false";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "0" }));
        }

        [Test]
        public void IfThenElse_IfConditionTrue_ThenBranchShouldExecute()
        {
            // arrange
            var input = @"main() : integer
                              if true then
                                  17
                               else
                                  19";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            Console.WriteLine(tacs);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "17" }));
        }

        [Test]
        public void IfThenElse_IfConditionFalse_ElseBranchShouldExecute()
        {
            // arrange
            var input = @"main() : integer
                              if false then
                                  17
                               else
                                  19";

            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(input);
            Assert.That(program, Is.Not.Null, frontEnd.ErrorRecord.ToString());

            // act
            var tacs = new ThreeAddressCodeFactory().Generate(program);
            Console.WriteLine(tacs);
            var output = new CodeGenerator().Generate(tacs);
            var tinyOut = new TinyMachine(ExePath, TestFilePath).Execute(output);

            // assert
            Assert.That(tinyOut, Is.EqualTo(new[] { "19" }));
        }

        #endregion
    }
}
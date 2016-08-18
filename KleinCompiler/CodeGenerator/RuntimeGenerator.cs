using System;

namespace KleinCompiler.CodeGenerator
{
    public class RuntimeGenerator
    {
        public static string CallingProcedureCallingSequence(ref int lineNumber, int addressOfFunction)
        {
            var offset = new StackOffset();
            return $@"
*                 ; Calling Procedure Calling Sequence
{lineNumber++}: LDA 5, 9(7)  ; put return address in r5. 9 = offset, i.e. end of these instructions
{lineNumber++}: ST 0, {offset.Register0}(6)  ; store values of registers in stack frame
{lineNumber++}: ST 1, {offset.Register1}(6)
{lineNumber++}: ST 2, {offset.Register2}(6)
{lineNumber++}: ST 3, {offset.Register3}(6)
{lineNumber++}: ST 4, {offset.Register4}(6)
{lineNumber++}: ST 5, {offset.Register5}(6)
{lineNumber++}: ST 6, {offset.Register6}(6)
{lineNumber++}: LDA 6, {offset.TopOfStack}(6)  ; set value of status (r6) to one past top of new stack frame
{lineNumber++}: LDA 7, {addressOfFunction}(0)  ; jump to function. i.e.load r7 with address of function (r0 == 0)
";
        }

        public static string CalledProcedureReturnSequence(ref int lineNumber)
        {
            var offset = new NegativeStackOffset();
            return $@"
*                 ; Called Procedure Return Sequence
{lineNumber++}: ST 2, {offset.ReturnValue}(6) ; store result of function r2, in result postion in stack frame
{lineNumber++}: LD 7, {offset.Register5}(6) ; jump to caller.  i.e.load r7 with address of caller from stack frame
";
        }

        public static string CallingProcedureReturnSequence(ref int lineNumber)
        {
            var offset = new NegativeStackOffset();
            return $@"
*                 ; Calling Procedure Return Sequence
{lineNumber++}: LD 0, {offset.Register0}(6)  ; load values of registers from stack frame
{lineNumber++}: LD 1, {offset.Register1}(6)
{lineNumber++}: LD 2, {offset.Register2}(6)
{lineNumber++}: LD 3, {offset.Register3}(6)
{lineNumber++}: LD 4, {offset.Register4}(6)
{lineNumber++}: LD 5, {offset.Register5}(6)
{lineNumber++}: LD 6, {offset.Register6}(6)  ; r6 stack position gets loaded last
";
        }

        public static string SetRegisters1To5(ref int lineNumber, int value)
        {
            return $@"
{lineNumber++}: LDC 1, {value}(0) ; set registers 1-5 to value {value}
{lineNumber++}: LDC 2, {value}(0)
{lineNumber++}: LDC 3, {value}(0)
{lineNumber++}: LDC 4, {value}(0)
{lineNumber++}: LDC 5, {value}(0)
";
        }

        public static string Halt(ref int lineNumber)
        {
            return $@"
{lineNumber++}: HALT 0,0,0
";
        }

        public static string PrintRegisters(ref int lineNumber)
        {
            return $@"
{lineNumber++}: OUT 0,0,0 ; output the values of all the registers
{lineNumber++}: OUT 1,0,0
{lineNumber++}: OUT 2,0,0
{lineNumber++}: OUT 3,0,0
{lineNumber++}: OUT 4,0,0
{lineNumber++}: OUT 5,0,0
{lineNumber++}: OUT 6,0,0
";
        }

        public static string InitialJump(int address)
        {
            return $@"
0: LDA 7, {address}(0) ; initial jump to address {address} (main)
";
        }
    }

    public class StackOffset
    {
        public int ReturnValue => 0;
        public int Register0 => 1;
        public int Register1 => 2;
        public int Register2 => 3;
        public int Register3 => 4;
        public int Register4 => 5;
        public int Register5 => 6;
        public int Register6 => 7;
        public int TopOfStack => Register6+1;  // one past the top of the stack

        public int Negative => 0;
    }

    public class NegativeStackOffset
    {
        private StackOffset offset;
        public NegativeStackOffset()
        {
            offset = new StackOffset();
        }

        public int ReturnValue => CalcOffset(offset.ReturnValue);
        public int Register0 => CalcOffset(offset.Register0);
        public int Register1 => CalcOffset(offset.Register1);
        public int Register2 => CalcOffset(offset.Register2);
        public int Register3 => CalcOffset(offset.Register3);
        public int Register4 => CalcOffset(offset.Register4);
        public int Register5 => CalcOffset(offset.Register5);
        public int Register6 => CalcOffset(offset.Register6);

        private int CalcOffset(int positiveOffset)
        {
            return -(offset.TopOfStack - positiveOffset);
        }
    }
}
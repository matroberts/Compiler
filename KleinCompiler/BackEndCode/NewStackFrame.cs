using System;

namespace KleinCompiler.BackEndCode
{

    // StackOffset is used by the caller to calculate address offsets in the new stack from
    public class NewStackFrame
    {
        private readonly int newStackOffset;
        private readonly int numberArguments;

        public NewStackFrame(int newStackOffset, int numberArguments)
        {
            this.newStackOffset = newStackOffset;
            this.numberArguments = numberArguments;
        }

        public int ReturnValue => newStackOffset;
        public int Argn(int n)
        {
            if(n >= numberArguments)
                throw new ArgumentOutOfRangeException($"StackFrame has '{numberArguments}' arguments (0-based), you asked for argument '{n}'");
            return newStackOffset + 1 + n;
        }
        public int ReturnAddress => newStackOffset + numberArguments +1;
        public int Register0 => newStackOffset + numberArguments + 2;
        public int Register1 => newStackOffset + numberArguments + 3;
        public int Register2 => newStackOffset + numberArguments + 4;
        public int Register3 => newStackOffset + numberArguments + 5;
        public int Register4 => newStackOffset + numberArguments + 6;
        public int Register5 => newStackOffset + numberArguments + 7;
        public int Register6 => newStackOffset + numberArguments + 8;
        public int NewStackPointer => newStackOffset + numberArguments + 9;  // one past the top of the stack
    }

    // NegativeStackOffset is used inside a function to find values in the current stack frame
    public class StackFrame
    {
        private NewStackFrame offset;
        public StackFrame(int numberArguments)
        {
            offset = new NewStackFrame(0, numberArguments);
        }

        public int ReturnValue => CalcOffset(offset.ReturnValue);
        public int Argn(int n) => CalcOffset(offset.Argn(n));
        public int ReturnAddress => CalcOffset(offset.ReturnAddress);
        public int Register0 => CalcOffset(offset.Register0);
        public int Register1 => CalcOffset(offset.Register1);
        public int Register2 => CalcOffset(offset.Register2);
        public int Register3 => CalcOffset(offset.Register3);
        public int Register4 => CalcOffset(offset.Register4);
        public int Register5 => CalcOffset(offset.Register5);
        public int Register6 => CalcOffset(offset.Register6);

        private int CalcOffset(int positiveOffset)
        {
            return -(offset.NewStackPointer - positiveOffset);
        }

        public int Address(string variable)
        {
            if (variable.StartsWith("t"))
            {
                return int.Parse(variable.TrimStart('t'));
            }
            else if (variable.StartsWith("arg"))
            {
                return Argn(int.Parse(variable.Substring("arg".Length)));
            }
            else
            {
                throw new ArgumentException("Unknown type of variable 'variable'.  Variables must start with 't' or 'arg'");
            }
        }
    }
}
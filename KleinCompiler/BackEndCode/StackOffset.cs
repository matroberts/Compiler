namespace KleinCompiler.BackEndCode
{
    public class StackOffset
    {
        public int ReturnValue => 0;
        public int ReturnAddress => 1;
        public int Register0 => 2;
        public int Register1 => 3;
        public int Register2 => 4;
        public int Register3 => 5;
        public int Register4 => 6;
        public int Register5 => 7;
        public int Register6 => 8;
        public int TopOfStack => Register6+1;  // one past the top of the stack
    }

    public class NegativeStackOffset
    {
        private StackOffset offset;
        public NegativeStackOffset()
        {
            offset = new StackOffset();
        }

        public int ReturnValue => CalcOffset(offset.ReturnValue);
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
            return -(offset.TopOfStack - positiveOffset);
        }
    }
}
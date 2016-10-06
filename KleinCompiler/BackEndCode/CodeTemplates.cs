namespace KleinCompiler.BackEndCode
{
    public class CodeTemplates
    {
        public static string Halt(ref int lineNumber)
        {
            return $@"{lineNumber++}: HALT 0,0,0
";
        }

        public static string DoPrintConst(ref int lineNumber, string value)
        {
            return $@"
{lineNumber++}: LDC 1, {value}(0)     ; load constant into r1
{lineNumber++}: OUT 1, 0, 0           ; write out contens of r1
";
        }

        public static string BeginFunc(ref int lineNumber, string name)
        {
            return $@"
* BeginFunc '{name}'
";
        }

        public static string EndFunc(ref int lineNumber, string name)
        {
            var offset = new NegativeStackOffset();
            return $@"{lineNumber++}: ST 2, {offset.ReturnValue}(6) ; store result of function r2, in result postion in stack frame
{lineNumber++}: LD 7, {offset.ReturnAddress}(6) ; jump to caller.  i.e.load r7 with address of caller from stack frame
* EndFunc '{name}'
";
        }

        public static string Call(ref int lineNumber, string name)
        {
            var offset = new StackOffset();
            var noffset = new NegativeStackOffset();

            return $@"
* Call '{name}'
{lineNumber++}: ST 0, {offset.Register0}(6)   ; store registers in stack frame
{lineNumber++}: ST 1, {offset.Register1}(6)
{lineNumber++}: ST 2, {offset.Register2}(6)
{lineNumber++}: ST 3, {offset.Register3}(6)
{lineNumber++}: ST 4, {offset.Register4}(6)
{lineNumber++}: ST 5, {offset.Register5}(6)
{lineNumber++}: ST 6, {offset.Register6}(6)
{lineNumber++}: LDA 5, 3(7)  ; calculate address of **return point** 
{lineNumber++}: ST 5, {offset.ReturnAddress}(6)   ; store return address in stack frame
{lineNumber++}: LDA 6, {offset.TopOfStack}(6)  ; set value of status (r6) to one past top of new stack frame
{lineNumber++}: LDA 7, [{name}](0)  ; jump to function
{lineNumber++}: LD 0, {noffset.Register0}(6)  ; **return point**,  restore registers from stack frame
{lineNumber++}: LD 1, {noffset.Register1}(6)
{lineNumber++}: LD 2, {noffset.Register2}(6)
{lineNumber++}: LD 3, {noffset.Register3}(6)
{lineNumber++}: LD 4, {noffset.Register4}(6)
{lineNumber++}: LD 5, {noffset.Register5}(6)
{lineNumber++}: LD 6, {noffset.Register6}(6)  ; r6 stack position gets loaded last
* End Call '{name}'
";
        }

        public static string SetRegisterValue(ref int lineNumber, string register, string value)
        {
            return $@"{lineNumber++}: LDC {register}, {value}(0) ; set registers {register} to value {value}
";
        }

        public static string PrintRegisters(ref int lineNumber)
        {
            return $@"* output the values of all the registers
{lineNumber++}: OUT 0,0,0 
{lineNumber++}: OUT 1,0,0
{lineNumber++}: OUT 2,0,0
{lineNumber++}: OUT 3,0,0
{lineNumber++}: OUT 4,0,0
{lineNumber++}: OUT 5,0,0
{lineNumber++}: OUT 6,0,0
";
        }
    }
}
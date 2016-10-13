namespace KleinCompiler.BackEndCode
{
    public class CodeTemplates
    {
        public static string Halt(ref int lineNumber)
        {
            return $@"{lineNumber++}: HALT 0,0,0
";
        }

        public static string PrintValue(ref int lineNumber, string value)
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

        public static string Return(ref int lineNumber, StackFrame stackFrame, string variable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(variable)}(6) ; return variable '{variable}'
{lineNumber++}: ST 2, {stackFrame.ReturnValue}(6)
";
        }

        public static string EndFunc(ref int lineNumber, StackFrame stackFrame, string name)
        {
            return $@"{lineNumber++}: LD 7, {stackFrame.ReturnAddress}(6) ; jump to caller
* EndFunc '{name}'
";
        }

        public static string SetRegisterValue(ref int lineNumber, int register, int value)
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

        public static string Assign(ref int lineNumber, StackFrame stackFrame, string variable, string value)
        {
            return $@"{lineNumber++}: LDC 2, {value}(0) ; {variable} := {value}   
{lineNumber++}: ST 2, {stackFrame.Address(variable)}(6) 
";
        }

        public static string BeginCall(string functionName)
        {
            return $@"* BeginCall '{functionName}'
";
        }

        public static string Param(ref int lineNumber, StackFrame stackFrame, NewStackFrame newStackFrame, string variable, int argNum)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(variable)}(6) ; param{argNum} := {variable}
{lineNumber++}: ST 2, {newStackFrame.Argn(argNum)}(6)
";
        }

        public static string Call(ref int lineNumber, NewStackFrame newStackFrame, StackFrame calleeStackFrame, string name)
        {
            return $@"* Call '{name}'
{lineNumber++}: ST 0, {newStackFrame.Register0}(6)   ; store registers in stack frame
{lineNumber++}: ST 1, {newStackFrame.Register1}(6)
{lineNumber++}: ST 2, {newStackFrame.Register2}(6)
{lineNumber++}: ST 3, {newStackFrame.Register3}(6)
{lineNumber++}: ST 4, {newStackFrame.Register4}(6)
{lineNumber++}: ST 5, {newStackFrame.Register5}(6)
{lineNumber++}: ST 6, {newStackFrame.Register6}(6)
{lineNumber++}: LDA 5, 3(7)  ; calculate address of **return point** 
{lineNumber++}: ST 5, {newStackFrame.ReturnAddress}(6)   ; store return address in stack frame
{lineNumber++}: LDA 6, {newStackFrame.NewStackPointer}(6)  ; set value of stack pointer (r6) to one past top of new stack frame
{lineNumber++}: LDA 7, [{name}](0)  ; jump to function
{lineNumber++}: LD 0, {calleeStackFrame.Register0}(6)  ; **return point**,  restore registers from stack frame
{lineNumber++}: LD 1, {calleeStackFrame.Register1}(6)
{lineNumber++}: LD 2, {calleeStackFrame.Register2}(6)
{lineNumber++}: LD 3, {calleeStackFrame.Register3}(6)
{lineNumber++}: LD 4, {calleeStackFrame.Register4}(6)
{lineNumber++}: LD 5, {calleeStackFrame.Register5}(6)
{lineNumber++}: LD 6, {calleeStackFrame.Register6}(6)  ; r6 stack pointer gets loaded last
* End Call '{name}'
";
        }

        public static string PrintVariable(ref int lineNumber, StackFrame stackFrame, string variable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(variable)}(6) ; print variable '{variable}'
{lineNumber++}: OUT 2, 0, 0
";
        }
    }
}
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

        public static string AssignConst(ref int lineNumber, StackFrame stackFrame, string variable, string value)
        {
            return $@"{lineNumber++}: LDC 2, {value}(0) ; {variable} := {value}   
{lineNumber++}: ST 2, {stackFrame.Address(variable)}(6) 
";
        }

        public static string AssignVariable(ref int lineNumber, StackFrame stackFrame, string lhs, string rhs)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(rhs)}(6) ; {lhs} := {rhs}   
{lineNumber++}: ST 2, {stackFrame.Address(lhs)}(6) 
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
{lineNumber++}: LDA 7, [func:{name}](0)  ; jump to function
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

        public static string Plus(ref int lineNumber, StackFrame stackFrame, string leftOperand, string rightOperand, string resultVariable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(leftOperand)}(6) ; {resultVariable} := {leftOperand} + {rightOperand}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: ADD 2, 2, 3
{lineNumber++}: ST 2, {stackFrame.Address(resultVariable)}(6)
";
        }

        public static string Minus(ref int lineNumber, StackFrame stackFrame, string leftOperand, string rightOperand, string resultVariable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(leftOperand)}(6) ; {resultVariable} := {leftOperand} - {rightOperand}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: SUB 2, 2, 3
{lineNumber++}: ST 2, {stackFrame.Address(resultVariable)}(6)
";
        }

        public static string Times(ref int lineNumber, StackFrame stackFrame, string leftOperand, string rightOperand, string resultVariable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(leftOperand)}(6) ; {resultVariable} := {leftOperand} * {rightOperand}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: MUL 2, 2, 3
{lineNumber++}: ST 2, {stackFrame.Address(resultVariable)}(6)
";
        }

        public static string Divide(ref int lineNumber, StackFrame stackFrame, string leftOperand, string rightOperand, string resultVariable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(leftOperand)}(6) ; {resultVariable} := {leftOperand} / {rightOperand}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: DIV 2, 2, 3
{lineNumber++}: ST 2, {stackFrame.Address(resultVariable)}(6)
";
        }

        public static string Negate(ref int lineNumber, StackFrame stackFrame, string rightOperand, string resultVariable)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(rightOperand)}(6) ; {resultVariable} := - {rightOperand}
{lineNumber++}: SUB 2, 0, 2
{lineNumber++}: ST 2, {stackFrame.Address(resultVariable)}(6)
";
        }

        public static string Goto(ref int lineNumber, string label)
        {
            return $@"{lineNumber++}: LDA 7, [label:{label}](0)  ; goto {label}
";
        }

        public static string Label(ref int lineNumber, string label)
        {
            return $@"* label:{label}
";
        }

        public static string IfEqual(ref int lineNumber, StackFrame stackFrame, string leftOperand, string rightOperand, string label)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(leftOperand)}(6) ; If {leftOperand} = {rightOperand} Goto {label}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: SUB 2, 2, 3
{lineNumber++}: JEQ 2, [label:{label}](0)
";
        }

        public static string IfLessThan(ref int lineNumber, StackFrame stackFrame, string leftOperand, string rightOperand, string label)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(leftOperand)}(6) ; If {leftOperand} < {rightOperand} Goto {label}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: SUB 2, 2, 3
{lineNumber++}: JLT 2, [label:{label}](0)
";
        }

        public static string Not(ref int lineNumber, StackFrame stackFrame, string rightOperand, string resultVariable)
        {
            return $@"{lineNumber++}: LDC 2, 1(0) ; {resultVariable} := not {rightOperand}
{lineNumber++}: LD 3, {stackFrame.Address(rightOperand)}(6)
{lineNumber++}: SUB 2, 2, 3
{lineNumber++}: ST 2, {stackFrame.Address(resultVariable)}(6)
";
        }

        public static string IfFalse(ref int lineNumber, StackFrame stackFrame, string operand, string label)
        {
            return $@"{lineNumber++}: LD 2, {stackFrame.Address(operand)}(6) ; IfFalse {operand} Goto {label}
{lineNumber++}: JEQ 2, [label:{label}](0)
";
        }
    }
}
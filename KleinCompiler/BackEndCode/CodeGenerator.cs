using System;
using System.Collections.Generic;
using System.Text;

namespace KleinCompiler.BackEndCode
{
    public class CodeGenerator
    {
        public string Generate(Tacs tacs)
        {
            var symbolTable = new Dictionary<string, object>();
            int lineNumber = 0;
            var sb = new StringBuilder();
            StackFrame stackFrame = null;
            NewStackFrame newStackFrame = null;
            int argNum = 0;

            for (int index = 0; index < tacs.Count; index++)
            {
                var tac = tacs[index];
                switch (tac.Operation)
                {
                    case Tac.Op.Init:
                        // make the initial stack frame line up with the command line arguments
                        var numArgs = int.Parse(tac.Arg1);
                        stackFrame = new StackFrame(numArgs);
                        sb.Append(CodeTemplates.SetRegisterValue(ref lineNumber, 6, new NewStackFrame(0, numArgs).NewStackPointer));
                        break;
                    case Tac.Op.Halt:
                        sb.Append(CodeTemplates.Halt(ref lineNumber));
                        break;
                    case Tac.Op.BeginFunc:
                        symbolTable.Add(tac.Arg1, lineNumber);
                        stackFrame = new StackFrame(int.Parse(tac.Arg2));
                        sb.Append(CodeTemplates.BeginFunc(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.Return:
                        sb.Append(CodeTemplates.Return(ref lineNumber, stackFrame, tac.Arg1));
                        break;
                    case Tac.Op.EndFunc:
                        sb.Append(CodeTemplates.EndFunc(ref lineNumber, stackFrame, tac.Arg1));
                        stackFrame = null;
                        break;
                    case Tac.Op.BeginCall:
                        argNum = 0;
                        newStackFrame = new NewStackFrame(stackFrame.Address(LookAheadAndGetCallReturnVariable(tacs, index)), int.Parse(tac.Arg2));
                        sb.Append(CodeTemplates.BeginCall(tac.Arg1));
                        break;
                    case Tac.Op.Param:
                        sb.Append(CodeTemplates.Param(ref lineNumber, stackFrame, newStackFrame, tac.Arg1, argNum));
                        argNum++;
                        break;
                    case Tac.Op.Call:
                        sb.Append(CodeTemplates.Call(ref lineNumber, newStackFrame, tac.Arg1, argNum));
                        break;
                    case Tac.Op.Assign:
                        sb.Append(CodeTemplates.Assign(ref lineNumber, stackFrame, tac.Result, tac.Arg1));
                        break;
                    case Tac.Op.PrintVariable:
                        sb.Append(CodeTemplates.PrintVariable(ref lineNumber, stackFrame, tac.Arg1));
                        break;
                    case Tac.Op.SetRegisterValue:
                        sb.Append(CodeTemplates.SetRegisterValue(ref lineNumber, int.Parse(tac.Arg1), int.Parse(tac.Arg2)));
                        break;
                    case Tac.Op.PrintValue:
                        sb.Append(CodeTemplates.PrintValue(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.PrintRegisters:
                        sb.Append(CodeTemplates.PrintRegisters(ref lineNumber));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(tac.Operation.ToString());
                }
            }

            // fill in the addresses of the function calls from the symbol table
            return TemplateEngine.Render(sb.ToString(), symbolTable);
        }

        private string LookAheadAndGetCallReturnVariable(Tacs tacs, int index)
        {
            for (int i = index; i < tacs.Count; i++)
            {
                if (tacs[i].Operation == Tac.Op.Call)
                    return tacs[i].Result;
            }
            throw new ArgumentException($"Could not find a 'Call' operation, following the 'Param' statement at index '{index}'.");
        }
    }
}
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
            StackFrame calleeStackFrame = null;
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
                        newStackFrame = new NewStackFrame(stackFrame.Address(tac.Result), int.Parse(tac.Arg2));
                        calleeStackFrame = new StackFrame(int.Parse(tac.Arg2));
                        sb.Append(CodeTemplates.BeginCall(tac.Arg1));
                        break;
                    case Tac.Op.Param:
                        sb.Append(CodeTemplates.Param(ref lineNumber, stackFrame, newStackFrame, tac.Arg1, argNum));
                        argNum++;
                        break;
                    case Tac.Op.Call:
                        sb.Append(CodeTemplates.Call(ref lineNumber, newStackFrame, calleeStackFrame, tac.Arg1));
                        newStackFrame = null;
                        calleeStackFrame = null;
                        break;
                    case Tac.Op.Assign:
                        if (tac.Arg1.StartsWith("arg") || tac.Arg1.StartsWith("t"))
                        {
                            sb.Append(CodeTemplates.AssignVariable(ref lineNumber, stackFrame, tac.Result, tac.Arg1));
                        }
                        else
                        {
                            sb.Append(CodeTemplates.AssignConst(ref lineNumber, stackFrame, tac.Result, tac.Arg1));
                        }
                        break;
                    case Tac.Op.Plus:
                        sb.Append(CodeTemplates.Plus(ref lineNumber, stackFrame, tac.Arg1, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.Minus:
                        sb.Append(CodeTemplates.Minus(ref lineNumber, stackFrame, tac.Arg1, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.Times:
                        sb.Append(CodeTemplates.Times(ref lineNumber, stackFrame, tac.Arg1, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.Divide:
                        sb.Append(CodeTemplates.Divide(ref lineNumber, stackFrame, tac.Arg1, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.Negate:
                        sb.Append(CodeTemplates.Negate(ref lineNumber, stackFrame, tac.Arg2, tac.Result));
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
                    case Tac.Op.Goto:
                        sb.Append(CodeTemplates.Goto(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.Label:
                        sb.Append(CodeTemplates.Label(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.IfEqual:
                        sb.Append(CodeTemplates.IfEqual(ref lineNumber, stackFrame, tac.Arg1, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.IfLessThan:
                        sb.Append(CodeTemplates.IfLessThan(ref lineNumber, stackFrame, tac.Arg1, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.Not:
                        sb.Append(CodeTemplates.Not(ref lineNumber, stackFrame, tac.Arg2, tac.Result));
                        break;
                    case Tac.Op.IfFalse:
                        sb.Append(CodeTemplates.IfFalse(ref lineNumber, stackFrame, tac.Arg2, tac.Result));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(tac.Operation.ToString());
                }
            }
            var template = sb.ToString();
            var labels = LabelFinder.FindLabels(template);
            return template.RenderFunctions(symbolTable).RenderLabels(labels);
        }
    }
}
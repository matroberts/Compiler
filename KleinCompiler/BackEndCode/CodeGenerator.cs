﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KleinCompiler.BackEndCode
{
    public class CodeGenerator
    {
        public string Generate(Tacs tacs)
        {
            var symbolTable = new Dictionary<string, object>();
            StackFrame stackFrame = null;
            int lineNumber = 0;
            int numberArguments = 0;
            var sb = new StringBuilder();
            for (int index = 0; index < tacs.Count; index++)
            {
                var tac = tacs[index];
                switch (tac.Operation)
                {
                    case Tac.Op.Halt:
                        sb.Append(CodeTemplates.Halt(ref lineNumber));
                        break;
                    case Tac.Op.BeginFunc:
                        symbolTable.Add(tac.Arg1, lineNumber);
                        stackFrame = new StackFrame(int.Parse(tac.Arg2));
                        sb.Append(CodeTemplates.BeginFunc(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.EndFunc:
                        sb.Append(CodeTemplates.EndFunc(ref lineNumber, stackFrame, tac.Arg1));
                        stackFrame = null;
                        break;
                    case Tac.Op.BeginCall:
                        numberArguments = 0;
                        sb.Append(CodeTemplates.BeginCall());
                        break;
                    case Tac.Op.Param:
                        numberArguments++;
                        var returnVariable = LookAheadAndGetCallReturnVariable(tacs, index);
                        sb.Append(CodeTemplates.Param(ref lineNumber, tac.Arg1, numberArguments, returnVariable));
                        break;
                    case Tac.Op.Call:
                        sb.Append(CodeTemplates.Call(ref lineNumber, tac.Arg1, numberArguments, tac.Result));
                        break;
                    case Tac.Op.Assign:
                        sb.Append(CodeTemplates.Assign(ref lineNumber, tac.Result, tac.Arg1));
                        break;
                    case Tac.Op.PrintVariable:
                        sb.Append(CodeTemplates.PrintVariable(ref lineNumber, stackFrame, tac.Arg1));
                        break;
                    case Tac.Op.SetRegisterValue:
                        sb.Append(CodeTemplates.SetRegisterValue(ref lineNumber, tac.Arg1, tac.Arg2));
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
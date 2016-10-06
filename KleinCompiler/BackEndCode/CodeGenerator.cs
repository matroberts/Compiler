using System;
using System.Collections.Generic;
using System.Text;

namespace KleinCompiler.BackEndCode
{
    public class CodeGenerator
    {
        private int lineNumber = 0;
        private Dictionary<string, object> symbolTable = new Dictionary<string, object>();
        public string Generate(Tacs tacs)
        {
            var sb = new StringBuilder();
            foreach (var tac in tacs)
            {
                switch (tac.Operation)
                {
                    case Tac.Op.Halt:
                        sb.Append(CodeTemplates.Halt(ref lineNumber));
                        break;
                    case Tac.Op.BeginFunc:
                        symbolTable.Add(tac.Arg1, lineNumber);
                        sb.Append(CodeTemplates.BeginFunc(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.EndFunc:
                        sb.Append(CodeTemplates.EndFunc(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.Return:
                        break;
                    case Tac.Op.BeginCall:
                        break;
                    case Tac.Op.Param:
                        break;
                    case Tac.Op.Call:
                        sb.Append(CodeTemplates.Call(ref lineNumber, tac.Arg1));
                        break;
                    case Tac.Op.Assign:
                        break;
                    case Tac.Op.DoPrint:
                        break;
                    case Tac.Op.DoPrintConst:
                        sb.Append(CodeTemplates.DoPrintConst(ref lineNumber, tac.Arg1));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // do some template action
            return TemplateEngine.Render(sb.ToString(), symbolTable);
        }
    }
}
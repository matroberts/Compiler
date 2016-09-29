using System;
using System.Collections.Generic;
using System.Linq;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler.BackEndCode
{
    public class ThreeAddressCodeFactory : IAstVisitor
    {
        private List<Tac> threeAddressCodes = new List<Tac>();

        private int tempCounter = 0;
        private string MakeNewTemp() => $"t{tempCounter++}";

        public List<Tac> Generate(Ast ast)
        {
            ast.Accept(this);
            return threeAddressCodes;
        }

        public void Visit(Program program)
        {
            // call main
            threeAddressCodes.Add(new Tac(Op.BeginCall, null, null, null));
            var mainResult = MakeNewTemp();
            threeAddressCodes.Add(new Tac(Op.Call, "main", null, mainResult));

            // send result of main to print
            threeAddressCodes.Add(new Tac(Op.BeginCall, null, null, null));
            threeAddressCodes.Add(new Tac(Op.Param, mainResult, null, null));
            threeAddressCodes.Add(new Tac(Op.Call, "print", null, MakeNewTemp()));
            threeAddressCodes.Add(new Tac(Op.Stop, null, null, null));

            // declare print function
            threeAddressCodes.Add(new Tac(Op.Begin, "print", null, null));
            threeAddressCodes.Add(new Tac(Op.DoPrint, "arg0", null, null));
            threeAddressCodes.Add(new Tac(Op.Return, "arg0", null, null));
            threeAddressCodes.Add(new Tac(Op.End, null, null, null));

            foreach (var definition in program.Definitions)
            {
                definition.Accept(this);
            }
        }

        public void Visit(Definition definition)
        {
            threeAddressCodes.Add(new Tac(Op.Begin, definition.Name, null, null));
            definition.Body.Accept(this);
            threeAddressCodes.Add(new Tac(Op.End, definition.Name, null, null));
        }

        public void Visit(Body body)
        {
            foreach (var print in body.Prints)
            {
                //TODO
            }
            body.Expr.Accept(this);
            threeAddressCodes.Add(new Tac(Op.Return, threeAddressCodes.Last().Result, null, null));
        }

        public void Visit(Formal node)
        {
            throw new NotImplementedException();
        }

        public void Visit(BooleanTypeDeclaration node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntegerTypeDeclaration node)
        {
            throw new NotImplementedException();
        }

        public void Visit(Print node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfThenElse node)
        {
            throw new NotImplementedException();
        }

        public void Visit(LessThanOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(EqualsOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(OrOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(PlusOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(MinusOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(AndOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(TimesOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(DivideOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NotOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NegateOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(Identifier node)
        {
            throw new NotImplementedException();
        }

        public void Visit(BooleanLiteral node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntegerLiteral literal)
        {
            threeAddressCodes.Add(new Tac(Op.Assign, literal.Value.ToString(), null, MakeNewTemp()));
        }

        public void Visit(FunctionCall node)
        {
            throw new NotImplementedException();
        }

        public void Visit(Actual node)
        {
            throw new NotImplementedException();
        }
    }

    /*
     *   Three Address Code
     *   ==================
     *   Op        arg1            arg2          result       notes
     *   --------  --------------  ------------  -----------  ----------
     *   Stop      null            null          null         exit the program
     *   Begin     name            null          null         marks the beginning of a function 'name'
     *   End       name            null          null         marks the end of a function 'name'
     *   Return    t               null          null         write t into the stack frames return slot
     *   BeginCall null            null          null         marks the beginning of a function call
     *   Param     t               null          null         put t into the new functions stack frame
     *   Call      name            null          t            jump to the function, put the result in t
     *   Recieve   t                                          copy result from stack frame into t
     *   Assign    t1              null          t2           t1 variable or constant, t2 result in a variable
     */


    public struct Tac
    {
        public Tac(Op operation, string arg1, string arg2, string result)
        {
            Operation = operation;
            Result = result;
            Arg1 = arg1;
            Arg2 = arg2;
        }

        public Op Operation { get; }
        public string Result { get; }
        public string Arg1 { get; }
        public string Arg2 { get; }
        public override string ToString()
        {
            switch (Operation)
            {
                case Op.Begin:
                    return $"\r\n{Operation} {Arg1}";
                case Op.End:
                case Op.Param:
                case Op.Return:
                case Op.Stop:
                case Op.DoPrint:
                    return $"{Operation} {Arg1}";
                case Op.Assign:
                    return $"{Result} := {Arg1}";
                case Op.BeginCall:
                    return $"{Operation}";
                case Op.Call:
                    return $"{Result} := {Operation} {Arg1}";
                default:
                    return $"{Result} := {Arg1} {Operation} {Arg2}";
            }
        }
    }

    public enum Op
    {
        Stop,
        Begin,
        End,
        Return,
        BeginCall,
        Param,
        Call,
        Assign,
        DoPrint
    }
}
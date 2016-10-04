using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler.BackEndCode
{
    public class ThreeAddressCodeFactory : IAstVisitor
    {
        private ThreeAddressCode ThreeAddressCode = new ThreeAddressCode();

        private int tempCounter = 0;
        private string MakeNewTemp() => $"t{tempCounter++}";

        public ThreeAddressCode Generate(Ast ast)
        {
            ast.Accept(this);
            return ThreeAddressCode;
        }

        public void Visit(Program program)
        {
            // call main
            ThreeAddressCode.BeginCall();
            var mainResult = MakeNewTemp();
            ThreeAddressCode.Call("main", mainResult);

            // send result of main to print
            ThreeAddressCode.BeginCall();
            ThreeAddressCode.Param(mainResult);
            ThreeAddressCode.Call("print", MakeNewTemp());
            ThreeAddressCode.Halt();

            // declare print function
            ThreeAddressCode.BeginFunc("print");
            ThreeAddressCode.DoPrint("arg0");
            ThreeAddressCode.Return("arg0");
            ThreeAddressCode.EndFunc("print");

            foreach (var definition in program.Definitions)
            {
                definition.Accept(this);
            }
        }

        public void Visit(Definition definition)
        {
            ThreeAddressCode.BeginFunc(definition.Name);
            definition.Body.Accept(this);
            ThreeAddressCode.EndFunc(definition.Name);
        }

        public void Visit(Body body)
        {
            foreach (var print in body.Prints)
            {
                //TODO
            }
            body.Expr.Accept(this);
            ThreeAddressCode.Return(ThreeAddressCode.Tacs.Last().Result);
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
            ThreeAddressCode.Assign(literal.Value.ToString(), MakeNewTemp());
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

    public class ThreeAddressCode
    {
        public List<Tac> Tacs { get; } = new List<Tac>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            foreach (var tac in Tacs)
            {
                sb.AppendLine(tac.ToString());
            }
            return sb.ToString();
        }

        public void BeginFunc(string name)
        {
            Tacs.Add(new Tac(Op.BeginFunc, name, null, null));
        }

        public void EndFunc(string name)
        {
            Tacs.Add(new Tac(Op.EndFunc, name, null, null));
        }

        public void Return(string variableName)
        {
            Tacs.Add(new Tac(Op.Return, variableName, null, null));
        }

        public void BeginCall()
        {
            Tacs.Add(new Tac(Op.BeginCall, null, null, null));
        }

        public void Call(string functionName, string returnVariable)
        {
            Tacs.Add(new Tac(Op.Call, functionName, null, returnVariable));
        }

        public void Param(string variableName)
        {
            Tacs.Add(new Tac(Op.Param, variableName, null, null));
        }

        public void Halt()
        {
            Tacs.Add(new Tac(Op.Halt, null, null, null));
        }

        public void DoPrint(string arg0)
        {
            Tacs.Add(new Tac(Op.DoPrint, arg0, null, null));
        }

        public void Assign(string variableOrConstant, string returnVariable)
        {
            Tacs.Add(new Tac(Op.Assign, variableOrConstant, null, returnVariable));
        }
    }

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
                case Op.BeginFunc:
                    return $"\r\n{Operation} {Arg1}";
                case Op.EndFunc:
                case Op.Param:
                case Op.Return:
                case Op.Halt:
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
        Halt,
        BeginFunc,
        EndFunc,
        Return,
        BeginCall,
        Param,
        Call,
        Assign,
        DoPrint
    }
}
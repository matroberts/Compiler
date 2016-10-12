﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler.BackEndCode
{
    public class ThreeAddressCodeFactory : IAstVisitor
    {
        private Tacs tacs = new Tacs();

        private int tempCounter = 0;
        private string MakeNewTemp() => $"t{tempCounter++}";

        public Tacs Generate(Ast ast)
        {
            ast.Accept(this);
            return tacs;
        }

        public void Visit(Program program)
        {
            // init is a special op code which calls main with the command line arguments, then calls print on the result
            var main = program.Definitions.Single(d => d.Name == "main");
            tacs.Add(Tac.Init(main.Name, main.Formals.Count));
            tacs.Add(Tac.Halt());

            // declare print function
            tacs.Add(Tac.BeginFunc("print", 1));
            tacs.Add(Tac.PrintVariable("arg0"));
            tacs.Add(Tac.EndFunc("print"));

            foreach (var definition in program.Definitions)
            {
                definition.Accept(this);
            }
        }

        public void Visit(Definition definition)
        {
            // temps are local to the function so reset the counter for each func
            // (makes it easy to work out where the temp is stored in the stack frame)
            tempCounter = 0;
            tacs.Add(Tac.BeginFunc(definition.Name, definition.Formals.Count));
            definition.Body.Accept(this);
            tacs.Add(Tac.EndFunc(definition.Name));
        }

        public void Visit(Body body)
        {
            foreach (var print in body.Prints)
            {
                //TODO
            }
            body.Expr.Accept(this);
            tacs.Add(Tac.Return(tacs.Last().Result));
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
            tacs.Add(Tac.Assign(literal.Value.ToString(), MakeNewTemp()));
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

    public class Tacs : List<Tac>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            foreach (var tac in this)
            {
                sb.AppendLine(tac.ToString());
            }
            return sb.ToString();
        }
    }

    public struct Tac
    {
        public enum Op
        {
            Init,
            Halt,
            BeginFunc,
            EndFunc,
            Return,
            BeginCall,
            Param,
            Call,
            Assign,
            PrintVariable, 
            PrintValue,       // For testing, print the value of the const passed in arg0
            SetRegisterValue, // For testing, sets the value of the register in arg0, to the value in arg1
            PrintRegisters,   // For testing, prints out the values of all the registers
        }

        public static Tac Init(string functionName, int numberOfArguments) => new Tac(Op.Init, functionName, numberOfArguments.ToString(), null);
        public static Tac Halt() => new Tac(Op.Halt, null, null, null);
        public static Tac BeginFunc(string name, int numberArgs) => new Tac(Op.BeginFunc, name, numberArgs.ToString(), null);
        public static Tac EndFunc(string name) => new Tac(Op.EndFunc, name, null, null);
        public static Tac Return(string variable) => new Tac(Op.Return, variable, null, null);
        public static Tac BeginCall() => new Tac(Op.BeginCall, null, null, null);
        public static Tac Call(string functionName, string returnVariable) => new Tac(Op.Call, functionName, null, returnVariable);
        public static Tac Param(string variable) => new Tac(Op.Param, variable, null, null);
        public static Tac Assign(string variableOrConstant, string returnVariable) => new Tac(Op.Assign, variableOrConstant, null, returnVariable);
        public static Tac PrintVariable(string variable) => new Tac(Op.PrintVariable, variable, null, null);
        public static Tac PrintValue(int value) => new Tac(Op.PrintValue, value.ToString(), null, null);
        public static Tac SetRegisterValue(int register, int value) => new Tac(Op.SetRegisterValue, register.ToString(), value.ToString(), null);
        public static Tac PrintRegisters() => new Tac(Op.PrintRegisters, null, null, null);

        private Tac(Op operation, string arg1, string arg2, string result)
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
                    return $"\r\n{Operation} {Arg1} {Arg2}";
                case Op.EndFunc:
                case Op.Param:
                case Op.Return:
                case Op.Halt:
                case Op.PrintVariable:
                case Op.PrintValue:
                case Op.Init:
                    return $"{Operation} {Arg1} {Arg2}";
                case Op.Assign:
                    return $"{Result} := {Arg1}";
                case Op.BeginCall:
                case Op.PrintRegisters:
                    return $"{Operation}";
                case Op.Call:
                    return $"{Result} := {Operation} {Arg1}";
                case Op.SetRegisterValue:
                    return $"r{Arg1} := {Arg2}";
                default:
                    return $"{Result} := {Arg1} {Operation} {Arg2}";
            }
        }
    }


}
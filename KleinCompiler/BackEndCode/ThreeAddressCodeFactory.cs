using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler.BackEndCode
{
    public class ThreeAddressCodeFactory : IAstVisitor
    {
        private Tacs tacs = new Tacs();

        private int tempCounter = 0;
        private string MakeNewTemp() => $"t{tempCounter++}";
        private int labelCounter = 0;
        private string MakeNewLabel() => $"label{labelCounter++}";

        public Tacs Generate(Ast ast)
        {
            ast.Accept(this);
            return tacs;
        }

        public void Visit(Program program)
        {
            // the init op code sets up the call stack, to read command line arguments
            var main = program.Definitions.Single(d => d.Name == "main");
            tacs.Add(Tac.Init(main.Formals.Count));
            // call main
            tacs.Add(Tac.BeginCall(main.Name, main.Formals.Count, "t0"));
            for (int i = 0; i < main.Formals.Count; i++)
            {
                tacs.Add(Tac.Param($"arg{i}"));
            }
            tacs.Add(Tac.Call("main", "t0"));
            // call print
            tacs.Add(Tac.BeginCall("print", 1, "t1"));
            tacs.Add(Tac.Param("t0"));
            tacs.Add(Tac.Call("print", "t1"));
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
            Ast.SymbolTable.CurrentFunction = definition.Name;
            tacs.Add(Tac.BeginFunc(definition.Name, definition.Formals.Count));
            definition.Body.Accept(this);
            tacs.Add(Tac.EndFunc(definition.Name));
        }

        public void Visit(Body body)
        {
            foreach (var print in body.Prints)
            {
                print.Accept(this);
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

        public void Visit(Print print)
        {
            print.Expr.Accept(this);
            var temp = tacs.Last().Result;
            var returnVariable = MakeNewTemp();
            tacs.Add(Tac.BeginCall("print", 1, returnVariable));
            tacs.Add(Tac.Param(temp));
            tacs.Add(Tac.Call("print", returnVariable));
        }

        public void Visit(IfThenElse node)
        {
            throw new NotImplementedException();
        }

        public void Visit(LessThanOperator lessThan)
        {
            lessThan.Left.Accept(this);
            var leftOperand = tacs.Last().Result;
            lessThan.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            var result = MakeNewTemp();
            var label1 = MakeNewLabel();
            var label2 = MakeNewLabel();
            tacs.Add(Tac.IfLessThan(leftOperand, rightOperand, label1));
            tacs.Add(Tac.Assign("0", result));
            tacs.Add(Tac.Goto(label2));
            tacs.Add(Tac.Label(label1, result));
            tacs.Add(Tac.Assign("1", result));
            tacs.Add(Tac.Label(label2, result));
        }

        public void Visit(EqualsOperator equals)
        {
            equals.Left.Accept(this);
            var leftOperand = tacs.Last().Result;
            equals.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            var result = MakeNewTemp();
            var label1 = MakeNewLabel();
            var label2 = MakeNewLabel();
            tacs.Add(Tac.IfEqual(leftOperand, rightOperand, label1));
            tacs.Add(Tac.Assign("0", result));
            tacs.Add(Tac.Goto(label2));
            tacs.Add(Tac.Label(label1, result));
            tacs.Add(Tac.Assign("1", result));
            tacs.Add(Tac.Label(label2, result));
        }

        public void Visit(OrOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(PlusOperator plus)
        {
            plus.Left.Accept(this);
            var leftOperand = tacs.Last().Result;
            plus.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            tacs.Add(Tac.Plus(leftOperand, rightOperand, MakeNewTemp()));
        }

        public void Visit(MinusOperator minus)
        {
            minus.Left.Accept(this);
            var leftOperand = tacs.Last().Result;
            minus.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            tacs.Add(Tac.Minus(leftOperand, rightOperand, MakeNewTemp()));
        }

        public void Visit(AndOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(TimesOperator times)
        {
            times.Left.Accept(this);
            var leftOperand = tacs.Last().Result;
            times.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            tacs.Add(Tac.Times(leftOperand, rightOperand, MakeNewTemp()));
        }

        public void Visit(DivideOperator divide)
        {
            divide.Left.Accept(this);
            var leftOperand = tacs.Last().Result;
            divide.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            tacs.Add(Tac.Divide(leftOperand, rightOperand, MakeNewTemp()));
        }

        public void Visit(NotOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NegateOperator negate)
        {
            negate.Right.Accept(this);
            var rightOperand = tacs.Last().Result;
            tacs.Add(Tac.Negate(rightOperand, MakeNewTemp()));
        }

        public void Visit(Identifier identifier)
        {
            var arg = $"arg{Ast.SymbolTable.ArgumentNumber(identifier.Value)}";
            tacs.Add(Tac.Assign(arg, MakeNewTemp()));
        }

        public void Visit(BooleanLiteral booleanLiteral)
        {
            var booleanValue = booleanLiteral.Value ? "1" : "0";
            tacs.Add(Tac.Assign(booleanValue, MakeNewTemp()));
        }

        public void Visit(IntegerLiteral literal)
        {
            tacs.Add(Tac.Assign(literal.Value.ToString(), MakeNewTemp()));
        }

        public void Visit(FunctionCall functionCall)
        {
            var args = new List<string>();
            foreach (var actual in functionCall.Actuals)
            {
                actual.Expr.Accept(this);
                args.Add(tacs.Last().Result);
            }

            var returnValue = MakeNewTemp();
            tacs.Add(Tac.BeginCall(functionCall.Name, functionCall.Actuals.Count, returnValue));
            foreach (var arg in args)
            {
                tacs.Add(Tac.Param(arg));
            }
            tacs.Add(Tac.Call(functionCall.Name, returnValue));
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

            Plus,
            Minus,
            Times,
            Divide,
            Negate,

            PrintVariable,
            PrintValue,       // For testing, print the value of the const passed in arg0
            SetRegisterValue, // For testing, sets the value of the register in arg0, to the value in arg1
            PrintRegisters,   // For testing, prints out the values of all the registers

            IfEqual,
            Goto,
            Label,
            IfLessThan
        }

        public static Tac Init(int numberOfArguments) => new Tac(Op.Init, numberOfArguments.ToString(), null, null);
        public static Tac Halt() => new Tac(Op.Halt, null, null, null);
        public static Tac BeginFunc(string name, int numberArgs) => new Tac(Op.BeginFunc, name, numberArgs.ToString(), null);
        public static Tac EndFunc(string name) => new Tac(Op.EndFunc, name, null, null);
        public static Tac Return(string variable) => new Tac(Op.Return, variable, null, null);
        public static Tac BeginCall(string functionName, int numberArguments, string returnVariable) => new Tac(Op.BeginCall, functionName, numberArguments.ToString(), returnVariable);
        public static Tac Call(string functionName, string returnVariable) => new Tac(Op.Call, functionName, null, returnVariable);
        public static Tac Param(string variable) => new Tac(Op.Param, variable, null, null);
        public static Tac Assign(string variableOrConstant, string returnVariable) => new Tac(Op.Assign, variableOrConstant, null, returnVariable);

        public static Tac Plus(string leftOperand, string rightOperand, string result) => new Tac(Op.Plus, leftOperand, rightOperand, result);
        public static Tac Minus(string leftOperand, string rightOperand, string result) => new Tac(Op.Minus, leftOperand, rightOperand, result);
        public static Tac Times(string leftOperand, string rightOperand, string result) => new Tac(Op.Times, leftOperand, rightOperand, result);
        public static Tac Divide(string leftOperand, string rightOperand, string result) => new Tac(Op.Divide, leftOperand, rightOperand, result);
        public static Tac Negate(string rightOperand, string result) => new Tac(Op.Negate, null, rightOperand, result);

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
                case Op.BeginCall:
                case Op.Goto:
                case Op.Label:
                    return $"{Operation} {Arg1} {Arg2} {Result}";
                case Op.Assign:
                    return $"{Result} := {Arg1}";
                case Op.PrintRegisters:
                    return $"{Operation}";
                case Op.Call:
                    return $"{Result} := {Operation} {Arg1} {Arg2}";
                case Op.SetRegisterValue:
                    return $"r{Arg1} := {Arg2}";
                case Op.IfEqual:
                case Op.IfLessThan:
                    return $"{Arg1} {Operation} {Arg2} goto {Result}";
                default:
                    return $"{Result} := {Arg1} {Operation} {Arg2}";
            }
        }

        public static Tac IfEqual(string leftOperand, string rightOperand, string label) => new Tac(Op.IfEqual, leftOperand, rightOperand, label);

        public static Tac Goto(string label) => new Tac(Op.Goto, label, null, null);

        public static Tac Label(string label, string variable) => new Tac(Op.Label, label, null, variable);

        public static Tac IfLessThan(string leftOperand, string rightOperand, string label) => new Tac(Op.IfLessThan, leftOperand, rightOperand, label);
    }
}
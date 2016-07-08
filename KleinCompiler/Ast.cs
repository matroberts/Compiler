﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler
{
    public class TypeValidationResult
    {
        public static TypeValidationResult Valid(KType type)
        {
            return new TypeValidationResult(type);
        }

        public static TypeValidationResult Invalid(string message)
        {
            return new TypeValidationResult(message);
        }

        private TypeValidationResult(string message)
        {
            Message = message;
            Type = null;
        }

        private TypeValidationResult(KType type)
        {
            Message = String.Empty;
            Type = type;
        }

        public KType? Type { get; }
        public bool HasError => Type == null;
        public string Message { get; }
    }

    public abstract class Ast
    {
        public override bool Equals(object obj)
        {
            var token = obj as Ast;
            if (token == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return true;
        }

        public abstract void Accept(IAstVisitor visior);

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public abstract TypeValidationResult CheckType();

        private KType? typeExpr;
        public KType TypeExpr
        {
            get
            {
                if (typeExpr == null)
                {
                    var result = CheckType();
                    if(result.HasError)
                        throw new Exception(result.Message);
                    typeExpr = result.Type;
                }
                return typeExpr.Value;
            }
            protected set { typeExpr = value; }
        }
    }

    #region Declaration

    public class Program : Ast
    {
        public Program(List<Definition> definitions)
        {
            Definitions = definitions.AsReadOnly();
        }

        public Program(params Definition[] definitions)
        {
            Definitions = definitions.ToList().AsReadOnly();
        }

        public ReadOnlyCollection<Definition> Definitions { get; }

        public override bool Equals(object obj)
        {
            var program = obj as Program;
            if (program == null)
                return false;

            if (Definitions.Count.Equals(program.Definitions.Count) == false)
                return false;

            for (int i = 0; i < Definitions.Count; i++)
            {
                if (Definitions[i].Equals(program.Definitions[i]) == false)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            foreach (var definition in Definitions)
            {
                var result = definition.CheckType();
                if (result.HasError)
                    return result;
            }
            // type of program should be type of main
            return TypeValidationResult.Valid(KType.Boolean);
        }
    }

    public class Definition : Ast
    {
        public Definition(Identifier identifier, KleinType type, List<Formal> formals, Body body)
        {
            Identifier = identifier;
            Type = type;
            Body = body;
            Formals = formals.AsReadOnly();
        }
        public Identifier Identifier { get; }
        public KleinType Type { get; }
        public Body Body { get; }
        public ReadOnlyCollection<Formal> Formals { get; }

        public override bool Equals(object obj)
        {
            var definition = obj as Definition;
            if (definition == null)
                return false;

            if (Identifier.Equals(definition.Identifier) == false)
                return false;

            if (Type.Equals(definition.Type) == false)
                return false;

            if (Formals.Count.Equals(definition.Formals.Count) == false)
                return false;

            for (int i = 0; i < Formals.Count; i++)
            {
                if (Formals[i].Equals(definition.Formals[i]) == false)
                    return false;
            }

            // checks for null here are due to the declaration grammar tests, which does not have a body
            if (Body != null && definition.Body != null && Body.Equals(definition.Body) == false)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Identifier.Value})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            TypeExpr = Type.Value;
            var result = Body.CheckType();
            if (result.HasError)
                return result;

            if (this.TypeExpr != Body.TypeExpr)
            {
                return TypeValidationResult.Invalid($"Function '{Identifier.Value}' has a type '{this.TypeExpr}', but its body has a type '{Body.TypeExpr}'");
            }
            return TypeValidationResult.Valid(TypeExpr);
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class Formal : Ast
    {
        public Formal(Identifier identifier, KleinType type)
        {
            Type = type;
            Identifier = identifier;
        }

        public KleinType Type { get; }
        public Identifier Identifier { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Formal;
            if (node == null)
                return false;

            if (this.Type.Equals(node.Type) == false)
                return false;
            if (this.Identifier.Equals(node.Identifier) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}({Identifier.Value})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Body : Ast
    {
        public Body(Expr expr) : this (expr, new List<Print>())
        {
        }

        public Body(Expr expr, List<Print> prints)
        {
            Expr = expr;
            Prints = prints.AsReadOnly();
        }

        public Expr Expr { get; }

        public ReadOnlyCollection<Print> Prints { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Body;
            if (node == null)
                return false;

            if (this.Expr.Equals(node.Expr) == false)
                return false;

            if (this.Prints.Count.Equals(node.Prints.Count) == false)
                return false;

            for (int i = 0; i < Prints.Count; i++)
            {
                if (Prints[i].Equals(node.Prints[i]) == false)
                    return false;
            }

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            // prints

            var result = Expr.CheckType();
            if (result.HasError)
                return result;

            TypeExpr = result.Type.Value;

            return TypeValidationResult.Valid(TypeExpr);
        }
    }

    public class Print : Ast
    {
        public Print(Expr expr)
        {
            Expr = expr;
        }

        public Expr Expr { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Print;
            if (node == null)
                return false;

            if (this.Expr.Equals(node.Expr) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum KType
    {
        Integer,
        Boolean
    }
    public class KleinType : Ast
    {
        public KleinType(KType value)
        {
            Value = value;
        }
        public KType Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as KleinType;
            if (node == null)
                return false;

            if (this.Value.Equals(node.Value) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Value})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion

    #region Expression

    public abstract class Expr : Ast
    {
    }

    public class IfThenElse : Expr
    {
        public IfThenElse(Expr ifExpr, Expr thenExpr, Expr elseExpr)
        {
            IfExpr = ifExpr;
            ThenExpr = thenExpr;
            ElseExpr = elseExpr;
        }

        public Expr IfExpr { get; }
        public Expr ThenExpr { get; }
        public Expr ElseExpr { get; }

        public override bool Equals(object obj)
        {
            var node = obj as IfThenElse;
            if (node == null)
                return false;

            if (IfExpr.Equals(node.IfExpr) == false)
                return false;

            if (ThenExpr.Equals(node.ThenExpr) == false)
                return false;

            if (ElseExpr.Equals(node.ElseExpr) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum BOp
    {
        [OpText("<")]
        LessThan,
        [OpText("=")]
        Equals,
        [OpText("or")]
        Or,
        [OpText("+")]
        Plus,
        [OpText("-")]
        Minus,
        [OpText("and")]
        And,
        [OpText("*")]
        Times,
        [OpText("/")]
        Divide
    }
    public class BinaryOperator : Expr
    {
        public BinaryOperator(Expr left, BOp op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
        public Expr Left { get; }
        public BOp Operator { get; }
        public Expr Right { get; }

        public override bool Equals(object obj)
        {
            var node = obj as BinaryOperator;
            if (node == null)
                return false;

            if (Operator.Equals(node.Operator) == false)
                return false;

            if (Left.Equals(node.Left) == false)
                return false;

            if (Right.Equals(node.Right) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}({Operator})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum UOp
    {
        [OpText("not")]
        Not,
        [OpText("-")]
        Negate
    }
    public class UnaryOperator : Expr
    {
        public UnaryOperator(UOp op, Expr right)
        {
            Operator = op;
            Right = right;
        }
        public UOp Operator { get; }
        public Expr Right { get; }

        public override bool Equals(object obj)
        {
            var node = obj as UnaryOperator;
            if (node == null)
                return false;

            if (Operator.Equals(node.Operator) == false)
                return false;

            if (Right.Equals(node.Right) == false)
                return false;

            return true;
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}({Operator})";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class FunctionCall : Expr
    {
        public FunctionCall(Identifier identifier, List<Actual> actuals)
        {
            Identifier = identifier;
            Actuals = actuals.AsReadOnly();
        }
        public Identifier Identifier { get; }
        public ReadOnlyCollection<Actual> Actuals { get; }
        public override bool Equals(object obj)
        {
            var node = obj as FunctionCall;
            if (node == null)
                return false;
            if (Identifier.Equals(node.Identifier) == false)
                return false;
            if (Actuals.Count.Equals(node.Actuals.Count) == false)
                return false;
            for (int i = 0; i < Actuals.Count; i++)
            {
                if (Actuals[i].Equals(node.Actuals[i]) == false)
                    return false;
            }

            return true;
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}({Identifier.Value})";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Actual : Ast
    {
        public Actual(Expr expr)
        {
            Expr = expr;
        }
        public Expr Expr { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Actual;
            if (node == null)
                return false;
            if (Expr.Equals(node.Expr) == false)
                return false;

            return true;
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Identifier : Expr
    {
        public Identifier(string value)
        {
            Value = value;
        }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Identifier;
            if (node == null)
                return false;

            if (this.Value.Equals(node.Value) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Value})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BooleanLiteral : Expr
    {
        public BooleanLiteral(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as BooleanLiteral;
            if (node == null)
                return false;

            if (this.Value.Equals(node.Value) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Value})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            TypeExpr = KType.Boolean;
            return TypeValidationResult.Valid(TypeExpr);
        }
    }

    public class IntegerLiteral : Expr
    {
        public IntegerLiteral(string value)
        {
            Value = uint.Parse(value);
        }

        public uint Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as IntegerLiteral;
            if (node == null)
                return false;

            if (this.Value.Equals(node.Value) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Value})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            TypeExpr = KType.Integer;

            return TypeValidationResult.Valid(TypeExpr);
        }
    }

    #endregion
}
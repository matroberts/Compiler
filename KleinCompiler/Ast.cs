﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace KleinCompiler
{
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
    }

    public abstract class Expr : Ast
    {
    }

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
    }

    public class Definition : Ast
    {
        public Definition(Identifier identifier, KleinType type, List<Formal> formals)
        {
            Identifier = identifier;
            Type = type;
            Formals = formals.AsReadOnly();
        }
        public Identifier Identifier { get; }
        public KleinType Type { get; }
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

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }



    public enum BOp
    {
        Times,
        Plus
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
    }

    public class UnaryOperator : Expr
    {
        public string Operator { get; set; }
        public Expr Right { get; set; }

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
    }

    public class BooleanLiteral : Expr
    {
        public BooleanLiteral(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }

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
    }
}
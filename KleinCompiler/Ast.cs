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

    public class Definition : Ast
    {
        public Definition Next { get; set; }
        public Identifier Identifier { get; set; }
        public KleinType Type { get; set; }

        public override bool Equals(object obj)
        {
            var definition = obj as Definition;
            if (definition == null)
                return false;

            if (Identifier.Equals(definition.Identifier) == false)
                return false;

            if (Type.Equals(definition.Type) == false)
                return false;

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

    public class Program : Definition
    {
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class Expr : Ast
    {
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class BinaryOperator : Expr
    {
        public BinaryOperator(Expr left, string op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
        public Expr Left { get; }
        public string Operator { get; }
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

            if (this.Value != node.Value)
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

    public class KleinType : Ast
    {
        public KleinType(string value)
        {
            Value = value;
        }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as KleinType;
            if (node == null)
                return false;

            if (this.Value != node.Value)
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
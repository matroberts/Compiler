namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class BinaryOperator : Expr
    {
        protected BinaryOperator(Expr left, Expr right)
        {
            Left = left;
            Right = right;
        }
        public Expr Left { get; }
        public Expr Right { get; }

        public override bool Equals(object obj)
        {
            var node = obj as BinaryOperator;
            if (node == null)
                return false;

            if (this.GetType() != node.GetType())
                return false;

            if (Left.Equals(node.Left) == false)
                return false;

            if (Right.Equals(node.Right) == false)
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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class LessThanOperator : BinaryOperator
    {
        public LessThanOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class EqualsOperator : BinaryOperator
    {
        public EqualsOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class OrOperator : BinaryOperator
    {
        public OrOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class PlusOperator : BinaryOperator
    {
        public PlusOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class MinusOperator : BinaryOperator
    {
        public MinusOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class AndOperator : BinaryOperator
    {
        public AndOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class TimesOperator : BinaryOperator
    {
        public TimesOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class DivideOperator : BinaryOperator
    {
        public DivideOperator(Expr left, Expr right) : base(left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }
}
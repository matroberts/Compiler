namespace KleinCompiler.AbstractSyntaxTree
{
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
    public abstract class BinaryOperator : Expr
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

//            if (this.GetType().Equals(node.GetType()) == false)
//                return false;

            if (Operator.Equals(node.Operator) == false)
                return false;

            if (Left.Equals(node.Left) == false)
                return false;

            if (Right.Equals(node.Right) == false)
                return false;

            return true;
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
    /*
     * 
        LessThan,
        Equals,
        Or,
        Plus,
        Minus,
        And,
        Times,
        Divide 
        */

    public class LessThanOperator : BinaryOperator
    {
        public LessThanOperator(Expr left, Expr right) : base(left, BOp.LessThan, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class EqualsOperator : BinaryOperator
    {
        public EqualsOperator(Expr left, Expr right) : base(left, BOp.Equals, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class OrOperator : BinaryOperator
    {
        public OrOperator(Expr left, Expr right) : base(left, BOp.Or, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class PlusOperator : BinaryOperator
    {
        public PlusOperator(Expr left, Expr right) : base(left, BOp.Plus, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class MinusOperator : BinaryOperator
    {
        public MinusOperator(Expr left, Expr right) : base(left, BOp.Minus, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class AndOperator : BinaryOperator
    {
        public AndOperator(Expr left, Expr right) : base(left, BOp.And, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class TimesOperator : BinaryOperator
    {
        public TimesOperator(Expr left, Expr right) : base(left, BOp.Times, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }

    public class DivideOperator : BinaryOperator
    {
        public DivideOperator(Expr left, Expr right) : base(left, BOp.Divide, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
    }
}
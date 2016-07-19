namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class BinaryOperator : Expr
    {
        protected BinaryOperator(int position, Expr left, Expr right) : base(position)
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

        protected TypeValidationResult CheckType(PrimitiveType leftType, PrimitiveType rightType, PrimitiveType returnType, string operatorName)
        {
            var leftResult = Left.CheckType();
            if (leftResult.HasError)
                return leftResult;

            if (leftResult.Type.Equals(leftType) == false)
                return TypeValidationResult.Invalid(Position, $"{operatorName} left expression is not {leftType}");

            var rightResult = Right.CheckType();
            if (rightResult.HasError)
                return rightResult;

            if (rightResult.Type.Equals(rightType) == false)
                return TypeValidationResult.Invalid(Position, $"{operatorName} right expression is not {rightType}");

            Type = returnType;
            return TypeValidationResult.Valid(Type);
        }
    }

    public class LessThanOperator : BinaryOperator
    {
        public LessThanOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new BooleanType(), "LessThan");
        }
    }

    public class EqualsOperator : BinaryOperator
    {
        public EqualsOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new BooleanType(), "Equals");
        }
    }

    public class OrOperator : BinaryOperator
    {
        public OrOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new BooleanType(), new BooleanType(), new BooleanType(), "Or");
        }
    }

    public class AndOperator : BinaryOperator
    {
        public AndOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new BooleanType(), new BooleanType(), new BooleanType(), "And");
        }
    }

    public class PlusOperator : BinaryOperator
    {
        public PlusOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new IntegerType(), "Plus");
        }
    }

    public class MinusOperator : BinaryOperator
    {
        public MinusOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new IntegerType(), "Minus");
        }
    }


    public class TimesOperator : BinaryOperator
    {
        public TimesOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new IntegerType(), "Times");
        }
    }

    public class DivideOperator : BinaryOperator
    {
        public DivideOperator(int position, Expr left, Expr right) : base(position, left, right)
        {
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new IntegerType(), "Divide");
        }
    }
}
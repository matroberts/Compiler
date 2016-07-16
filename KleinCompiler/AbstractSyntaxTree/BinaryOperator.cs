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

        public TypeValidationResult CheckType(PrimitiveType leftType, PrimitiveType rightType, PrimitiveType returnType, string operatorName)
        {
            var leftResult = Left.CheckType();
            if (leftResult.HasError)
                return leftResult;

            if (leftResult.Type.Equals(leftType) == false)
                return TypeValidationResult.Invalid($"{operatorName} left expression is not {leftType}");

            var rightResult = Right.CheckType();
            if (rightResult.HasError)
                return rightResult;

            if (rightResult.Type.Equals(rightType) == false)
                return TypeValidationResult.Invalid($"{operatorName} right expression is not {rightType}");

            Type = returnType;
            return TypeValidationResult.Valid(Type);
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

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new BooleanType(), "LessThan");
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

        public override TypeValidationResult CheckType()
        {
            return CheckType(new IntegerType(), new IntegerType(), new BooleanType(), "Equals");
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

        public override TypeValidationResult CheckType()
        {
            return CheckType(new BooleanType(), new BooleanType(), new BooleanType(), "Or");
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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }
}
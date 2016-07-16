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
            var leftResult = Left.CheckType();
            if (leftResult.HasError)
                return leftResult;

            if (leftResult.Type.Equals(new IntegerType()) == false)
                return TypeValidationResult.Invalid($"LessThan left expression is not an integer");

            var rightResult = Right.CheckType();
            if (rightResult.HasError)
                return rightResult;

            if(rightResult.Type.Equals(new IntegerType()) == false)
                return TypeValidationResult.Invalid($"LessThan right expression is not an integer");

            Type = new BooleanType();
            return TypeValidationResult.Valid(Type);
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
            var leftResult = Left.CheckType();
            if (leftResult.HasError)
                return leftResult;

            if (leftResult.Type.Equals(new IntegerType()) == false)
                return TypeValidationResult.Invalid($"Equals left expression is not an integer");

            var rightResult = Right.CheckType();
            if (rightResult.HasError)
                return rightResult;

            if (rightResult.Type.Equals(new IntegerType()) == false)
                return TypeValidationResult.Invalid($"Equals right expression is not an integer");

            Type = new BooleanType();
            return TypeValidationResult.Valid(Type);
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
            throw new System.NotImplementedException();
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
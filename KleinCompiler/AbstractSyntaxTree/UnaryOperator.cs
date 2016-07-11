namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class UnaryOperator : Expr
    {
        protected UnaryOperator(Expr right)
        {
            Right = right;
        }
        public Expr Right { get; }
    }

    public class NotOperator : UnaryOperator
    {
        public NotOperator(Expr right) : base (right)
        {
            
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override bool Equals(object obj)
        {
            var node = obj as NotOperator;
            if (node == null)
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
            Type = KType.Boolean;

            var result = Right.CheckType();
            if (result.HasError)
                return result;

            if(Type != result.Type)
                return TypeValidationResult.Invalid($"Not operator called with expression which is not boolean");

            return TypeValidationResult.Valid(Type);
        }
    }

    public class NegateOperator : UnaryOperator
    {
        public NegateOperator(Expr right) : base (right)
        {
            
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override bool Equals(object obj)
        {
            var node = obj as NegateOperator;
            if (node == null)
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
            Type = KType.Integer;

            var result = Right.CheckType();
            if (result.HasError)
                return result;

            if (Type != result.Type)
                return TypeValidationResult.Invalid($"Negate operator called with expression which is not integer");

            return TypeValidationResult.Valid(Type);
        }
    }
}
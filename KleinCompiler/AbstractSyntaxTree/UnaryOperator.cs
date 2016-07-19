namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class UnaryOperator : Expr
    {
        protected UnaryOperator(int position, Expr right) : base(position)
        {
            Right = right;
        }

        public Expr Right { get; }

        public override bool Equals(object obj)
        {
            var node = obj as UnaryOperator;
            if (node == null)
                return false;

            if (this.GetType() != node.GetType())
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

    public class NotOperator : UnaryOperator
    {
        public NotOperator(int position, Expr right) : base (position, right)
        {
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            Type = new BooleanType();

            var result = Right.CheckType();
            if (result.HasError)
                return result;

            if(Type.Equals(result.Type) == false)
                return TypeValidationResult.Invalid(Position, $"Not operator called with expression which is not boolean");

            return TypeValidationResult.Valid(Type);
        }
    }

    public class NegateOperator : UnaryOperator
    {
        public NegateOperator(int position, Expr right) : base (position, right)
        {
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            Type = new IntegerType();

            var result = Right.CheckType();
            if (result.HasError)
                return result;

            if (Type.Equals(result.Type) == false)
                return TypeValidationResult.Invalid(Position, $"Negate operator called with expression which is not integer");

            return TypeValidationResult.Valid(Type);
        }
    }
}
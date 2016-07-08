namespace KleinCompiler.AbstractSyntaxTree
{
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
}
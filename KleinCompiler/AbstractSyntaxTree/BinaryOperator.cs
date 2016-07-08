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

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }
}
namespace KleinCompiler.AbstractSyntaxTree
{
    public class Actual : Ast
    {
        public Actual(Expr expr)
        {
            Expr = expr;
            Position = Expr.Position;
        }
        public Expr Expr { get; }
        public int Position { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Actual;
            if (node == null)
                return false;
            if (Expr.Equals(node.Expr) == false)
                return false;

            return true;
        }
        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
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
            var result = Expr.CheckType();
            if (result.HasError)
                return result;

            Type = result.Type;

            return TypeValidationResult.Valid(Type);
        }
    }
}
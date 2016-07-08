namespace KleinCompiler.AbstractSyntaxTree
{
    public class IfThenElse : Expr
    {
        public IfThenElse(Expr ifExpr, Expr thenExpr, Expr elseExpr)
        {
            IfExpr = ifExpr;
            ThenExpr = thenExpr;
            ElseExpr = elseExpr;
        }

        public Expr IfExpr { get; }
        public Expr ThenExpr { get; }
        public Expr ElseExpr { get; }

        public override bool Equals(object obj)
        {
            var node = obj as IfThenElse;
            if (node == null)
                return false;

            if (IfExpr.Equals(node.IfExpr) == false)
                return false;

            if (ThenExpr.Equals(node.ThenExpr) == false)
                return false;

            if (ElseExpr.Equals(node.ElseExpr) == false)
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
            throw new System.NotImplementedException();
        }
    }
}
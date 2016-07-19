namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class Expr : Ast
    {
        protected Expr(int position)
        {
            Position = position;
        }
        public int Position { get; }
    }
}
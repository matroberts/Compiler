namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class Expr : Ast
    {
        protected Expr(int position) : base(position)
        {
        }
    }
}
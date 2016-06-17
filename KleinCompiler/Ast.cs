namespace KleinCompiler
{
    public abstract class Ast
    {
        
    }

    public class Definition : Ast
    {
        public Definition Next { get; private set; }
    }

    public class Program : Definition
    {
        
    }

    public class Expr : Ast
    {
        
    }

    public class BinaryOperator : Expr
    {
        public Expr Left { get; private set; }
        public string Operator { get; private set; }
        public Expr Right { get; private set; }
    }

    public class UnaryOperator : Expr
    {
        public string Operator { get; private set; }
        public Expr Right { get; private set; }
    }

    public class IfThenElse : Expr
    {
        public Expr IfExpr { get; private set; }
        public Expr ThenExpr { get; private set; }
        public Expr ElseExpr { get; private set; }
    }

    public class FunctionCall : Expr
    {
        
    }

    public class Identifier : Expr
    {
        
    }

    public class Literal : Expr
    {
        
    }

}
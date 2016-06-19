namespace KleinCompiler
{
    public abstract class Ast
    {
        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override bool Equals(object obj)
        {
            var token = obj as Ast;
            if (token == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    public class Definition : Ast
    {
        public Definition Next { get; set; }


    }

    public class Program : Definition
    {

    }

    public class Expr : Ast
    {

    }

    public class BinaryOperator : Expr
    {
        public Expr Left { get; set; }
        public string Operator { get; set; }
        public Expr Right { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}({Operator})[{Left}, {Right}]";
        }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class UnaryOperator : Expr
    {
        public string Operator { get; set; }
        public Expr Right { get; set; }


    }

    public class IfThenElse : Expr
    {
        public Expr IfExpr { get; set; }
        public Expr ThenExpr { get; set; }
        public Expr ElseExpr { get; set; }


    }

    public class FunctionCall : Expr
    {
        

    }

    public class Identifier : Expr
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}({Value})";
        }

        public override bool Equals(object obj)
        {
            var node = obj as Identifier;
            if (node == null)
                return false;

            if (this.Value != node.Value)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Literal : Expr
    {
        
    }

}
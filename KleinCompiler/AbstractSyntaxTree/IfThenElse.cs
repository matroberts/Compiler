using System;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class IfThenElse : Expr
    {
        public IfThenElse(int position, Expr ifExpr, Expr thenExpr, Expr elseExpr) : base(position)
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
            var ifResult = IfExpr.CheckType();
            if (ifResult.HasError)
                return ifResult;

            if(ifResult.Type.Equals(new BooleanType())== false)
                return TypeValidationResult.Invalid(Position, "IfThenElseOperator must have a boolean if expression");

            var thenResult = ThenExpr.CheckType();
            if (thenResult.HasError)
                return thenResult;

            var elseResult = ElseExpr.CheckType();
            if (elseResult.HasError)
                return elseResult;

            if(thenResult.Type.Equals(elseResult.Type) == false)
                return TypeValidationResult.Invalid(Position, "IfThenElse, type of then and else expression must be the same");

            Type = thenResult.Type;
            return TypeValidationResult.Valid(Type);
        }
    }
}
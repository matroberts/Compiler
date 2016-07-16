using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class Body : Ast
    {
        public Body(Expr expr) : this (expr, new List<Print>())
        {
        }

        public Body(Expr expr, List<Print> prints)
        {
            Expr = expr;
            Prints = prints.AsReadOnly();
        }

        public Expr Expr { get; }

        public ReadOnlyCollection<Print> Prints { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Body;
            if (node == null)
                return false;

            if (this.Expr.Equals(node.Expr) == false)
                return false;

            if (this.Prints.Count.Equals(node.Prints.Count) == false)
                return false;

            for (int i = 0; i < Prints.Count; i++)
            {
                if (Prints[i].Equals(node.Prints[i]) == false)
                    return false;
            }

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
            foreach (var print in Prints)
            {
                var printResult = print.CheckType();
                if (printResult.HasError)
                    return printResult;
            }

            var result = Expr.CheckType();
            if (result.HasError)
                return result;

            Type = result.Type;

            return TypeValidationResult.Valid(Type);
        }
    }
}
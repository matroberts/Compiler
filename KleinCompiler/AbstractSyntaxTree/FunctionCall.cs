using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class FunctionCall : Expr
    {
        public FunctionCall(Identifier identifier, List<Actual> actuals)
        {
            Identifier = identifier;
            Actuals = actuals.AsReadOnly();
        }
        public Identifier Identifier { get; }
        public ReadOnlyCollection<Actual> Actuals { get; }
        public override bool Equals(object obj)
        {
            var node = obj as FunctionCall;
            if (node == null)
                return false;
            if (Identifier.Equals(node.Identifier) == false)
                return false;
            if (Actuals.Count.Equals(node.Actuals.Count) == false)
                return false;
            for (int i = 0; i < Actuals.Count; i++)
            {
                if (Actuals[i].Equals(node.Actuals[i]) == false)
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
            return $"{GetType().Name}({Identifier.Value})";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            // check identifier exists in symbol table
            // get type of identifier from the symbol table.
            this.Type = SymbolTable.Type(this.Identifier);
            // how can you set the type of the identifier??
            //this.Identifier.Type = this.Type;
            // set type of function and identifier

            // check actuals match formals of function

            return TypeValidationResult.Valid(Type);
        }
    }
}
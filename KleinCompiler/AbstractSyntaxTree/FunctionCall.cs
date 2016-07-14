using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class FunctionCall : Expr
    {
        public FunctionCall(Identifier identifier, List<Actual> actuals)
        {
            Name = identifier.Value;
            Actuals = actuals.AsReadOnly();
        }
        public string Name { get; }
        public ReadOnlyCollection<Actual> Actuals { get; }
        public override bool Equals(object obj)
        {
            var node = obj as FunctionCall;
            if (node == null)
                return false;
            if (Name.Equals(node.Name) == false)
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
            return $"{GetType().Name}({Name})";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            foreach (var actual in Actuals)
            {
                var result = actual.CheckType();
                if (result.HasError)
                    return result;
            }

            if (SymbolTable.Exists(Name) == false)
                return TypeValidationResult.Invalid($"Function '{Name}' has no definition");
            this.Type = SymbolTable.Type(Name);



//            if(SymbolTable.CheckArgs(Name, Actuals.Select(a => a.Type).ToList()) == false)
//                return TypeValidationResult.Invalid("");

            return TypeValidationResult.Valid(Type);
        }
    }
}
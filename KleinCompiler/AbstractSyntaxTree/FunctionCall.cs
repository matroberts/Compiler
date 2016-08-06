using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class FunctionCall : Expr
    {
        public FunctionCall(Identifier identifier, List<Actual> actuals) : base(identifier.Position)
        {
            Name = identifier.Value;
            Actuals = actuals.AsReadOnly();
        }
        public string Name { get; }
        public ReadOnlyCollection<Actual> Actuals { get; }
        private string ActualsTypeString => $"({string.Join(",", Actuals.Select(a => a.Type.ToString()))})";

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
                return TypeValidationResult.Invalid(Position, $"Function '{Name}' has no definition");

            if(SymbolTable.Type(Name).CheckArgs(Actuals.Select(a => a.Type)) == false)
                return TypeValidationResult.Invalid(Position, $"Function {Name}{SymbolTable.Type(Name)} called with mismatched arguments {Name}{ActualsTypeString}");

            this.Type = SymbolTable.Type(Name).ReturnType;
            return TypeValidationResult.Valid(Type);
        }

    }
}
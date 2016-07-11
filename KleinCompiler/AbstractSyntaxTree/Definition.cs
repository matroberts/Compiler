using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class Definition : Ast
    {
        public Definition(Identifier identifier, KleinType kleinType, List<Formal> formals, Body body)
        {
            Name = identifier.Value;
            KleinType = kleinType;
            Body = body;
            Formals = formals.AsReadOnly();
        }
        public string Name { get; }
        public KleinType KleinType { get; }
        public Body Body { get; }
        public ReadOnlyCollection<Formal> Formals { get; }

        public override bool Equals(object obj)
        {
            var definition = obj as Definition;
            if (definition == null)
                return false;

            if (Name.Equals(definition.Name) == false)
                return false;

            if (KleinType.Equals(definition.KleinType) == false)
                return false;

            if (Formals.Count.Equals(definition.Formals.Count) == false)
                return false;

            for (int i = 0; i < Formals.Count; i++)
            {
                if (Formals[i].Equals(definition.Formals[i]) == false)
                    return false;
            }

            // checks for null here are due to the declaration grammar tests, which does not have a body
            if (Body != null && definition.Body != null && Body.Equals(definition.Body) == false)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Name})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override TypeValidationResult CheckType()
        {
            Type = KleinType.Value;
            var result = Body.CheckType();
            if (result.HasError)
                return result;

            if (this.Type != Body.Type)
            {
                return TypeValidationResult.Invalid($"Function '{Name}' has a type '{this.Type}', but its body has a type '{Body.Type}'");
            }
            return TypeValidationResult.Valid(Type);
        }

    }
}
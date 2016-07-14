using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class Definition : Ast
    {
        public Definition(Identifier identifier, TypeDeclaration typeDeclaration, List<Formal> formals, Body body)
        {
            Name = identifier.Value;
            TypeDeclaration = typeDeclaration;
            Body = body;
            Formals = formals.AsReadOnly();
            Type2 = new FunctionType(TypeDeclaration.ToType2(), Formals.Select(f => f.TypeDeclaration.ToType2()));
        }
        public string Name { get; }
        public TypeDeclaration TypeDeclaration { get; }
        public Body Body { get; }
        public ReadOnlyCollection<Formal> Formals { get; }

        public override bool Equals(object obj)
        {
            var definition = obj as Definition;
            if (definition == null)
                return false;

            if (Name.Equals(definition.Name) == false)
                return false;

            if (TypeDeclaration.Equals(definition.TypeDeclaration) == false)
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
            //Type2 set in constructor
            var result = Body.CheckType();
            if (result.HasError)
                return result;

            if (SymbolTable.Type(Name).ReturnType.Equals(Body.Type2) == false)
            {
                return TypeValidationResult.Invalid($"Function '{Name}' has a return type '{SymbolTable.Type(Name).ReturnType}', but its body has a type '{Body.Type2}'");
            }
            return TypeValidationResult.Valid(Type2);
        }

    }
}
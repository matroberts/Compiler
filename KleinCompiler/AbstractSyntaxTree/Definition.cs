using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class Definition : Ast
    {
        public Definition(Identifier identifier, TypeDeclaration typeDeclaration, List<Formal> formals, Body body) : base(identifier.Position)
        {
            Name = identifier.Value;
            TypeDeclaration = typeDeclaration;
            Body = body;
            Formals = formals.AsReadOnly();
            FunctionType = new FunctionType(TypeDeclaration.PrimitiveType, Formals.Select(f => f.PrimitiveType).ToArray());
        }
        public string Name { get; }
        public TypeDeclaration TypeDeclaration { get; }
        public FunctionType FunctionType { get; }
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
            SymbolTable.CurrentFunction = Name;

            var returnTypeResult = TypeDeclaration.CheckType();
            if (returnTypeResult.HasError)
                return returnTypeResult;

            foreach (var formal in Formals)
            {
                var formalResult = formal.CheckType();
                if (formalResult.HasError)
                    return formalResult;
            }

            string duplicateFormalName = Formals.Select(d => d.Name)
                                              .GroupBy(i => i)
                                              .Where(g => g.Count() > 1)
                                              .Select(t => t.Key)
                                              .FirstOrDefault();
            if (duplicateFormalName != null)
                return TypeValidationResult.Invalid(Position, $"Function '{Name}' has duplicate formal '{duplicateFormalName}'");

            var result = Body.CheckType();
            if (result.HasError)
                return result;

            if (FunctionType.ReturnType.Equals(Body.Type) == false)
            {
                return TypeValidationResult.Invalid(Position, $"Function '{Name}' has a return type '{FunctionType.ReturnType}', but its body has a type '{Body.Type}'");
            }

            Type = FunctionType;
            return TypeValidationResult.Valid(Type);
        }

    }
}
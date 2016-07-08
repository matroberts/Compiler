using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class Program : Ast
    {
        public Program(List<Definition> definitions)
        {
            Definitions = definitions.AsReadOnly();
        }

        public Program(params Definition[] definitions)
        {
            Definitions = definitions.ToList().AsReadOnly();
        }

        public ReadOnlyCollection<Definition> Definitions { get; }

        public override bool Equals(object obj)
        {
            var program = obj as Program;
            if (program == null)
                return false;

            if (Definitions.Count.Equals(program.Definitions.Count) == false)
                return false;

            for (int i = 0; i < Definitions.Count; i++)
            {
                if (Definitions[i].Equals(program.Definitions[i]) == false)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
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
            foreach (var definition in Definitions)
            {
                var result = definition.CheckType();
                if (result.HasError)
                    return result;
            }
            // type of program should be type of main
            return TypeValidationResult.Valid(KType.Boolean);
        }
    }
}
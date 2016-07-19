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
            Position = Definitions[0].Position;
        }

        public Program(params Definition[] definitions) : this(definitions.ToList())
        {
        }
        public int Position { get; }
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
            string duplicateFunctionName = Definitions.Select(d => d.Name)
                                                          .GroupBy(i => i)
                                                          .Where(g => g.Count() > 1)
                                                          .Select(t => t.Key)
                                                          .FirstOrDefault();
            if(duplicateFunctionName != null)
                return TypeValidationResult.Invalid($"Program contains duplicate function name '{duplicateFunctionName}'");

            SymbolTable = new SymbolTable(Definitions);

            if(SymbolTable.Exists("main") == false)
                return TypeValidationResult.Invalid("Program must contain a function 'main'");

            Type = SymbolTable.Type("main").ReturnType;

            foreach (var definition in Definitions)
            {
                var result = definition.CheckType();
                if (result.HasError)
                    return result;
            }
            return TypeValidationResult.Valid(Type);
        }
    }
}
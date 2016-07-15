using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class SymbolTable
    {
        private Dictionary<string, FunctionType> identifierTypes = new Dictionary<string, FunctionType>();
        public SymbolTable(ReadOnlyCollection<Definition> definitions)
        {
            foreach (var definition in definitions)
            {
                identifierTypes.Add(definition.Name, definition.FunctionType);
            }
        }

        public FunctionType Type(string identifier) => identifierTypes[identifier];
        public bool Exists(string identifier) => identifierTypes.ContainsKey(identifier);
    }
}
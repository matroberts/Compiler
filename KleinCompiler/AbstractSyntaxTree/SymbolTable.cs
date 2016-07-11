using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class SymbolTable
    {
        private Dictionary<string, KType> identifierTypes = new Dictionary<string, KType>();
        public SymbolTable(ReadOnlyCollection<Definition> definitions)
        {
            foreach (var definition in definitions)
            {
                identifierTypes.Add(definition.Identifier.Value, definition.KleinType.Value);
            }
        }

        public KType Type(string identifier) => identifierTypes[identifier];
        public bool Exists(string identifier) => identifierTypes.ContainsKey(identifier);
    }
}
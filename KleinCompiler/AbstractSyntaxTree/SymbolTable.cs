using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class SymbolTable
    {
        private Dictionary<Identifier, KType> identifierTypes = new Dictionary<Identifier, KType>();
        public SymbolTable(ReadOnlyCollection<Definition> definitions)
        {
            foreach (var definition in definitions)
            {
                identifierTypes.Add(definition.Identifier, definition.KleinType.Value);
            }
        }

        public KType Type(Identifier identifier) => identifierTypes[identifier];
        public bool Exists(Identifier identifier) => identifierTypes.ContainsKey(identifier);
    }
}
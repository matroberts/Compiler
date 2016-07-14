using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace KleinCompiler.AbstractSyntaxTree
{

    public class SymbolTable
    {
        private Dictionary<string, KType> identifierTypes = new Dictionary<string, KType>();
        private Dictionary<string, List<KType>> identifierFormals = new Dictionary<string, List<KType>>();
        public SymbolTable(ReadOnlyCollection<Definition> definitions)
        {
            foreach (var definition in definitions)
            {
                identifierTypes.Add(definition.Name, definition.KleinType.Value);
                identifierFormals.Add(definition.Name, definition.Formals.Select(f => f.KleinType.Value).ToList());
            }
        }

        public KType Type(string identifier) => identifierTypes[identifier];
        public bool Exists(string identifier) => identifierTypes.ContainsKey(identifier);


    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class SymbolTable
    {
        private Dictionary<string, FunctionType> identifierTypes = new Dictionary<string, FunctionType>();
        private Dictionary<string, ReadOnlyCollection<Formal>> identifierFormals = new Dictionary<string, ReadOnlyCollection<Formal>>();
        public SymbolTable(ReadOnlyCollection<Definition> definitions)
        {
            foreach (var definition in definitions)
            {
                identifierTypes.Add(definition.Name, definition.FunctionType);
                identifierFormals.Add(definition.Name, definition.Formals);
            }
        }

        public FunctionType Type(string identifier) => identifierTypes[identifier];
        public bool Exists(string identifier) => identifierTypes.ContainsKey(identifier);

        public string CurrentFunction { get; set; }
        public bool FormalExists(string identifier)
        {
            if(string.IsNullOrWhiteSpace(CurrentFunction))
                throw new Exception("CurrentFunction must be set before you can access FormalExists");

            return identifierFormals[CurrentFunction].Any(f => f.Name.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        }

        public PrimitiveType FormalType(string identifier)
        {
            if (string.IsNullOrWhiteSpace(CurrentFunction))
                throw new Exception("CurrentFunction must be set before you can access FormalType");

            return identifierFormals[CurrentFunction].First(f => f.Name.Equals(identifier, StringComparison.OrdinalIgnoreCase)).ToKType();
        }
    }
}
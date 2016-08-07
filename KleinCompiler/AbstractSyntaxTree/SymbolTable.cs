using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class SymbolTable
    {
        private Dictionary<string, FunctionInfo> functionInfo = new Dictionary<string, FunctionInfo>();
        public SymbolTable(ReadOnlyCollection<Definition> definitions)
        {
            foreach (var definition in definitions)
            {
                functionInfo.Add(definition.Name, new FunctionInfo(definition.FunctionType, definition.Formals));
            }
        }

        public FunctionType FunctionType(string identifier)
        {
            if (functionInfo.ContainsKey(identifier) == false)
                return null;
            return functionInfo[identifier].FunctionType;
        }

        public string CurrentFunction { get; set; }
        public bool FormalExists(string identifier)
        {
            if(string.IsNullOrWhiteSpace(CurrentFunction))
                throw new Exception("CurrentFunction must be set before you can access FormalExists");

            return functionInfo[CurrentFunction].Formals.Any(f => f.Name.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        }

        public PrimitiveType FormalType(string identifier)
        {
            if (string.IsNullOrWhiteSpace(CurrentFunction))
                throw new Exception("CurrentFunction must be set before you can access FormalType");

            return functionInfo[CurrentFunction].Formals.First(f => f.Name.Equals(identifier, StringComparison.OrdinalIgnoreCase)).PrimitiveType;
        }
    }

    public class FunctionInfo
    {
        public FunctionInfo(FunctionType functionType, ReadOnlyCollection<Formal> formals)
        {
            FunctionType = functionType;
            Formals = formals;
        }
        public FunctionType FunctionType { get; }
        public ReadOnlyCollection<Formal> Formals { get; }
        public List<string> Callers { get; } = new List<string>();
    }
}
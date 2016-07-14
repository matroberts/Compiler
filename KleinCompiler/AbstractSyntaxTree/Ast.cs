using System;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class Ast
    {
        public override bool Equals(object obj)
        {
            var token = obj as Ast;
            if (token == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return true;
        }

        public abstract void Accept(IAstVisitor visior);

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public abstract TypeValidationResult CheckType();

        public KType Type { get; protected set; }


        private static SymbolTable symbolTable;

        public static SymbolTable SymbolTable
        {
            get
            {
                if(symbolTable == null)
                    throw new Exception("You must call CheckType() on the AST and it must be valid, before you can access the symbol table");
                return symbolTable;
            }
            protected set { symbolTable = value; }
        }
    }
}
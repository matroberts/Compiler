using System;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class TypeValidationResult
    {
        public static TypeValidationResult Valid(Type2 type)
        {
            return new TypeValidationResult(type);
        }

        public static TypeValidationResult Invalid(string message)
        {
            return new TypeValidationResult(message);
        }

        private TypeValidationResult(string message)
        {
            Message = message;
            Type = null;
        }

        private TypeValidationResult(Type2 type)
        {
            Message = String.Empty;
            Type = type;
        }

        public Type2 Type { get; }
        public bool HasError => Type == null;
        public string Message { get; }
    }
}
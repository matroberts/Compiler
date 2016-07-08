using System;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class TypeValidationResult
    {
        public static TypeValidationResult Valid(KType type)
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

        private TypeValidationResult(KType type)
        {
            Message = String.Empty;
            Type = type;
        }

        public KType? Type { get; }
        public bool HasError => Type == null;
        public string Message { get; }
    }
}
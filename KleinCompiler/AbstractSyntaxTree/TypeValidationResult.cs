using System;

namespace KleinCompiler.AbstractSyntaxTree
{
    public class TypeValidationResult
    {
        public static TypeValidationResult Valid(KType type)
        {
            return new TypeValidationResult(type);
        }

        public static TypeValidationResult Invalid(int position, string message)
        {
            return new TypeValidationResult(position, message);
        }

        private TypeValidationResult(int position, string message)
        {
            Message = message;
            Type = null;
            Position = position;
        }

        private TypeValidationResult(KType type)
        {
            Message = String.Empty;
            Type = type;
        }

        public KType Type { get; }
        public bool HasError => Type == null;
        public string Message { get; }
        public int Position { get; }
    }
}
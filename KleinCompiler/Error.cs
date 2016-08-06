using System;
using System.Diagnostics;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler
{
    public class Error
    {
        public static FilePositionCalculator FilePositionCalculator { get; set; }
        public enum ErrorTypeEnum
        {
            No,
            Lexical,
            Syntax,
            Semantic
        }

        public static Error CreateNoError()
        {
            return new Error(ErrorTypeEnum.No, position: 0, message: "No Error Has Occured", stackTrace: "");
        }
        public static Error CreateLexicalError(ErrorToken token, string stackTrace)
        {
            return new Error(ErrorTypeEnum.Lexical, position: token.Position, message: token.Message, stackTrace: stackTrace);
        }
        public static Error CreateSyntaxError(Symbol symbol, Token token, string stackTrace)
        {
            return new Error(ErrorTypeEnum.Syntax, position: token.Position, message: $"Attempting to parse symbol '{symbol.ToString()}' found token {token.ToString()}", stackTrace: stackTrace);
        }

        public static Error CreateSemanticError(TypeValidationResult result)
        {
            if(result.HasError)
                return new Error(ErrorTypeEnum.Semantic, position: result.Position, message: result.Message, stackTrace: null);
            else
                return CreateNoError();
        }
    

        private Error(ErrorTypeEnum errorType, int position, string message, string stackTrace)
        {
            ErrorType = errorType;
            Position = position;
            Message = message;
            StackTrace = stackTrace;
            if (FilePositionCalculator != null)
                FilePosition = FilePositionCalculator.FilePosition(position);
        }

        public ErrorTypeEnum ErrorType { get; }
        public int Position { get; }
        public FilePosition FilePosition { get; }
        public string Message { get; }
        public string StackTrace { get; }

        public override string ToString()
        {
            string output = $"{ErrorType} Error: {Message}";
            if (string.IsNullOrWhiteSpace(StackTrace) == false)
                output += $"\r\n\r\n{StackTrace}";
            return output;
        }
    }
}
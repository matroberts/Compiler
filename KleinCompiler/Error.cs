using System;
using System.Diagnostics;

namespace KleinCompiler
{
    public class Error
    {
        public enum ErrorTypeEnum
        {
            No,
            Lexical,
            Syntax,
            Exception    
        }

        public static Error CreateNoError()
        {
            return new Error(ErrorTypeEnum.No, token: null, symbol: Symbol.Program, message: "No Error Has Occured", stackTrace: "");
        }
        public static Error CreateLexicalError(ErrorToken token, string stackTrace)
        {
            return new Error(ErrorTypeEnum.Lexical, token: token, symbol: Symbol.Program, message: token.Message, stackTrace: stackTrace);
        }
        public static Error CreateSyntaxError(Symbol symbol, Token token, string stackTrace)
        {
            return new Error(ErrorTypeEnum.Syntax, token: token, symbol: symbol, message: $"Attempting to parse symbol '{symbol.ToString()}' found token {token.ToString()}", stackTrace: stackTrace);
        }
        public static Error CreateExceptionError(Exception exception, string stackTrace)
        {
            return new Error(ErrorTypeEnum.Exception, token: null, symbol: Symbol.Program, message: exception.ToString(), stackTrace: stackTrace);
        }
        private Error(ErrorTypeEnum errorType, Token token, Symbol symbol, string message, string stackTrace)
        {
            ErrorType = errorType;
            Token = token;
            Symbol = symbol;
            Message = message;
            StackTrace = stackTrace;
        }

        public ErrorTypeEnum ErrorType { get; }
        public Token Token { get; }
        public Symbol Symbol { get; }
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
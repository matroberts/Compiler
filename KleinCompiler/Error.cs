using System;

namespace KleinCompiler
{
    public class Error
    {
        public static Error CreateLexicalError(ErrorToken token)
        {
            return new Error() {Message = token.Message, Token = token};
        }

        public static Error CreateSyntaxError(Symbol symbol, Token token)
        {
            return new Error() {Message = $"Syntax Error:  Attempting to parse symbol '{symbol.ToString()}' found token {token.ToString()}", Symbol = symbol, Token = token};
        }

        public static Error CreateExceptionError(Exception exception)
        {
            return new Error() {Message = exception.Message};
        }

        public Token Token { get; private set; }
        public Symbol Symbol { get; private set; }
        public string Message { get; private set; }
        
    
    }
}
namespace KleinCompiler
{
    public enum SymbolName
    {
        // Error
        Error,
        // Tokens / Terminals
        LineComment,
        BlockComment,
        Identifier,
        IntegerLiteral,
        BooleanTrue,
        BooleanFalse,
        IntegerType,
        BooleanType,
        If,
        Then,
        Else,
        Not,
        Or,
        And,
        Plus,            // + 
        Minus,           // - 
        Multiply,        // *
        Divide,          // / 
        LessThan,        // <
        Equality,        // = 
        OpenBracket,     // (
        CloseBracket,    // )
        Comma,           // , 
        Colon,           // :
    }
}
namespace KleinCompiler
{
    public enum SymbolName
    {
        // Non Terminals
        End,             // i.e. $
        Program,          
        DefTail,          
        Def,              
        Formals,          
        NonEmptyFormals,  
        FormalsTail,      
        Formal,           
        Type,             

        // Error
        Error,
        // Tokens / Terminals
        LineComment,
        BlockComment,
        Identifier,
        IntegerLiteral,

        [Keyword("true")]
        BooleanTrue,
        [Keyword("false")]
        BooleanFalse,
        [Keyword("integer")]
        IntegerType,
        [Keyword("boolean")]
        BooleanType,
        [Keyword("if")]
        If,
        [Keyword("then")]
        Then,
        [Keyword("else")]
        Else,
        [Keyword("not")]
        Not,
        [Keyword("or")]
        Or,
        [Keyword("and")]
        And,
        [Keyword("+")]
        Plus,            
        [Keyword("-")]
        Minus,           
        [Keyword("*")]
        Multiply,        
        [Keyword("/")]
        Divide,          
        [Keyword("<")]
        LessThan,        
        [Keyword("=")]
        Equality,        
        [Keyword("(")]
        OpenBracket,     
        [Keyword(")")]
        CloseBracket,    
        [Keyword(",")]
        Comma,           
        [Keyword(":")]
        Colon,              
    }
}
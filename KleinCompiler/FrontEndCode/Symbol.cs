﻿namespace KleinCompiler.FrontEndCode
{
    public enum SymbolType
    {
        Token,
        NonTerminal,
        Semantic
    }
    public enum Symbol
    {
        [SymbolType(SymbolType.NonTerminal)]
        Program,
        [SymbolType(SymbolType.NonTerminal)]
        DefTail,
        [SymbolType(SymbolType.NonTerminal)]
        Def,
        [SymbolType(SymbolType.NonTerminal)]
        Formals,
        [SymbolType(SymbolType.NonTerminal)]
        NonEmptyFormals,
        [SymbolType(SymbolType.NonTerminal)]
        FormalTail,
        [SymbolType(SymbolType.NonTerminal)]
        Formal,
        [SymbolType(SymbolType.NonTerminal)]
        Body,
        [SymbolType(SymbolType.NonTerminal)]
        Type,
        [SymbolType(SymbolType.NonTerminal)]
        Expr,
        [SymbolType(SymbolType.NonTerminal)]
        SimpleExprTail,
        [SymbolType(SymbolType.NonTerminal)]
        SimpleExpr,
        [SymbolType(SymbolType.NonTerminal)]
        TermTail,
        [SymbolType(SymbolType.NonTerminal)]
        Term,
        [SymbolType(SymbolType.NonTerminal)]
        FactorTail,
        [SymbolType(SymbolType.NonTerminal)]
        Factor,
        [SymbolType(SymbolType.NonTerminal)]
        Func,
        [SymbolType(SymbolType.NonTerminal)]
        FuncTail,
        [SymbolType(SymbolType.NonTerminal)]
        Actuals,
        [SymbolType(SymbolType.NonTerminal)]
        NonEmptyActuals,
        [SymbolType(SymbolType.NonTerminal)]
        ActualsTail,
        [SymbolType(SymbolType.NonTerminal)]
        Literal,
        [SymbolType(SymbolType.NonTerminal)]
        Print,
        // Tokens / Terminals
        [SymbolType(SymbolType.Token)]
        LineComment,
        [SymbolType(SymbolType.Token)]
        BlockComment,
        [SymbolType(SymbolType.Token)]
        Identifier,
        [SymbolType(SymbolType.Token)]
        IntegerLiteral,
        [SymbolType(SymbolType.Token)]
        BooleanTrue,
        [SymbolType(SymbolType.Token)]
        BooleanFalse,
        [SymbolType(SymbolType.Token)]
        IntegerType,
        [SymbolType(SymbolType.Token)]
        BooleanType,
        [SymbolType(SymbolType.Token)]
        If,
        [SymbolType(SymbolType.Token)]
        Then,
        [SymbolType(SymbolType.Token)]
        Else,
        [SymbolType(SymbolType.Token)]
        Not,
        [SymbolType(SymbolType.Token)]
        Or,
        [SymbolType(SymbolType.Token)]
        And,
        [SymbolType(SymbolType.Token)]
        Plus,
        [SymbolType(SymbolType.Token)]
        Minus,
        [SymbolType(SymbolType.Token)]
        Times,
        [SymbolType(SymbolType.Token)]
        Divide,
        [SymbolType(SymbolType.Token)]
        LessThan,
        [SymbolType(SymbolType.Token)]
        Equality,
        [SymbolType(SymbolType.Token)]
        OpenBracket,
        [SymbolType(SymbolType.Token)]
        CloseBracket,
        [SymbolType(SymbolType.Token)]
        Comma,
        [SymbolType(SymbolType.Token)]
        Colon,
        [SymbolType(SymbolType.Token)]
        PrintKeyword,
        [SymbolType(SymbolType.Token)]
        LexicalError,
        [SymbolType(SymbolType.Token)]
        End,           // End of tokens marker

        [SymbolType(SymbolType.Semantic)]
        MakeProgram,
        [SymbolType(SymbolType.Semantic)]
        MakeDefinition,
        [SymbolType(SymbolType.Semantic)]
        MakeIntegerTypeDeclaration,
        [SymbolType(SymbolType.Semantic)]
        MakeBooleanTypeDeclaration,
        [SymbolType(SymbolType.Semantic)]
        MakeFormal,
        [SymbolType(SymbolType.Semantic)]
        MakeBody,
        [SymbolType(SymbolType.Semantic)]
        MakePrint,
        // Binary operators
        [SymbolType(SymbolType.Semantic)]
        MakeLessThan,
        [SymbolType(SymbolType.Semantic)]
        MakeEquals,
        [SymbolType(SymbolType.Semantic)]
        MakeOr,
        [SymbolType(SymbolType.Semantic)]
        MakePlus,
        [SymbolType(SymbolType.Semantic)]
        MakeMinus,
        [SymbolType(SymbolType.Semantic)]
        MakeAnd,
        [SymbolType(SymbolType.Semantic)]
        MakeTimes,
        [SymbolType(SymbolType.Semantic)]
        MakeDivide,
        // Unary operators
        [SymbolType(SymbolType.Semantic)]
        MakeNot,
        [SymbolType(SymbolType.Semantic)]
        MakeNegate,
        // Other operators
        [SymbolType(SymbolType.Semantic)]
        MakeIfThenElse,
        [SymbolType(SymbolType.Semantic)]
        MakeFunctionCall,
        [SymbolType(SymbolType.Semantic)]
        MakeActual,
        // Literal and Identifier
        [SymbolType(SymbolType.Semantic)]
        MakeIdentifier,
        [SymbolType(SymbolType.Semantic)]
        MakeIntegerLiteral,
        [SymbolType(SymbolType.Semantic)]
        MakeMakeBooleanTrueLiteral,
        [SymbolType(SymbolType.Semantic)]
        MakeMakeBooleanFalseLiteral,
        // PushPosition is used to capture the file position of non-terminal AST nodes
        [SymbolType(SymbolType.Semantic)]
        PushPosition, 
    }
}
﻿namespace KleinCompiler
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
        Multiply,
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
        MakePlus,
        [SymbolType(SymbolType.Semantic)]
        MakeTimes,
        [SymbolType(SymbolType.Semantic)]
        MakeIdentifier,
        [SymbolType(SymbolType.Semantic)]
        MakeType,
        [SymbolType(SymbolType.Semantic)]
        MakeDefinition,
    }
}
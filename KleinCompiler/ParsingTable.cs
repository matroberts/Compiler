using System;

namespace KleinCompiler
{
    /*
     First(Print)           = print
     First(Number)          = IntegerLiteral
     First(Boolean)         = BooleanTrue BooleanFalse
     First(Literal)         = IntegerLiteral BooleanTrue BooleanFalse
     First(ActualsTail>     = , e
     First(NonEmptyActuals> = First(Expr)
                            = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
     First(Actuals)         = e First(NonEmptyActuals)
     First(FuncTail)        = ( e
     First(Func)            = Identfier
     First(Factor)          = if not First(Func) First(Literal) - (
                            = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
     First(FactorTail)      = and * / e
     First(Term)            = First(Factor)
                            = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
     First(TermTail)        = or + - e
     First(Simple-Expr)     = First(Term)
                            = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
     First(SimpleExprTail)  = < = e
     First(Expr)            = First(Simple-Expr)
                            = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
     First(Type)            = integer boolean
     First(Body)            = First(Print) First(Expr)
                            = print if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
     First(Formal)          = Identifier
     First(FormalTail)      = , e
     First(NonEptyFormals)  = First(Formal)
                            = Identifier
     First(Formals)         = e First(NonEmptyForamls)
                            = e Identifier
     First(Def)             = Identifier
     First(DefTail)         = First(Def) e
                            = Identifier e
     First(Program)         = First(Def)
                            = Identifier


     Follow(Program)        = End
     Follow(DefTail)        = Follow(Program)
                            = End
     Follow(Def)            = First(DefTail - e) Follow(Program) Follow(DefTail)
                            = Identifier End
     Follow(Formals)        = ) 
     Follow(NonEmptyformals)= Follow(Formals)
                            = )
     Follow(FormalTail)     = Follow(NonEmptyFormals)
                            = )
     Follow(Formal)         = First(FormalTail - e) Follow(NonEmptyFormals) Follow(FormalTail)
                            = , )
     Follow(Body)           = 
     Follow(Type)           = First(Body - e) Follow(Formal)
                            = print if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( , )
     Follow(Expr)           = Follow(Body) then else ) Follow(Factor) First(ActualsTail - e) Follow(NonEmptyActuals) Follow(ActualsTail)
                            =              then else ) Follow(Factor) , Follow(NonEmptyActuals) Follow(ActualsTail)
                            =              then else ) and * /  or + - < = , 
     Follow(SimpleExprTail) = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(Simple-Expr)    = First(SimpleExprTail - e) Follow(Expr) Follow(SimpleExprTail)
                            = < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(TermTail)       = Follow(Simple-Expr) 
                            = < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(Term)           = First(TermTail) Follow(Simple-Expr) Follow(TermTail) 
                            = or + - < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(FactorTail)     = Follow(Term) 
                            = or + - < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(Factor)         = First(FactorTail -e) Follow(Term) 
                            = and * /  or + - < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(Func)           = Follow(Factor)
                            = and * /  or + - < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(FuncTail)       = Follow(Func)
                            = and * /  or + - < = Follow(Expr)
                            = then else ) and * /  or + - < = , 
     Follow(Actuals)        = )
     Follow(NonEmptyActuals)= Follow(Actuals)
                            = )
     Follow(ActualsTail)    = Follow(NonEmptyActuals) 
                            = )
     Follow(Literal)        = Follow(Factor)
                            = then else ) and * /  or + - < = , 
     Follow(Print)          = First(Body - e) 
                            = print if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
  */
    public class ParsingTable : IParsingTable
    {
        private readonly Rule[,] table;
        public ParsingTable()
        {
            int numberSymbols = Enum.GetNames(typeof(Symbol)).Length;
            table = new Rule[numberSymbols, numberSymbols];

            // M[Program, Identifier]         = R1
            MakeEntry(Rule.R1, Symbol.Program, Symbol.Identifier);
            // M[DefTail, Identifier]         = R2
            MakeEntry(Rule.R2, Symbol.DefTail, Symbol.Identifier);
            // M[DefTail, End]                = R3
            MakeEntry(Rule.R3, Symbol.DefTail, Symbol.End);
            // M[Def, Identifier]             = R4
            MakeEntry(Rule.R4, Symbol.Def, Symbol.Identifier);
            // M[Formals, )]                  = R5
            MakeEntry(Rule.R5, Symbol.Formals, Symbol.CloseBracket);
            // M[Formals, Identifier]         = R6
            MakeEntry(Rule.R6, Symbol.Formals, Symbol.Identifier);
            // M[NonEmptyFormals, Identifier] = R7
            MakeEntry(Rule.R7, Symbol.NonEmptyFormals, Symbol.Identifier);
            // M[FormalTail, ","]             = R8
            MakeEntry(Rule.R8, Symbol.FormalTail, Symbol.Comma);
            // M[FormalTail, )]               = R9
            MakeEntry(Rule.R9, Symbol.FormalTail, Symbol.CloseBracket);
            // M[Formal, Identifier]          = R10
            MakeEntry(Rule.R10, Symbol.Formal, Symbol.Identifier);
            // M[Body, print]                 = R11
            MakeEntry(Rule.R11, Symbol.Body, Symbol.PrintKeyword);
            // M[Body, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R12
            MakeEntry(Rule.R12, Symbol.Body, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            // M[Type, integer]               = R13
            MakeEntry(Rule.R13, Symbol.Type, Symbol.IntegerType);
            // M[Type, boolean]               = R14
            MakeEntry(Rule.R14, Symbol.Type, Symbol.BooleanType);
            // M[Expr, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R15
            MakeEntry(Rule.R15, Symbol.Expr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            // M[SimpleExprTail, <]           = R16
            MakeEntry(Rule.R16, Symbol.SimpleExprTail, Symbol.LessThan);
            // M[SimpleExprTail, =]           = R17
            MakeEntry(Rule.R17, Symbol.SimpleExprTail, Symbol.Equality);
            // M[SimpleExprTail, then else ) and * /  or + - < = , ] = R18
            MakeEntry(Rule.R18, Symbol.SimpleExprTail, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            // M[Simple-Expr, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R19
            MakeEntry(Rule.R19, Symbol.SimpleExpr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            // M[TermTail, or]                = R20
            MakeEntry(Rule.R20, Symbol.TermTail, Symbol.Or);
            // M[TermTail, +]                 = R21
            MakeEntry(Rule.R21, Symbol.TermTail, Symbol.Equality);
            // M[TermTail, -]                 = R22
            MakeEntry(Rule.R22, Symbol.TermTail, Symbol.Minus);
            // M[TermTail, then else ) and * /  or + - < = , ] = R23
            MakeEntry(Rule.R23, Symbol.TermTail, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            // M[Term, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R24
            MakeEntry(Rule.R24, Symbol.Term, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            // M[FactorTail, and ]            = R25
            MakeEntry(Rule.R25, Symbol.FactorTail, Symbol.And);
            // M[FactorTail, *]               = R26
            MakeEntry(Rule.R26, Symbol.FactorTail, Symbol.Multiply);
            // M[FactorTail, /]               = R27
            MakeEntry(Rule.R27, Symbol.FactorTail, Symbol.Divide);
            // M[FactorTail, then else ) and * /  or + - < = ,] = R28
            MakeEntry(Rule.R28, Symbol.FactorTail, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            // M[Factor, if]                  = R29
            MakeEntry(Rule.R29, Symbol.Factor, Symbol.If);
            // M[Factor, not]                 = R30
            MakeEntry(Rule.R30, Symbol.Factor, Symbol.Not);
            // M[Factor, Identifier]          = R31
            MakeEntry(Rule.R31, Symbol.Factor, Symbol.Identifier);
            // M[Factor, IntegerLiteral BooleanTrue BooleanFalse] = R32
            MakeEntry(Rule.R32, Symbol.Factor, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse);
            // M[Factor, -]                   = R33
            MakeEntry(Rule.R33, Symbol.Factor, Symbol.Minus);
            // M[Factor, (]                   = R34
            MakeEntry(Rule.R34, Symbol.Factor, Symbol.OpenBracket);
            // M[Func, Identifier]            = R35
            MakeEntry(Rule.R35, Symbol.Func, Symbol.Identifier);
            // M[FuncTail, (]                 = R36
            MakeEntry(Rule.R36, Symbol.FuncTail, Symbol.OpenBracket);
            // M[FuncTail, then else ) and * /  or + - < = , ] = R37
            MakeEntry(Rule.R37, Symbol.FuncTail, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            // M[Actuals, ) ]                 = R38
            MakeEntry(Rule.R38, Symbol.Actuals, Symbol.CloseBracket);
            // N[Actuals, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R39
            MakeEntry(Rule.R39, Symbol.Actuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            // M[NonEmptyActuals, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R40
            MakeEntry(Rule.R40, Symbol.NonEmptyActuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            // M[ActualsTail, ","]            = R41
            MakeEntry(Rule.R41, Symbol.ActualsTail, Symbol.Comma);
            // M[ActualsTail, ) ]             = R42
            MakeEntry(Rule.R42, Symbol.ActualsTail, Symbol.CloseBracket);
            // M[Literal, IntegerLiteral]     = R43
            MakeEntry(Rule.R43, Symbol.Literal, Symbol.IntegerLiteral);
            // M[Literal, BooleanTrue]        = R44
            MakeEntry(Rule.R44, Symbol.Literal, Symbol.BooleanTrue);
            // M[Literal,  BooleanFalse]      = R45
            MakeEntry(Rule.R45, Symbol.Literal, Symbol.BooleanFalse);
            // M[Print, print]                = R46 
            MakeEntry(Rule.R46, Symbol.Print, Symbol.PrintKeyword);
        }

        private void MakeEntry(Rule rule, Symbol nonTerminal, params Symbol[] terminals)
        {
            foreach (var terminal in terminals)
            {
                table[(int) nonTerminal, (int) terminal] = rule;
            }
        }

        public Rule this[Symbol symbol, Symbol token] => table[(int)symbol, (int)token];
    }
}
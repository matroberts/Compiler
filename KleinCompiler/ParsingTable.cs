namespace KleinCompiler
{
    /*
 M[Program, Identifier]         = R1
 M[DefTail, Identifier]         = R2
 M[DefTail, End]                = R3
 M[Def, Identifier]             = R4
 M[Formals, )]                  = R5
 M[Formals, Identifier]         = R6
 M[NonEmptyFormals, Identifier] = R7
 M[FormalTail, ","]             = R8
 M[FormalTail, )]               = R9
 M[Formal, Identifier]          = R10
 M[Body, print]                 = R11
 M[Body, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R12
 M[Type, integer]               = R13
 M[Type, boolean]               = R14
 M[Expr, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R15
 M[SimpleExprTail, <]           = R16
 M[SimpleExprTail, =]           = R17
 M[SimpleExprTail, then else ) and * /  or + - < = , ] = R18
 M[Simple-Expr, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R19
 M[TermTail, or]                = R20
 M[TermTail, +]                 = R21
 M[TermTail, -]                 = R22
 M[TermTail, then else ) and * /  or + - < = , ] = R23
 M[Term, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R24
 M[FactorTail, and ]            = R25
 M[FactorTail, *]               = R26
 M[FactorTail, /]               = R27
 M[FactorTail, then else ) and * /  or + - < = ,] = R28
 M[Factor, if]                  = R29
 M[Factor, not]                 = R30
 M[Factor, Identifier]          = R31
 M[Factor, IntegerLiteral BooleanTrue BooleanFalse] = R32
 M[Factor, -]                   = R33
 M[Factor, (]                   = R34
 M[Func, Identifier]            = R35
 M[FuncTail, (]                 = R36
 M[FuncTail, then else ) and * /  or + - < = , ] = R37
 M[Actuals, ) ]                 = R38
 N[NonEmptyActuals, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R39
 M[NonEmptyActuals, if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( ] = R40
 M[ActualsTail, ","]            = R41
 M[ActualsTail, ) ]             = R42
 M[Literal, IntegerLiteral]     = R43
 M[Literal, BooleanTrue]        = R44
 M[Literal,  BooleanFalse]      = R45
 M[Print, print]                = R46 


 */
    public class ParsingTable
    {

    }
}
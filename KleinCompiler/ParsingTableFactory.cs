namespace KleinCompiler
{
    public class ParsingTableFactory
    {
        public static ParsingTable Create()
        {
            var parsingTable = new ParsingTable();

            parsingTable.AddRule(Rule.R1, Symbol.Program, Symbol.Identifier);
            parsingTable.AddRule(Rule.R2, Symbol.DefTail, Symbol.Identifier);
            parsingTable.AddRule(Rule.R3, Symbol.DefTail, Symbol.End);
            parsingTable.AddRule(Rule.R4, Symbol.Def, Symbol.Identifier);
            parsingTable.AddRule(Rule.R5, Symbol.Formals, Symbol.CloseBracket);
            parsingTable.AddRule(Rule.R6, Symbol.Formals, Symbol.Identifier);
            parsingTable.AddRule(Rule.R7, Symbol.NonEmptyFormals, Symbol.Identifier);
            parsingTable.AddRule(Rule.R8, Symbol.FormalTail, Symbol.Comma);
            parsingTable.AddRule(Rule.R9, Symbol.FormalTail, Symbol.CloseBracket);
            parsingTable.AddRule(Rule.R10, Symbol.Formal, Symbol.Identifier);
            parsingTable.AddRule(Rule.R11, Symbol.Body, Symbol.PrintKeyword);
            parsingTable.AddRule(Rule.R12, Symbol.Body, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R13, Symbol.Type, Symbol.IntegerType);
            parsingTable.AddRule(Rule.R14, Symbol.Type, Symbol.BooleanType);
            parsingTable.AddRule(Rule.R15, Symbol.Expr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R16, Symbol.SimpleExprTail, Symbol.LessThan);
            parsingTable.AddRule(Rule.R17, Symbol.SimpleExprTail, Symbol.Equality);
            parsingTable.AddRule(Rule.R18, Symbol.SimpleExprTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.Comma);
            parsingTable.AddRule(Rule.R19, Symbol.SimpleExpr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R20, Symbol.TermTail, Symbol.Or);
            parsingTable.AddRule(Rule.R21, Symbol.TermTail, Symbol.Plus);
            parsingTable.AddRule(Rule.R22, Symbol.TermTail, Symbol.Minus);
            parsingTable.AddRule(Rule.R23, Symbol.TermTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(Rule.R24, Symbol.Term, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R25, Symbol.FactorTail, Symbol.And);
            parsingTable.AddRule(Rule.R26, Symbol.FactorTail, Symbol.Multiply);
            parsingTable.AddRule(Rule.R27, Symbol.FactorTail, Symbol.Divide);
            parsingTable.AddRule(Rule.R28, Symbol.FactorTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(Rule.R29, Symbol.Factor, Symbol.If);
            parsingTable.AddRule(Rule.R30, Symbol.Factor, Symbol.Not);
            parsingTable.AddRule(Rule.R31, Symbol.Factor, Symbol.Identifier);
            parsingTable.AddRule(Rule.R32, Symbol.Factor, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse);
            parsingTable.AddRule(Rule.R33, Symbol.Factor, Symbol.Minus);
            parsingTable.AddRule(Rule.R34, Symbol.Factor, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R35, Symbol.Func, Symbol.Identifier);
            parsingTable.AddRule(Rule.R36, Symbol.FuncTail, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R37, Symbol.FuncTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(Rule.R38, Symbol.Actuals, Symbol.CloseBracket);
            parsingTable.AddRule(Rule.R39, Symbol.Actuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R40, Symbol.NonEmptyActuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(Rule.R41, Symbol.ActualsTail, Symbol.Comma);
            parsingTable.AddRule(Rule.R42, Symbol.ActualsTail, Symbol.CloseBracket);
            parsingTable.AddRule(Rule.R43, Symbol.Literal, Symbol.IntegerLiteral);
            parsingTable.AddRule(Rule.R44, Symbol.Literal, Symbol.BooleanTrue);
            parsingTable.AddRule(Rule.R45, Symbol.Literal, Symbol.BooleanFalse);
            parsingTable.AddRule(Rule.R46, Symbol.Print, Symbol.PrintKeyword);

            return parsingTable;
        }
    }
}
namespace KleinCompiler
{
    public class ParsingTableFactory
    {
        public static ParsingTable Create()
        {
            var parsingTable = new ParsingTable();

            parsingTable.AddRule(RuleFactory.R1, Symbol.Program, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R2, Symbol.DefTail, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R3, Symbol.DefTail, Symbol.End);
            parsingTable.AddRule(RuleFactory.R4, Symbol.Def, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R5, Symbol.Formals, Symbol.CloseBracket);
            parsingTable.AddRule(RuleFactory.R6, Symbol.Formals, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R7, Symbol.NonEmptyFormals, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R8, Symbol.FormalTail, Symbol.Comma);
            parsingTable.AddRule(RuleFactory.R9, Symbol.FormalTail, Symbol.CloseBracket);
            parsingTable.AddRule(RuleFactory.R10, Symbol.Formal, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R11, Symbol.Body, Symbol.PrintKeyword);
            parsingTable.AddRule(RuleFactory.R12, Symbol.Body, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R13, Symbol.Type, Symbol.IntegerType);
            parsingTable.AddRule(RuleFactory.R14, Symbol.Type, Symbol.BooleanType);
            parsingTable.AddRule(RuleFactory.R15, Symbol.Expr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R16, Symbol.SimpleExprTail, Symbol.LessThan);
            parsingTable.AddRule(RuleFactory.R17, Symbol.SimpleExprTail, Symbol.Equality);
            parsingTable.AddRule(RuleFactory.R18, Symbol.SimpleExprTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.Comma);
            parsingTable.AddRule(RuleFactory.R19, Symbol.SimpleExpr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R20, Symbol.TermTail, Symbol.Or);
            parsingTable.AddRule(RuleFactory.R21, Symbol.TermTail, Symbol.Plus);
            parsingTable.AddRule(RuleFactory.R22, Symbol.TermTail, Symbol.Minus);
            parsingTable.AddRule(RuleFactory.R23, Symbol.TermTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(RuleFactory.R24, Symbol.Term, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R25, Symbol.FactorTail, Symbol.And);
            parsingTable.AddRule(RuleFactory.R26, Symbol.FactorTail, Symbol.Multiply);
            parsingTable.AddRule(RuleFactory.R27, Symbol.FactorTail, Symbol.Divide);
            parsingTable.AddRule(RuleFactory.R28, Symbol.FactorTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(RuleFactory.R29, Symbol.Factor, Symbol.If);
            parsingTable.AddRule(RuleFactory.R30, Symbol.Factor, Symbol.Not);
            parsingTable.AddRule(RuleFactory.R31, Symbol.Factor, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R32, Symbol.Factor, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse);
            parsingTable.AddRule(RuleFactory.R33, Symbol.Factor, Symbol.Minus);
            parsingTable.AddRule(RuleFactory.R34, Symbol.Factor, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R35, Symbol.Func, Symbol.Identifier);
            parsingTable.AddRule(RuleFactory.R36, Symbol.FuncTail, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R37, Symbol.FuncTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(RuleFactory.R38, Symbol.Actuals, Symbol.CloseBracket);
            parsingTable.AddRule(RuleFactory.R39, Symbol.Actuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R40, Symbol.NonEmptyActuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(RuleFactory.R41, Symbol.ActualsTail, Symbol.Comma);
            parsingTable.AddRule(RuleFactory.R42, Symbol.ActualsTail, Symbol.CloseBracket);
            parsingTable.AddRule(RuleFactory.R43, Symbol.Literal, Symbol.IntegerLiteral);
            parsingTable.AddRule(RuleFactory.R44, Symbol.Literal, Symbol.BooleanTrue);
            parsingTable.AddRule(RuleFactory.R45, Symbol.Literal, Symbol.BooleanFalse);
            parsingTable.AddRule(RuleFactory.R46, Symbol.Print, Symbol.PrintKeyword);

            return parsingTable;
        }
    }
}
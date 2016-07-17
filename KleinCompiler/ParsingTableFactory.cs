namespace KleinCompiler
{
    public class ParsingTableFactory
    {
        private static Rule R1 => new Rule("R1 ", Symbol.Def, Symbol.DefTail, Symbol.MakeProgram);
        private static Rule R2 => new Rule("R2 ", Symbol.Def, Symbol.DefTail);
        private static Rule R3 => new Rule("R3 ");
        private static Rule R4 => new Rule("R4 ", Symbol.Identifier, Symbol.MakeIdentifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type, Symbol.Body, Symbol.MakeDefinition);
        private static Rule R5 => new Rule("R5 ");
        private static Rule R6 => new Rule("R6 ", Symbol.NonEmptyFormals);
        private static Rule R7 => new Rule("R7 ", Symbol.Formal, Symbol.FormalTail);
        private static Rule R8 => new Rule("R8 ", Symbol.Comma, Symbol.Formal, Symbol.FormalTail);
        private static Rule R9 => new Rule("R9 ");
        private static Rule R10 => new Rule("R10", Symbol.Identifier, Symbol.MakeIdentifier, Symbol.Colon, Symbol.Type, Symbol.MakeFormal);
        private static Rule R11 => new Rule("R11", Symbol.Print, Symbol.Body);
        private static Rule R12 => new Rule("R12", Symbol.Expr, Symbol.MakeBody);
        private static Rule R13 => new Rule("R13", Symbol.IntegerType, Symbol.MakeIntegerTypeDeclaration);
        private static Rule R14 => new Rule("R14", Symbol.BooleanType, Symbol.MakeBooleanTypeDeclaration);
        private static Rule R15 => new Rule("R15", Symbol.SimpleExpr, Symbol.SimpleExprTail);
        private static Rule R16 => new Rule("R16", Symbol.LessThan, Symbol.PushPosition, Symbol.SimpleExpr, Symbol.MakeLessThan, Symbol.SimpleExprTail);
        private static Rule R17 => new Rule("R17", Symbol.Equality, Symbol.PushPosition, Symbol.SimpleExpr, Symbol.MakeEquals, Symbol.SimpleExprTail);
        private static Rule R18 => new Rule("R18");
        private static Rule R19 => new Rule("R19", Symbol.Term, Symbol.TermTail);
        private static Rule R20 => new Rule("R20", Symbol.Or, Symbol.PushPosition, Symbol.Term, Symbol.MakeOr, Symbol.TermTail);
        private static Rule R21 => new Rule("R21", Symbol.Plus, Symbol.PushPosition, Symbol.Term, Symbol.MakePlus, Symbol.TermTail);
        private static Rule R22 => new Rule("R22", Symbol.Minus, Symbol.PushPosition, Symbol.Term, Symbol.MakeMinus, Symbol.TermTail);
        private static Rule R23 => new Rule("R23");
        private static Rule R24 => new Rule("R24", Symbol.Factor, Symbol.FactorTail);
        private static Rule R25 => new Rule("R25", Symbol.And, Symbol.PushPosition, Symbol.Factor, Symbol.MakeAnd, Symbol.FactorTail);
        private static Rule R26 => new Rule("R26", Symbol.Times, Symbol.PushPosition, Symbol.Factor, Symbol.MakeTimes, Symbol.FactorTail);
        private static Rule R27 => new Rule("R27", Symbol.Divide, Symbol.PushPosition, Symbol.Factor, Symbol.MakeDivide, Symbol.FactorTail);
        private static Rule R28 => new Rule("R28");
        private static Rule R29 => new Rule("R29", Symbol.If, Symbol.Expr, Symbol.Then, Symbol.Expr, Symbol.Else, Symbol.Expr, Symbol.MakeIfThenElse);
        private static Rule R30 => new Rule("R30", Symbol.Not, Symbol.Factor, Symbol.MakeNot);
        private static Rule R31 => new Rule("R31", Symbol.Func);
        private static Rule R32 => new Rule("R32", Symbol.Literal);
        private static Rule R33 => new Rule("R33", Symbol.Minus, Symbol.Factor, Symbol.MakeNegate);
        private static Rule R34 => new Rule("R34", Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);
        private static Rule R35 => new Rule("R35", Symbol.Identifier, Symbol.MakeIdentifier, Symbol.FuncTail);
        private static Rule R36 => new Rule("R36", Symbol.OpenBracket, Symbol.Actuals, Symbol.CloseBracket, Symbol.MakeFunctionCall);
        private static Rule R37 => new Rule("R37");
        private static Rule R38 => new Rule("R38");
        private static Rule R39 => new Rule("R39", Symbol.NonEmptyActuals);
        private static Rule R40 => new Rule("R40", Symbol.Expr, Symbol.MakeActual, Symbol.ActualsTail);
        private static Rule R41 => new Rule("R41", Symbol.Comma, Symbol.Expr, Symbol.MakeActual, Symbol.ActualsTail);
        private static Rule R42 => new Rule("R42");
        private static Rule R43 => new Rule("R43", Symbol.IntegerLiteral, Symbol.MakeIntegerLiteral);
        private static Rule R44 => new Rule("R44", Symbol.BooleanTrue, Symbol.MakeMakeBooleanTrueLiteral);
        private static Rule R45 => new Rule("R45", Symbol.BooleanFalse, Symbol.MakeMakeBooleanFalseLiteral);
        private static Rule R46 => new Rule("R46", Symbol.PrintKeyword, Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket, Symbol.MakePrint);

        public static ParsingTable Create()
        {
            var parsingTable = new ParsingTable(Symbol.Program, Symbol.End);

            parsingTable.AddRule(R1, Symbol.Program, Symbol.Identifier);
            parsingTable.AddRule(R2, Symbol.DefTail, Symbol.Identifier);
            parsingTable.AddRule(R3, Symbol.DefTail, Symbol.End);
            parsingTable.AddRule(R4, Symbol.Def, Symbol.Identifier);
            parsingTable.AddRule(R5, Symbol.Formals, Symbol.CloseBracket);
            parsingTable.AddRule(R6, Symbol.Formals, Symbol.Identifier);
            parsingTable.AddRule(R7, Symbol.NonEmptyFormals, Symbol.Identifier);
            parsingTable.AddRule(R8, Symbol.FormalTail, Symbol.Comma);
            parsingTable.AddRule(R9, Symbol.FormalTail, Symbol.CloseBracket);
            parsingTable.AddRule(R10, Symbol.Formal, Symbol.Identifier);
            parsingTable.AddRule(R11, Symbol.Body, Symbol.PrintKeyword);
            parsingTable.AddRule(R12, Symbol.Body, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(R13, Symbol.Type, Symbol.IntegerType);
            parsingTable.AddRule(R14, Symbol.Type, Symbol.BooleanType);
            parsingTable.AddRule(R15, Symbol.Expr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(R16, Symbol.SimpleExprTail, Symbol.LessThan);
            parsingTable.AddRule(R17, Symbol.SimpleExprTail, Symbol.Equality);
            parsingTable.AddRule(R18, Symbol.SimpleExprTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Times, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.Comma);
            parsingTable.AddRule(R19, Symbol.SimpleExpr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(R20, Symbol.TermTail, Symbol.Or);
            parsingTable.AddRule(R21, Symbol.TermTail, Symbol.Plus);
            parsingTable.AddRule(R22, Symbol.TermTail, Symbol.Minus);
            parsingTable.AddRule(R23, Symbol.TermTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Times, Symbol.Divide, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(R24, Symbol.Term, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(R25, Symbol.FactorTail, Symbol.And);
            parsingTable.AddRule(R26, Symbol.FactorTail, Symbol.Times);
            parsingTable.AddRule(R27, Symbol.FactorTail, Symbol.Divide);
            parsingTable.AddRule(R28, Symbol.FactorTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(R29, Symbol.Factor, Symbol.If);
            parsingTable.AddRule(R30, Symbol.Factor, Symbol.Not);
            parsingTable.AddRule(R31, Symbol.Factor, Symbol.Identifier);
            parsingTable.AddRule(R32, Symbol.Factor, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse);
            parsingTable.AddRule(R33, Symbol.Factor, Symbol.Minus);
            parsingTable.AddRule(R34, Symbol.Factor, Symbol.OpenBracket);
            parsingTable.AddRule(R35, Symbol.Func, Symbol.Identifier);
            parsingTable.AddRule(R36, Symbol.FuncTail, Symbol.OpenBracket);
            parsingTable.AddRule(R37, Symbol.FuncTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Times, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            parsingTable.AddRule(R38, Symbol.Actuals, Symbol.CloseBracket);
            parsingTable.AddRule(R39, Symbol.Actuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(R40, Symbol.NonEmptyActuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            parsingTable.AddRule(R41, Symbol.ActualsTail, Symbol.Comma);
            parsingTable.AddRule(R42, Symbol.ActualsTail, Symbol.CloseBracket);
            parsingTable.AddRule(R43, Symbol.Literal, Symbol.IntegerLiteral);
            parsingTable.AddRule(R44, Symbol.Literal, Symbol.BooleanTrue);
            parsingTable.AddRule(R45, Symbol.Literal, Symbol.BooleanFalse);
            parsingTable.AddRule(R46, Symbol.Print, Symbol.PrintKeyword);

            return parsingTable;
        }
    }
}
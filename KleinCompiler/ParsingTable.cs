using System;

namespace KleinCompiler
{
    public interface IParsingTable
    {
        Rule this[Symbol symbol, Symbol token] { get; }
    }

    public class ParsingTable : IParsingTable
    {
        private readonly Rule[,] table;
        public ParsingTable()
        {
            int numberSymbols = Enum.GetNames(typeof(Symbol)).Length;
            table = new Rule[numberSymbols, numberSymbols];

            MakeEntry(Rule.R1, Symbol.Program, Symbol.Identifier);
            MakeEntry(Rule.R2, Symbol.DefTail, Symbol.Identifier);
            MakeEntry(Rule.R3, Symbol.DefTail, Symbol.End);
            MakeEntry(Rule.R4, Symbol.Def, Symbol.Identifier);
            MakeEntry(Rule.R5, Symbol.Formals, Symbol.CloseBracket);
            MakeEntry(Rule.R6, Symbol.Formals, Symbol.Identifier);
            MakeEntry(Rule.R7, Symbol.NonEmptyFormals, Symbol.Identifier);
            MakeEntry(Rule.R8, Symbol.FormalTail, Symbol.Comma);
            MakeEntry(Rule.R9, Symbol.FormalTail, Symbol.CloseBracket);
            MakeEntry(Rule.R10, Symbol.Formal, Symbol.Identifier);
            MakeEntry(Rule.R11, Symbol.Body, Symbol.PrintKeyword);
            MakeEntry(Rule.R12, Symbol.Body, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            MakeEntry(Rule.R13, Symbol.Type, Symbol.IntegerType);
            MakeEntry(Rule.R14, Symbol.Type, Symbol.BooleanType);
            MakeEntry(Rule.R15, Symbol.Expr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            MakeEntry(Rule.R16, Symbol.SimpleExprTail, Symbol.LessThan);
            MakeEntry(Rule.R17, Symbol.SimpleExprTail, Symbol.Equality);
            MakeEntry(Rule.R18, Symbol.SimpleExprTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.Comma);
            MakeEntry(Rule.R19, Symbol.SimpleExpr, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            MakeEntry(Rule.R20, Symbol.TermTail, Symbol.Or);
            MakeEntry(Rule.R21, Symbol.TermTail, Symbol.Plus);
            MakeEntry(Rule.R22, Symbol.TermTail, Symbol.Minus);
            MakeEntry(Rule.R23, Symbol.TermTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            MakeEntry(Rule.R24, Symbol.Term, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            MakeEntry(Rule.R25, Symbol.FactorTail, Symbol.And);
            MakeEntry(Rule.R26, Symbol.FactorTail, Symbol.Multiply);
            MakeEntry(Rule.R27, Symbol.FactorTail, Symbol.Divide);
            MakeEntry(Rule.R28, Symbol.FactorTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            MakeEntry(Rule.R29, Symbol.Factor, Symbol.If);
            MakeEntry(Rule.R30, Symbol.Factor, Symbol.Not);
            MakeEntry(Rule.R31, Symbol.Factor, Symbol.Identifier);
            MakeEntry(Rule.R32, Symbol.Factor, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse);
            MakeEntry(Rule.R33, Symbol.Factor, Symbol.Minus);
            MakeEntry(Rule.R34, Symbol.Factor, Symbol.OpenBracket);
            MakeEntry(Rule.R35, Symbol.Func, Symbol.Identifier);
            MakeEntry(Rule.R36, Symbol.FuncTail, Symbol.OpenBracket);
            MakeEntry(Rule.R37, Symbol.FuncTail, Symbol.Identifier, Symbol.End, Symbol.Then, Symbol.Else, Symbol.CloseBracket, Symbol.And, Symbol.Multiply, Symbol.Divide, Symbol.Or, Symbol.Plus, Symbol.Minus, Symbol.LessThan, Symbol.Equality, Symbol.Comma);
            MakeEntry(Rule.R38, Symbol.Actuals, Symbol.CloseBracket);
            MakeEntry(Rule.R39, Symbol.Actuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            MakeEntry(Rule.R40, Symbol.NonEmptyActuals, Symbol.If, Symbol.Not, Symbol.Identifier, Symbol.IntegerLiteral, Symbol.BooleanTrue, Symbol.BooleanFalse, Symbol.Minus, Symbol.OpenBracket);
            MakeEntry(Rule.R41, Symbol.ActualsTail, Symbol.Comma);
            MakeEntry(Rule.R42, Symbol.ActualsTail, Symbol.CloseBracket);
            MakeEntry(Rule.R43, Symbol.Literal, Symbol.IntegerLiteral);
            MakeEntry(Rule.R44, Symbol.Literal, Symbol.BooleanTrue);
            MakeEntry(Rule.R45, Symbol.Literal, Symbol.BooleanFalse);
            MakeEntry(Rule.R46, Symbol.Print, Symbol.PrintKeyword);
        }

        private void MakeEntry(Rule rule, Symbol nonTerminal, params Symbol[] terminals)
        {
            foreach (var terminal in terminals)
            {
                var existingRule = table[(int) nonTerminal, (int) terminal];
                if (existingRule != null)
                    throw new ArgumentException($"Ambiguity detected in parser table [{nonTerminal}, {terminal}] rule '{existingRule.Name}' conflicts with '{rule.Name}'.");
                else
                    table[(int) nonTerminal, (int) terminal] = rule;
            }
        }

        public Rule this[Symbol symbol, Symbol token] => table[(int)symbol, (int)token];
    }
}
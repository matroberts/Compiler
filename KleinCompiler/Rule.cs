using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace KleinCompiler
{

    public class Rule
    {
        public static Rule R1 => new Rule(Symbol.Def, Symbol.DefTail);
        public static Rule R2 => new Rule(Symbol.Def, Symbol.DefTail);
        public static Rule R3 => new Rule();
        public static Rule R4 => new Rule(Symbol.Identifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type, Symbol.Body);
        public static Rule R5 => new Rule();
        public static Rule R6 => new Rule(Symbol.NonEmptyFormals);
        public static Rule R7 => new Rule(Symbol.Formal, Symbol.FormalTail);
        public static Rule R8 => new Rule(Symbol.Comma, Symbol.Formal, Symbol.FormalTail);
        public static Rule R9 => new Rule();
        public static Rule R10 => new Rule(Symbol.Identifier, Symbol.Colon, Symbol.Type);
        public static Rule R11 => new Rule(Symbol.Print, Symbol.Body);
        public static Rule R12 => new Rule(Symbol.Expr);
        public static Rule R13 => new Rule(Symbol.IntegerType);
        public static Rule R14 => new Rule(Symbol.BooleanType);
        public static Rule R15 => new Rule(Symbol.SimpleExpr, Symbol.SimpleExprTail);
        public static Rule R16 => new Rule(Symbol.LessThan, Symbol.SimpleExpr, Symbol.SimpleExprTail);
        public static Rule R17 => new Rule(Symbol.Equality, Symbol.SimpleExpr, Symbol.SimpleExprTail);
        public static Rule R18 => new Rule();
        public static Rule R19 => new Rule(Symbol.Term, Symbol.TermTail);
        public static Rule R20 => new Rule(Symbol.Or, Symbol.Term, Symbol.TermTail);
        public static Rule R21 => new Rule(Symbol.Plus, Symbol.Term, Symbol.TermTail);
        public static Rule R22 => new Rule(Symbol.Minus, Symbol.Term, Symbol.TermTail);
        public static Rule R23 => new Rule();
        public static Rule R24 => new Rule(Symbol.Factor, Symbol.FactorTail);
        public static Rule R25 => new Rule(Symbol.And, Symbol.Factor, Symbol.FactorTail);
        public static Rule R26 => new Rule(Symbol.Multiply, Symbol.Factor, Symbol.FactorTail);
        public static Rule R27 => new Rule(Symbol.Divide, Symbol.Factor, Symbol.FactorTail);
        public static Rule R28 => new Rule();
        public static Rule R29 => new Rule(Symbol.If, Symbol.Expr, Symbol.Then, Symbol.Expr, Symbol.Else, Symbol.Expr);
        public static Rule R30 => new Rule(Symbol.Not, Symbol.Factor);
        public static Rule R31 => new Rule(Symbol.Func);
        public static Rule R32 => new Rule(Symbol.Literal);
        public static Rule R33 => new Rule(Symbol.Minus, Symbol.Factor);
        public static Rule R34 => new Rule(Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);
        public static Rule R35 => new Rule(Symbol.Identifier, Symbol.FuncTail);
        public static Rule R36 => new Rule(Symbol.OpenBracket, Symbol.Actuals, Symbol.CloseBracket);
        public static Rule R37 => new Rule();
        public static Rule R38 => new Rule();
        public static Rule R39 => new Rule(Symbol.NonEmptyActuals);
        public static Rule R40 => new Rule(Symbol.Expr, Symbol.ActualsTail);
        public static Rule R41 => new Rule(Symbol.Comma, Symbol.Expr, Symbol.ActualsTail);
        public static Rule R42 => new Rule();
        public static Rule R43 => new Rule(Symbol.IntegerLiteral);
        public static Rule R44 => new Rule(Symbol.BooleanTrue);
        public static Rule R45 => new Rule(Symbol.BooleanFalse);
        public static Rule R46 => new Rule(Symbol.PrintKeyword, Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);

        public List<Symbol> Symbols { get; } = new List<Symbol>();
        public Rule(params Symbol[] symbols)
        {
            Symbols.AddRange(symbols);
        }

        public IEnumerable<Symbol> Reverse => (Symbols as IEnumerable<Symbol>).Reverse();
    }
}
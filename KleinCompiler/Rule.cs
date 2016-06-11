using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace KleinCompiler
{

    public class Rule
    {
        public static Rule R1 =>  new Rule("R1 ", Symbol.Def, Symbol.DefTail);
        public static Rule R2 =>  new Rule("R2 ", Symbol.Def, Symbol.DefTail);
        public static Rule R3 =>  new Rule("R3 ");
        public static Rule R4 =>  new Rule("R4 ", Symbol.Identifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type, Symbol.Body);
        public static Rule R5 =>  new Rule("R5 ");
        public static Rule R6 =>  new Rule("R6 ", Symbol.NonEmptyFormals);
        public static Rule R7 =>  new Rule("R7 ", Symbol.Formal, Symbol.FormalTail);
        public static Rule R8 =>  new Rule("R8 ", Symbol.Comma, Symbol.Formal, Symbol.FormalTail);
        public static Rule R9 =>  new Rule("R9 ");
        public static Rule R10 => new Rule("R10", Symbol.Identifier, Symbol.Colon, Symbol.Type);
        public static Rule R11 => new Rule("R11", Symbol.Print, Symbol.Body);
        public static Rule R12 => new Rule("R12", Symbol.Expr);
        public static Rule R13 => new Rule("R13", Symbol.IntegerType);
        public static Rule R14 => new Rule("R14", Symbol.BooleanType);
        public static Rule R15 => new Rule("R15", Symbol.SimpleExpr, Symbol.SimpleExprTail);
        public static Rule R16 => new Rule("R16", Symbol.LessThan, Symbol.SimpleExpr, Symbol.SimpleExprTail);
        public static Rule R17 => new Rule("R17", Symbol.Equality, Symbol.SimpleExpr, Symbol.SimpleExprTail);
        public static Rule R18 => new Rule("R18");
        public static Rule R19 => new Rule("R19", Symbol.Term, Symbol.TermTail);
        public static Rule R20 => new Rule("R20", Symbol.Or, Symbol.Term, Symbol.TermTail);
        public static Rule R21 => new Rule("R21", Symbol.Plus, Symbol.Term, Symbol.TermTail);
        public static Rule R22 => new Rule("R22", Symbol.Minus, Symbol.Term, Symbol.TermTail);
        public static Rule R23 => new Rule("R23");
        public static Rule R24 => new Rule("R24", Symbol.Factor, Symbol.FactorTail);
        public static Rule R25 => new Rule("R25", Symbol.And, Symbol.Factor, Symbol.FactorTail);
        public static Rule R26 => new Rule("R26", Symbol.Multiply, Symbol.Factor, Symbol.FactorTail);
        public static Rule R27 => new Rule("R27", Symbol.Divide, Symbol.Factor, Symbol.FactorTail);
        public static Rule R28 => new Rule("R28");
        public static Rule R29 => new Rule("R29", Symbol.If, Symbol.Expr, Symbol.Then, Symbol.Expr, Symbol.Else, Symbol.Expr);
        public static Rule R30 => new Rule("R30", Symbol.Not, Symbol.Factor);
        public static Rule R31 => new Rule("R31", Symbol.Func);
        public static Rule R32 => new Rule("R32", Symbol.Literal);
        public static Rule R33 => new Rule("R33", Symbol.Minus, Symbol.Factor);
        public static Rule R34 => new Rule("R34", Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);
        public static Rule R35 => new Rule("R35", Symbol.Identifier, Symbol.FuncTail);
        public static Rule R36 => new Rule("R36", Symbol.OpenBracket, Symbol.Actuals, Symbol.CloseBracket);
        public static Rule R37 => new Rule("R37");
        public static Rule R38 => new Rule("R38");
        public static Rule R39 => new Rule("R39", Symbol.NonEmptyActuals);
        public static Rule R40 => new Rule("R40", Symbol.Expr, Symbol.ActualsTail);
        public static Rule R41 => new Rule("R41", Symbol.Comma, Symbol.Expr, Symbol.ActualsTail);
        public static Rule R42 => new Rule("R42");
        public static Rule R43 => new Rule("R43", Symbol.IntegerLiteral);
        public static Rule R44 => new Rule("R44", Symbol.BooleanTrue);
        public static Rule R45 => new Rule("R45", Symbol.BooleanFalse);
        public static Rule R46 => new Rule("R46", Symbol.PrintKeyword, Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);

        public List<Symbol> Symbols { get; } = new List<Symbol>();
        public string Name { get; }
        public Rule(string name, params Symbol[] symbols)
        {
            Name = name;
            Symbols.AddRange(symbols);
        }

        public IEnumerable<Symbol> Reverse => (Symbols as IEnumerable<Symbol>).Reverse();
    }
}
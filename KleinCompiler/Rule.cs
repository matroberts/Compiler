using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace KleinCompiler
{

    public class Rule
    {
        //R1      <Program>             ::= <Def> <DefTail>                                            * left factored the original definitions rule
        public static Rule R1 => new Rule(Symbol.Def, Symbol.DefTail);
        //R2      <DefTail>             ::= <Def> <DefTail>
        public static Rule R2 => new Rule(Symbol.Def, Symbol.DefTail);
        //R3                              | e                                                          <-- here 'e' means no more tokens at all
        public static Rule R3 => new Rule();
        //R4      <Def>                 ::= <Identifier> ( <Formals> ) : <Type> <Body>                 <-- function declaration
        public static Rule R4 => new Rule(Symbol.Identifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type, Symbol.Body);
        //R5      <Formals>             ::= e
        public static Rule R5 => new Rule();
        //R6                              | <NonEmptyFormals>
        public static Rule R6 => new Rule(Symbol.NonEmptyFormals);
        //R7      <NonEmptyFormals>     ::= <Formal><FormalTail>                                       <-- left factored original rule
        public static Rule R7 => new Rule(Symbol.Formal, Symbol.FormalTail);
        //R8      <FormalTail>          ::= , <Formal><FormalTail>
        public static Rule R8 => new Rule(Symbol.Comma, Symbol.Formal, Symbol.FormalTail);
        //R9                              | e
        public static Rule R9 => new Rule();
        //R10     <Formal>              ::= <Identifier> : <Type>
        public static Rule R10 => new Rule(Symbol.Identifier, Symbol.Colon, Symbol.Type);
        //R11     <Body>                ::= <Print> <Body>
        public static Rule R11 => new Rule(Symbol.Print, Symbol.Body);
        //R12                             | <Expr>
        public static Rule R12 => new Rule(Symbol.Expr);
        //R13     <Type>                ::= integer
        public static Rule R13 => new Rule(Symbol.IntegerType);
        //R14                             | boolean
        public static Rule R14 => new Rule(Symbol.BooleanType);
        //R15     <Expr>                ::= <Simple-Expr> <SimpleExprTail>                             * removed left recursion and left factored
        public static Rule R15 => new Rule(Symbol.SimpleExpr, Symbol.SimpleExprTail);
        //R16     <SimpleExprTail>      ::= < <Simple-Expr> <SimpleExprTail>
        public static Rule R16 => new Rule(Symbol.LessThan, Symbol.SimpleExpr, Symbol.SimpleExprTail);
        //R17                             | = <Simple-Expr> <SimpleExprTail>
        public static Rule R17 => new Rule(Symbol.Equality, Symbol.SimpleExpr, Symbol.SimpleExprTail);
        //R18                             | e
        public static Rule R18 => new Rule();
        //R19     <Simple-Expr>         ::= <Term> <TermTail>                                          * removed left recursion and left factored
        public static Rule R19 => new Rule(Symbol.Term, Symbol.TermTail);
        //R20     <TermTail>            ::= or <Term> <TermTail>
        public static Rule R20 => new Rule(Symbol.Or, Symbol.Term, Symbol.TermTail);
        //R21                             | + <Term> <TermTail>
        public static Rule R21 => new Rule(Symbol.Plus, Symbol.Term, Symbol.TermTail);
        //R22                             | - <Term> <TermTail>
        public static Rule R22 => new Rule(Symbol.Minus, Symbol.Term, Symbol.TermTail);
        //R23                             | e
        public static Rule R23 => new Rule();
        //R24     <Term>                ::= <Factor><FactorTail>                                       * removed left recursion and left factored
        public static Rule R24 => new Rule(Symbol.Factor, Symbol.FactorTail);
        //R25     <FactorTail>          ::= and <Factor><FactorTail>
        public static Rule R25 => new Rule(Symbol.And, Symbol.Factor, Symbol.FactorTail);
        //R26                             | * <Factor><FactorTail>
        public static Rule R26 => new Rule(Symbol.Multiply, Symbol.Factor, Symbol.FactorTail);
        //R27                             | / <Factor><FactorTail>
        public static Rule R27 => new Rule(Symbol.Divide, Symbol.Factor, Symbol.FactorTail);
        //R28                             | e
        public static Rule R28 => new Rule();
        //R29     <Factor>              ::= if <Expr> then <Expr> else <Expr>
        public static Rule R29 => new Rule(Symbol.If, Symbol.Expr, Symbol.Then, Symbol.Expr, Symbol.Else, Symbol.Expr);
        //R30                             | not <Factor>
        public static Rule R30 => new Rule(Symbol.Not, Symbol.Factor);
        //R31                             | <Func>
        public static Rule R31 => new Rule(Symbol.Func);
        //R32                             | <Literal>
        public static Rule R32 => new Rule(Symbol.Literal);
        //R33                             | - <Factor>
        public static Rule R33 => new Rule(Symbol.Minus, Symbol.Factor);
        //R34                             | ( <Expr> )
        public static Rule R34 => new Rule(Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);
        //R35    <Func>                 ::= <Identifier><FuncTail>
        public static Rule R35 => new Rule(Symbol.Identifier, Symbol.FuncTail);
        //R36    <FuncTail>             ::= ( <Actuals> )
        public static Rule R36 => new Rule(Symbol.OpenBracket, Symbol.Actuals, Symbol.CloseBracket);
        //R37                             | e
        public static Rule R37 => new Rule();
        //R38     <Actuals>             ::= e
        public static Rule R38 => new Rule();
        //R39                             | <NonEmptyActuals>
        public static Rule R39 => new Rule(Symbol.NonEmptyActuals);
        //R40     <NonEmptyActuals>     ::= <Expr><ActualsTail>                                        * left factored original rule
        public static Rule R40 => new Rule(Symbol.Expr, Symbol.ActualsTail);
        //R41     <ActualsTail>         ::= , <Expr><ActualsTail>
        public static Rule R41 => new Rule(Symbol.Comma, Symbol.Expr, Symbol.ActualsTail);
        //R42                             | e
        public static Rule R42 => new Rule();
        //R43     <Literal>             ::= <Number>
        public static Rule R43 => new Rule(Symbol.IntegerLiteral);
        //R44                             | <BooleanTrue>
        public static Rule R44 => new Rule(Symbol.BooleanTrue);
        //R45                             | <BooleanFalse>
        public static Rule R45 => new Rule(Symbol.BooleanFalse);
        //R46     <Print>               ::= print ( <Expr> )                                                                         
        public static Rule R46 => new Rule(Symbol.PrintKeyword, Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);

        public List<Symbol> Symbols { get; } = new List<Symbol>();
        public Rule(params Symbol[] symbols)
        {
            Symbols.AddRange(symbols);
        }

        public IEnumerable<Symbol> Reverse => (Symbols as IEnumerable<Symbol>).Reverse();
    }
}
namespace KleinCompiler
{
    public class ParsingTable
    {
        /*
 Klein Grammar
 =============
R1      <Program>             ::= <Def> <DefTail>                                            * left factored the original definitions rule
R2      <DefTail>             ::= <Def> <DefTail>
R3                              | e                                                          <-- here 'e' means no more tokens at all
R4      <Def>                 ::= <Identifier> ( <Formals> ) : <Type> <Body>                 <-- function declaration
R5      <Formals>             ::= e
R6                              | <NonEmptyFormals>
R7      <NonEmptyFormals>     ::= <Formal><FormalTail>                                       <-- left factored original rule
R8      <FormalTail>          ::= , <Formal><FormalTail>
R9                              | e
R10     <Formal>              ::= <Identifier> : <Type>
R11     <Body>                ::= <Print> <Body>
R12                             | <Expr>
R13     <Type>                ::= integer
R14                             | boolean
R15     <Expr>                ::= <Simple-Expr> <SimpleExprTail>                             * removed left recursion and left factored
R16     <SimpleExprTail>      ::= < <Simple-Expr> <SimpleExprTail>
R17                             | = <Simple-Expr> <SimpleExprTail>
R18                             | e
R19     <Simple-Expr>         ::= <Term> <TermTail>                                          * removed left recursion and left factored
R20     <TermTail>            ::= or <Term> <TermTail>
R21                             | + <Term> <TermTail>
R22                             | - <Term> <TermTail>
R23                             | e
R24     <Term>                ::= <Factor><FactorTail>                                       * removed left recursion and left factored
R25     <FactorTail>          ::= and <Factor><FactorTail>
R26                             | * <Factor><FactorTail>
R27                             | / <Factor><FactorTail>
R28                             | e
R29     <Factor>              ::= if <Expr> then <Expr> else <Expr>
R30                             | not <Factor>
R31                             | <Func>
R32                             | <Literal>
R33                             | - <Factor>
R34                             | ( <Expr> )
R35    <Func>                 ::= <Identifier><FuncTail>
R36    <FuncTail>             ::= ( <Actuals> )
R37                             | e
R38     <Actuals>             ::= e
R39                             | <NonEmptyActuals>
R40     <NonEmptyActuals>     ::= <Expr><ActualsTail>                                        * left factored original rule
R41     <ActualsTail>         ::= , <Expr><ActualsTail>
R42                             | e
R43     <Literal>             ::= <Number>
R44                             | <Boolean>
R45     <Print>               ::= print ( <Expr> )                                                                         
    
 */
    }
}
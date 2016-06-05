namespace KleinCompiler
{
    public class ParsingTable
    {
        /*
 Klein Grammar
 =============
 <Program>             ::= <Def> <DefTail>                                            * left factored the original definitions rule
 <DefTail>             ::= <Def> <DefTail>
                         | e                                                          <-- here 'e' means no more tokens at all
 <Def>                 ::= <Identifier> ( <Formals> ) : <Type> <Body>                 <-- function declaration
 <Formals>             ::= e
                         | <NonEmptyFormals>
 <NonEmptyFormals>     ::= <Formal><FormalTail>                                       <-- left factored original rule
 <FormalTail>          ::= , <Formal><FormalTail>
                         | e
 <Formal>              ::= <Identifier> : <Type>
 <Body>                ::= <Print> <Body>
                         | <Expr>
 <Type>                ::= integer
                         | boolean
 <Expr>                ::= <Simple-Expr> <SimpleExprTail>                             * removed left recursion and left factored
 <SimpleExprTail>      ::= < <Simple-Expr> <SimpleExprTail>
                         | = <Simple-Expr> <SimpleExprTail>
                         | e
 <Simple-Expr>         ::= <Term> <TermTail>                                          * removed left recursion and left factored
 <TermTail>            ::= or <Term> <TermTail>
                         | + <Term> <TermTail>
                         | - <Term> <TermTail>
                         | e
 <Term>                ::= <Factor><FactorTail>                                       * removed left recursion and left factored
 <FactorTail>          ::= and <Factor><FactorTail>
                         | * <Factor><FactorTail>
                         | / <Factor><FactorTail>
                         | e
 <Factor>              ::= if <Expr> then <Expr> else <Expr>
                         | not <Factor>
                         | <Func>
                         | <Literal>
                         | - <Factor>
                         | ( <Expr> )
<Func>                 ::= <Identifier><FuncTail>
<FuncTail>             ::= ( <Actuals> )
                         | e
 <Actuals>             ::= e
                         | <NonEmptyActuals>
 <NonEmptyActuals>     ::= <Expr><ActualsTail>                                        * left factored original rule
 <ActualsTail>         ::= , <Expr><ActualsTail>
                         | e
 <Literal>             ::= <Number>
                         | <Boolean>
 <Print>               ::= print ( <Expr> )                                                                         






 */
    }
}
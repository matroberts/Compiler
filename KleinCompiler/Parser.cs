namespace KleinCompiler
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
    <NonEmptyFormals>     ::= <Formals> <NonEmptyFormalTail>                             * left factored the original rule
    <NonEmptyFormalTail>  ::= , <NonEmptyFormals>
                            | e
    <Formal>              ::= <Identifier> : <Type>
    <Body>                ::= <Print> <Body>
                            | <Expr>
    <Type>                ::= integer
                            | boolean
    <Expr>                ::= <Expr> < <Simple-Expr>                                     <-- left recursion
                            | <Expr> = <Simple-Expr>
                            | <Simple-Expr>
    <Simple-Expr>         ::= <Simple-Expr> or <Term>                                    <-- left recursion
                            | <Simple-Expr> + <Term>
                            | <Simple-Expr> - <Term>
                            | <Term>
    <Term>                ::= <Term> and <Factor>                                        <-- left recursion
                            | <Term> * <Factor>
                            | <Term> / <Factor>
                            | <Factor>
    <Factor>              ::= if <Expr> then <Expr> else <Expr>
                            | not <Factor>
                            | <Identifier> ( <Actuals> )                                 <-- function call. this should be fine with one token lookahead, cos after an identifier can see if there is a bracket to decide between this rule and the one after
                            | <Identifier>
                            | <Literal>
                            | - <Factor>
                            | ( <Expr> )
    <Actuals>             ::= e
                            | <NonEmptyActuals>
    <NonEmptyActuals>     ::= <Expr> <NonEmptyActualsTail>                               * left factored the original rule
    <NonEmptyActualsTail> ::= , <NonEmptyActuals>
                            | e
    <Literal>             ::= <Number>
                            | <Boolean>
    <Print>               ::= print ( <Expr> )                                                                         






    */
    public class Parser
    {
         
    }
}
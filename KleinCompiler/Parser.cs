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
    <Expr>                ::= <Simple-Expr> <SimpleExprTail>                             * removed left recursion and left factored
    <SimpleExprTail>      ::= < <Expr>
                            | = <Expr>
                            | e
    <Simple-Expr>         ::= <Term> <TermTail>                                          * removed left recursion and left factored
    <TermTail>            ::= or <Simple-Expr>
                            | + <Simple-Expr>
                            | - <Simple-Expr>
                            | e
    <Term>                ::= <Factor><FactorTail>                                       * removed left recursion and left factored
    <FactorTail>          ::= and <Term>
                            | * <Term>
                            | / <Term>
                            | e
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
         /*
          Here is a simple table-driven algorithm. A parser using this algorithm acts on the basis of the current token i in the input stream and the symbol A on top of the stack, until it reaches the end of the input stream, denoted by $.

Push the end of stream symbol, $, onto the stack.

Push the start symbol onto the stack.

Repeat
A is a terminal.
If A == i, then

Pop A from the stack and consume i.
Else we have a token mismatch. Fail.
A is a non-terminal.
If table entry [A, i] contains a rule A := Y1, Y2, ... Yn, then

Pop A from the stack. Push Yn, Yn-1, ... Y1 onto the stack, in that order.
Else there is no transition for this pair. Fail.
until A == $.
*/
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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
    public class Parser
    {
        /*
        The parser acts on the basis of the current token i in the input stream and 
        the symbol A on top of the stack, until it reaches the end of the input stream, denoted by $.

        Push the end of stream symbol, $, onto the stack.
        Push the start symbol onto the stack.

        Repeat
        A is a terminal.
        If A == i, then
           Pop A from the stack and consume i.
        Else we have a token mismatch. 
           Fail.

        A is a non-terminal.
        If table entry [A, i] contains a rule A := Y1, Y2, ... Yn, then
           Pop A from the stack. Push Yn, Yn-1, ... Y1 onto the stack, in that order.
        Else there is no transition for this pair. 
           Fail.

        until A == $.
        */

        public Parser(IParsingTable parsingTable)
        {
            this.parsingTable = parsingTable;
        }

        private Stack<Symbol> symbolStack = new Stack<Symbol>();
        private IParsingTable parsingTable;

        public bool Parse(Tokenizer tokenizer)
        {
            symbolStack.Push(Symbol.End);
            symbolStack.Push(Symbol.Program);

            while (symbolStack.Count != 0)
            {
                Symbol symbol = symbolStack.Pop();

                Token token = tokenizer.Peek();
                if (symbol == token.Symbol)
                {
                    tokenizer.Pop();  // consume symbol
                }
                else
                {
                    var rule = parsingTable[symbol, token.Symbol];
                    if (rule == null)
                    {
                        return false;
                    }
                    else
                    {
                        symbolStack.Push(rule.Reverse);
                    }
                }
/*
                if (symbol is NonTerminalSymbol)
                {
                    var rule = parsingTable.Rule(symbol, token);
                    if (rule == null)
                    {
                        // error
                    }
                    else
                    {
                        //symbolStack.Push(rule.Reversed());
                    }
                }
                else if (symbol is Token)
                {
                    if (symbol == token)
                    {
                        // great
                    }
                    else
                    {
                        // error
                        return false;
                    }
                }
                else if (symbol == null)
                {
                    if (token == null)
                    {
                        // great
                    }
                    else
                    {
                        // fail - program stack is empty, but there are still tokens in the stream
                        return false;
                    }
                }*/
            }
            return true;
        }
    }
}
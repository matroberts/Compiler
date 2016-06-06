using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace KleinCompiler
{
    /*
        First(Print)           = print
        First(Number)          = IntegerLiteral
        First(Boolean)         = BooleanTrue BooleanFalse
        First(Literal)         = IntegerLiteral BooleanTrue BooleanFalse
        First(ActualsTail>     = , e
        First(NonEmptyActuals> = First(Expr)
                               = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
        First(Actuals)         = e First(NonEmptyActuals)
        First(FuncTail)        = ( e
        First(Func)            = Identfier
        First(Factor)          = if not First(Func) First(Literal) - (
                               = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
        First(FactorTail)      = and * / e
        First(Term)            = First(Factor)
                               = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
        First(TermTail)        = or + - e
        First(Simple-Expr)     = First(Term)
                               = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
        First(SimpleExprTail)  = < = e
        First(Expr)            = First(Simple-Expr)
                               = if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
        First(Type)            = integer boolean
        First(Body)            = First(Print) First(Expr)
                               = print if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
        First(Formal)          = Identifier
        First(FormalTail)      = , e
        First(NonEptyFormals)  = First(Formal)
                               = Identifier
        First(Formals)         = e First(NonEmptyForamls)
                               = e Identifier
        First(Def)             = Identifier
        First(DefTail)         = First(Def) e
                               = Identifier e
        First(Program)         = First(Def)
                               = Identifier
      

        Follow(Program)        = End
        Follow(DefTail)        = Follow(Program)
                               = End
        Follow(Def)            = First(DefTail - e) Follow(Program) Follow(DefTail)
                               = Identifier End
        Follow(Formals)        = ) 
        Follow(NonEmptyformals)= Follow(Formals)
                               = )
        Follow(FormalTail)     = Follow(NonEmptyFormals)
                               = )
        Follow(Formal)         = First(FormalTail - e) Follow(NonEmptyFormals) Follow(FormalTail)
                               = , )
        Follow(Body)           = 
        Follow(Type)           = First(Body - e) Follow(Formal)
                               = print if not Identfier IntegerLiteral BooleanTrue BooleanFalse - ( , )
        Follow(Expr)           = Follow(Body) then else ) Follow(Factor) First(ActualsTail - e) Follow(NonEmptyActuals) Follow(ActualsTail)
                               =              then else ) Follow(Factor) , Follow(NonEmptyActuals) Follow(ActualsTail)
                               =              then else ) and * /  or + - < = , 
        Follow(SimpleExprTail) = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(Simple-Expr)    = First(SimpleExprTail - e) Follow(Expr) Follow(SimpleExprTail)
                               = < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(TermTail)       = Follow(Simple-Expr) 
                               = < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(Term)           = First(TermTail) Follow(Simple-Expr) Follow(TermTail) 
                               = or + - < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(FactorTail)     = Follow(Term) 
                               = or + - < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(Factor)         = First(FactorTail -e) Follow(Term) 
                               = and * /  or + - < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(Func)           = Follow(Factor)
                               = and * /  or + - < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(FuncTail)       = Follow(Func)
                               = and * /  or + - < = Follow(Expr)
                               = then else ) and * /  or + - < = , 
        Follow(Actuals)        = )
        Follow(NonEmptyActuals)= Follow(Actuals)
                               = )
        Follow(ActualsTail)    = Follow(NonEmptyActuals) 
                               = )
        Follow(Literal)        = Follow(Factor)
                               = then else ) and * /  or + - < = , 
        Follow(Print)          = First(Body - e) 
                               = print if not Identfier IntegerLiteral BooleanTrue BooleanFalse - (
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
            this.Error = string.Empty;
        }

        private Stack<Symbol> symbolStack = new Stack<Symbol>();
        private IParsingTable parsingTable;

        public string Error { get; private set; }

        public bool Parse(Tokenizer tokenizer)
        {
            symbolStack.Push(Symbol.End);
            symbolStack.Push(Symbol.Program);

            while (symbolStack.Count != 0)
            {
                ToConsole(tokenizer.Peek(), symbolStack);
                
                Symbol symbol = symbolStack.Pop();

                Token token = tokenizer.Peek();
                if (symbol == token.Symbol)
                {
                    tokenizer.Pop();
                }
                else
                {
                    var rule = parsingTable[symbol, token.Symbol];
                    if (rule == null)
                    {
                        Error = $"Attempting to parse symbol '{symbol.ToString()}' found token {token.ToString()}";
                        return false;
                    }
                    else
                    {
                        symbolStack.Push(rule.Reverse);
                    }
                }
            }
            return true;
        }

        private static void ToConsole(Token token, Stack<Symbol> stack)
        {
            Console.Write(token.Symbol.ToString().PadRight(20));
            foreach (var symbol in stack)
            {
                Console.Write(symbol + " ");
            }
            Console.WriteLine();
        }
    }
}
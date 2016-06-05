using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace KleinCompiler
{
 
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
                ToConsole(tokenizer.Peek(), symbolStack);
                
                Symbol symbol = symbolStack.Pop();

                Token token = tokenizer.Peek();
                if (symbol == token.Symbol)
                {
                    tokenizer.GetNextToken();
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
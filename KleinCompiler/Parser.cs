﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KleinCompiler
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
    public class Parser
    {
        public Parser(ParsingTable parsingTable)
        {
            this.parsingTable = parsingTable;
            this.Error = null;
        }
        private ParsingTable parsingTable;

        private Stack<Symbol> symbolStack = new Stack<Symbol>();

        private Stack<Ast> semanticStack = new Stack<Ast>();

        public Error Error { get; private set; }


        public bool EnableStackTrace { get; set; } = false;
        private readonly StringBuilder symbolStackTraceBuilder = new StringBuilder();
        public string SymbolStackTrace => symbolStackTraceBuilder.ToString();

        public Ast Ast => semanticStack.Pop();

        public bool Parse(Tokenizer tokenizer)
        {
            symbolStack.Push(parsingTable.LastSymbol);
            symbolStack.Push(parsingTable.FirstSymbol);

            Token lastToken = null;
            while (symbolStack.Count != 0)
            {
                if (EnableStackTrace)
                {
                    TraceStack(tokenizer.Peek(), symbolStack);
                }

                Symbol symbol = symbolStack.Pop();
                Token token = tokenizer.Peek();

                if (token is ErrorToken)
                {
                    Error = Error.CreateLexicalError(token as ErrorToken);
                    return false;
                }

                if (symbol.ToSymbolType() == SymbolType.Token)
                {
                    if (symbol == token.Symbol)
                    {
                        lastToken = tokenizer.Pop();
                    }
                    else
                    {
                        Error = Error.CreateSyntaxError(symbol, token);
                        return false;
                    }
                }
                else if (symbol.ToSymbolType() == SymbolType.NonTerminal)
                {
                    var rule = parsingTable[symbol, token.Symbol];
                    if (rule == null)
                    {
                        Error = Error.CreateSyntaxError(symbol, token);
                        return false;
                    }
                    else
                    {
                        symbolStack.Push(rule.Reverse);
                    }
                }
                else if (symbol.ToSymbolType() == SymbolType.Semantic)
                {
                    AstFactory.ProcessAction(semanticStack, symbol, lastToken);
                }
            }
            return true;
        }

        private void TraceStack(Token token, Stack<Symbol> stack)
        {
            symbolStackTraceBuilder.Append(token.Symbol.ToString().PadRight(20));
            symbolStackTraceBuilder.Append(token.Value.ToString().PadRight(20));
            foreach (var symbol in stack)
            {
                symbolStackTraceBuilder.Append(symbol + " ");
            }
            symbolStackTraceBuilder.AppendLine();
        }
    }
}
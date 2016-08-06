using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KleinCompiler.AbstractSyntaxTree;

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
        public Parser() : this(ParsingTableFactory.Create(), new AstFactory())
        {
        }
        public Parser(ParsingTable parsingTable, IAstFactory astFactory)
        {
            this.parsingTable = parsingTable;
            this.astFactory = astFactory;
            this.Error = Error.CreateNoError();
        }

        private ParsingTable parsingTable;
        private IAstFactory astFactory;
        private Stack<Symbol> symbolStack = new Stack<Symbol>();
        private Stack<Ast> semanticStack = new Stack<Ast>();
        public Error Error { get; private set; }

        public bool EnableStackTrace { get; set; } = false;
        private readonly StringBuilder stackTraceBuilder = new StringBuilder();
        public string StackTrace => stackTraceBuilder.ToString();

        public Ast Parse(Tokenizer tokenizer)
        {
            symbolStack.Push(parsingTable.LastSymbol);
            symbolStack.Push(parsingTable.FirstSymbol);

            Token lastToken = null;
            while (symbolStack.Count != 0)
            {
                if (EnableStackTrace)
                {
                    TraceStack(tokenizer.Peek(), symbolStack, semanticStack);
                }

                Symbol symbol = symbolStack.Pop();
                Token token = tokenizer.Peek();

                if (token is ErrorToken)
                {
                    Error = Error.CreateLexicalError(token as ErrorToken, StackTrace);
                    return null;
                }

                if (symbol.ToSymbolType() == SymbolType.Token)
                {
                    if (symbol == token.Symbol)
                    {
                        lastToken = tokenizer.Pop();
                    }
                    else
                    {
                        Error = Error.CreateSyntaxError(symbol, token, StackTrace);
                        return null;
                    }
                }
                else if (symbol.ToSymbolType() == SymbolType.NonTerminal)
                {
                    var rule = parsingTable[symbol, token.Symbol];
                    if (rule == null)
                    {
                        Error = Error.CreateSyntaxError(symbol, token, StackTrace);
                        return null;
                    }
                    else
                    {
                        symbolStack.Push(rule.Reverse);
                    }
                }
                else if (symbol.ToSymbolType() == SymbolType.Semantic)
                {
                    astFactory.ProcessAction(semanticStack, symbol, lastToken);
                }
            }
            return semanticStack.Peek();
        }

        private int w1 = 15;
        private int w2 = 10;
        private int w3 = 50;
        private int w4 = 1;
        private int w5 = 50;

        private void TraceStack(Token token, Stack<Symbol> symStack, Stack<Ast> semStack)
        {
            if (stackTraceBuilder.Length == 0)
            {
                stackTraceBuilder.Append("Token".PadRight(w1));
                stackTraceBuilder.Append("Value".PadRight(w2));
                stackTraceBuilder.Append("Symbol Stack".PadRight(w3));
                stackTraceBuilder.Append(' ', w4);
                stackTraceBuilder.Append("Semantic Stack".PadRight(w5));
                stackTraceBuilder.AppendLine();

                stackTraceBuilder.Append("=====".PadRight(w1));
                stackTraceBuilder.Append("=====".PadRight(w2));
                stackTraceBuilder.Append("===========".PadRight(w3));
                stackTraceBuilder.Append(' ', w4);
                stackTraceBuilder.Append("==============".PadRight(w5));
                stackTraceBuilder.AppendLine();
            }
            stackTraceBuilder.Append(token.Symbol.ToString().PadRight(w1));
            stackTraceBuilder.Append(token.Value.PadRight(w2));
            stackTraceBuilder.Append(string.Join(" ", symStack.ToArray()).PadAndTruncate(w3));
            stackTraceBuilder.Append(' ', w4);
            stackTraceBuilder.Append(string.Join(" ", semStack.Select(t => t.GetType().Name)).PadAndTruncate(w5));
            stackTraceBuilder.AppendLine();
        }
    }
}
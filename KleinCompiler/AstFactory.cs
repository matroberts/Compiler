using System;
using System.Collections.Generic;
using System.Text;

namespace KleinCompiler
{
    public class AstFactory
    {
        public static void ProcessAction(Stack<Ast> semanticStack, Symbol symbol, Token token)
        {
            TraceStack(semanticStack, symbol, token);
            switch (symbol)
            {
                case Symbol.MakePlus:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    var node = new BinaryOperator() {Left = (Expr) left, Operator = "+", Right = (Expr) right};
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeTimes:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    var node = new BinaryOperator() {Left = (Expr) left, Operator = "*", Right = (Expr) right};
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeIdentifier:
                {
                    var value = token.Value;
                    var node = new Identifier() {Value = value};
                    semanticStack.Push(node);
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }
        private static void TraceStack(Stack<Ast> semanticStack, Symbol symbol, Token token)
        {
            var semanticTraceBuilder = new StringBuilder();
            semanticTraceBuilder.Append(symbol.ToString().PadRight(20));
            semanticTraceBuilder.Append(token.ToString().PadRight(20));
            foreach (var item in semanticStack)
            {
                semanticTraceBuilder.Append(item + " ");
            }
            semanticTraceBuilder.AppendLine();
            Console.WriteLine(semanticTraceBuilder.ToString());
        }
    }
}
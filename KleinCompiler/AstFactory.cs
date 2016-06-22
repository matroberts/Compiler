using System;
using System.Collections.Generic;
using System.Text;

namespace KleinCompiler
{
    public class AstFactory
    {
        public static void ProcessAction(Stack<Ast> semanticStack, Symbol symbol, Token token)
        {
            switch (symbol)
            {
                case Symbol.MakeDefinition:
                {
                    var type = semanticStack.Pop();
                    var identifier = semanticStack.Pop();
                    var node = new Definition(identifier: (Identifier)identifier, type: (KleinType)type );
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakePlus:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    var node = new BinaryOperator(left: (Expr)left, op: "+", right: (Expr)right);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeTimes:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    var node = new BinaryOperator(left: (Expr)left, op: "*", right: (Expr)right);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeIdentifier:
                {
                    var value = token.Value;
                    var node = new Identifier(value);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeType:
                {
                    var value = token.Value;
                    var node = new KleinType(value);
                    semanticStack.Push(node);
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleinCompiler
{
    public interface IAstFactory
    {
        void ProcessAction(Stack<Ast> semanticStack, Symbol symbol, Token lastToken);
    }

    public class AstFactory : IAstFactory
    {
        public void ProcessAction(Stack<Ast> semanticStack, Symbol symbol, Token lastToken)
        {
            switch (symbol)
            {
                case Symbol.MakeProgram:
                {
                    semanticStack.Push(new Program(semanticStack.Cast<Definition>().Reverse().ToList()));     
                    return;               
                }
                case Symbol.MakeDefinition:
                {
                    var body = semanticStack.Pop();
                    var type = semanticStack.Pop();
                    var formals = new Stack<Formal>();
                    while (semanticStack.Peek() is Formal)
                    {
                        formals.Push(semanticStack.Pop() as Formal);
                    }
                    var identifier = semanticStack.Pop();

                    var node = new Definition(identifier: (Identifier)identifier, type: (KleinType)type, formals: formals.ToList(), body: (Body)body);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeBody:
                {
                    var expr = semanticStack.Pop();
                    semanticStack.Push(new Body(expr: (Expr)expr));
                    return;
                }
                case Symbol.MakePlus:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    var node = new BinaryOperator(left: (Expr)left, op: BOp.Plus, right: (Expr)right);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeTimes:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    var node = new BinaryOperator(left: (Expr)left, op: BOp.Times, right: (Expr)right);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeFormal:
                {
                    var type = semanticStack.Pop();
                    var identifier = semanticStack.Pop();
                    semanticStack.Push(new Formal(identifier: (Identifier)identifier, type: (KleinType)type));
                    return;
                }
                case Symbol.MakeIdentifier:
                {
                    var value = lastToken.Value;
                    var node = new Identifier(value);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeIntegerType:
                {
                    var node = new KleinType(KType.Integer);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeBooleanType:
                {
                    var node = new KleinType(KType.Boolean);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeIntegerLiteral:
                {
                    var node = new IntegerLiteral(lastToken.Value);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeMakeBooleanTrueLiteral:
                {
                    var node = new BooleanLiteral(true);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeMakeBooleanFalseLiteral:
                {
                    var node = new BooleanLiteral(false);
                    semanticStack.Push(node);
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }
    }
}
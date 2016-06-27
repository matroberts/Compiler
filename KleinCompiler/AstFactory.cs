using System;
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
                    var definitions = new Stack<Definition>();
                    while (semanticStack.Count > 0)
                    {
                        definitions.Push((Definition)semanticStack.Pop());
                    }
                    semanticStack.Push(new Program(definitions.ToList()));     
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
                case Symbol.MakeFormal:
                {
                    var type = semanticStack.Pop();
                    var identifier = semanticStack.Pop();
                    semanticStack.Push(new Formal(identifier: (Identifier)identifier, type: (KleinType)type));
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
                case Symbol.MakeBody:
                {
                    var expr = semanticStack.Pop();
                    var prints = new Stack<Print>();
                    while (semanticStack.Peek() is Print)
                    {
                        prints.Push((Print)semanticStack.Pop());
                    }
                    semanticStack.Push(new Body(expr: (Expr)expr, prints: prints.ToList()));
                    return;
                }
                case Symbol.MakePrint:
                {
                    var expr = semanticStack.Pop();
                    semanticStack.Push(new Print(expr: (Expr)expr));
                    return;
                }
                case Symbol.MakeLessThan:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.LessThan, semanticStack));
                    return;
                }
                case Symbol.MakeEquals:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.Equals, semanticStack));
                    return;
                }
                case Symbol.MakeOr:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.Or, semanticStack));
                    return;
                }
                case Symbol.MakePlus:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.Plus, semanticStack));
                    return;
                }
                case Symbol.MakeMinus:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.Minus, semanticStack));
                    return;
                }
                case Symbol.MakeAnd:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.And, semanticStack));
                    return;
                }
                case Symbol.MakeTimes:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.Times, semanticStack));
                    return;
                }
                case Symbol.MakeDivide:
                {
                    semanticStack.Push(CreateBinaryOperator(BOp.Divide, semanticStack));
                    return;
                }
                case Symbol.MakeNot:
                {
                    semanticStack.Push(CreateUnaryOperator(UOp.Not, semanticStack));
                    return;
                }
                case Symbol.MakeNegate:
                {
                    semanticStack.Push(CreateUnaryOperator(UOp.Negate, semanticStack));
                    return;
                }
                case Symbol.MakeIfThenElse:
                {
                    var elseExpr = semanticStack.Pop();
                    var thenExpr = semanticStack.Pop();
                    var ifExpr = semanticStack.Pop();
                    semanticStack.Push(new IfThenElse(ifExpr: (Expr)ifExpr, thenExpr: (Expr)thenExpr, elseExpr: (Expr)elseExpr));
                    return;
                }
                case Symbol.MakeFunctionCall:
                {
                    var actuals = new Stack<Actual>();
                    while (semanticStack.Peek() is Actual)
                    {
                        actuals.Push((Actual) semanticStack.Pop());
                    }
                    var identifier = semanticStack.Pop();
                    semanticStack.Push(new FunctionCall(identifier: (Identifier) identifier, actuals: actuals.ToList()));
                    return;
                }
                case Symbol.MakeActual:
                {
                    var expr = semanticStack.Pop();
                    semanticStack.Push(new Actual(expr: (Expr)expr));
                    return;
                }
                case Symbol.MakeIdentifier:
                {
                    semanticStack.Push(new Identifier(lastToken.Value));
                    return;
                }
                case Symbol.MakeIntegerLiteral:
                {
                    semanticStack.Push(new IntegerLiteral(lastToken.Value));
                    return;
                }
                case Symbol.MakeMakeBooleanTrueLiteral:
                {
                    semanticStack.Push(new BooleanLiteral(true));
                    return;
                }
                case Symbol.MakeMakeBooleanFalseLiteral:
                {
                    semanticStack.Push(new BooleanLiteral(false));
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private UnaryOperator CreateUnaryOperator(UOp uop, Stack<Ast> semanticStack)
        {
            var right = semanticStack.Pop();
            return new UnaryOperator(op: uop, right: (Expr)right);
        }

        private BinaryOperator CreateBinaryOperator(BOp bop, Stack<Ast> semanticStack)
        {
            var right = semanticStack.Pop();
            var left = semanticStack.Pop();
            return new BinaryOperator(left: (Expr)left, op: bop, right: (Expr)right);
        }
    }
}
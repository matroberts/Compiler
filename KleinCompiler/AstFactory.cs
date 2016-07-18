using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler
{
    public interface IAstFactory
    {
        void ProcessAction(Stack<Ast> semanticStack, Symbol symbol, Token lastToken);
    }

    public class AstFactory : IAstFactory
    {
        private Stack<int> PositionStack = new Stack<int>();

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
                    var typeDeclaration = semanticStack.Pop();
                    var formals = new Stack<Formal>();
                    while (semanticStack.Peek() is Formal)
                    {
                        formals.Push(semanticStack.Pop() as Formal);
                    }
                    var identifier = semanticStack.Pop();

                    var node = new Definition(identifier: (Identifier)identifier, typeDeclaration: (TypeDeclaration)typeDeclaration, formals: formals.ToList(), body: (Body)body);
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeFormal:
                {
                    var typeDeclaration = semanticStack.Pop();
                    var identifier = semanticStack.Pop();
                    semanticStack.Push(new Formal(identifier: (Identifier)identifier, typeDeclaration: (TypeDeclaration)typeDeclaration));
                    return;
                }
                case Symbol.MakeIntegerTypeDeclaration:
                {
                    var node = new IntegerTypeDeclaration();
                    semanticStack.Push(node);
                    return;
                }
                case Symbol.MakeBooleanTypeDeclaration:
                {
                    var node = new BooleanTypeDeclaration();
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
                    semanticStack.Push(new Print(position: PositionStack.Pop(), expr: (Expr)expr));
                    return;
                }
                case Symbol.MakeLessThan:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new LessThanOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeEquals:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new EqualsOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeOr:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new OrOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakePlus:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new PlusOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeMinus:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new MinusOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeAnd:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new AndOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeTimes:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new TimesOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeDivide:
                {
                    var right = semanticStack.Pop();
                    var left = semanticStack.Pop();
                    semanticStack.Push(new DivideOperator(position: PositionStack.Pop(), left: (Expr)left, right: (Expr)right));
                    return;
                }
                case Symbol.MakeNot:
                {
                    var right = semanticStack.Pop();
                    semanticStack.Push(new NotOperator(position: PositionStack.Pop(), right: (Expr)right));
                    return;
                }
                case Symbol.MakeNegate:
                {
                    var right = semanticStack.Pop();
                    semanticStack.Push(new NegateOperator(position: PositionStack.Pop(), right: (Expr)right));
                    return;
                }
                case Symbol.MakeIfThenElse:
                {
                    var elseExpr = semanticStack.Pop();
                    var thenExpr = semanticStack.Pop();
                    var ifExpr = semanticStack.Pop();
                    semanticStack.Push(new IfThenElse(position: PositionStack.Pop(), ifExpr: (Expr)ifExpr, thenExpr: (Expr)thenExpr, elseExpr: (Expr)elseExpr));
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
                    semanticStack.Push(new Identifier(lastToken.Position, lastToken.Value));
                    return;
                }
                case Symbol.MakeIntegerLiteral:
                {
                    semanticStack.Push(new IntegerLiteral(lastToken.Position, lastToken.Value));
                    return;
                }
                case Symbol.MakeMakeBooleanTrueLiteral:
                {
                    semanticStack.Push(new BooleanLiteral(lastToken.Position, true));
                    return;
                }
                case Symbol.MakeMakeBooleanFalseLiteral:
                {
                    semanticStack.Push(new BooleanLiteral(lastToken.Position, false));
                    return;
                }
                case Symbol.PushPosition:
                {
                    PositionStack.Push(lastToken.Position);
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }
    }
}
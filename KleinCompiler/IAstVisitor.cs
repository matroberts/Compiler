using System;

namespace KleinCompiler
{
    public interface IAstVisitor
    {
        void Visit(Definition definition);
        void Visit(Program definition);
        void Visit(Expr expr);
        void Visit(BinaryOperator expr);
        void Visit(UnaryOperator expr);
        void Visit(Identifier expr);
    }
}
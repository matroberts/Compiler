using System;

namespace KleinCompiler
{
    public interface IAstVisitor
    {
        void Visit(Definition node);
        void Visit(Program node);
        void Visit(Expr node);
        void Visit(BinaryOperator node);
        void Visit(UnaryOperator node);
        void Visit(Identifier node);
        void Visit(KleinType node);
    }
}
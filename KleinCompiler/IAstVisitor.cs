using System;

namespace KleinCompiler
{
    public interface IAstVisitor
    {
        void Visit(Program node);
        void Visit(Definition node);
        void Visit(BinaryOperator node);
        void Visit(UnaryOperator node);
        void Visit(Formal node);
        void Visit(Identifier node);
        void Visit(KleinType node);
        void Visit(BooleanLiteral node);
        void Visit(IntegerLiteral node);
    }
}
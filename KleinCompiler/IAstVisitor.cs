using System;

namespace KleinCompiler
{
    public interface IAstVisitor
    {
        void Visit(Program node);
        void Visit(Definition node);
        void Visit(Formal node);
        void Visit(KleinType node);
        void Visit(Body node);
        void Visit(Print node);
        void Visit(IfThenElse node);
        void Visit(BinaryOperator node);
        void Visit(UnaryOperator node);
        void Visit(Identifier node);
        void Visit(BooleanLiteral node);
        void Visit(IntegerLiteral node);
        void Visit(FunctionCall node);
        void Visit(Actual node);
    }
}
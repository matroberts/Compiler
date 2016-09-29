namespace KleinCompiler.AbstractSyntaxTree
{
    public interface IAstVisitor
    {
        void Visit(Program node);
        void Visit(Definition node);
        void Visit(Formal node);
        void Visit(BooleanTypeDeclaration node);
        void Visit(IntegerTypeDeclaration node);
        void Visit(Body node);
        void Visit(Print node);
        void Visit(IfThenElse node);
        void Visit(LessThanOperator node);
        void Visit(EqualsOperator node);
        void Visit(OrOperator node);
        void Visit(PlusOperator node);
        void Visit(MinusOperator node);
        void Visit(AndOperator node);
        void Visit(TimesOperator node);
        void Visit(DivideOperator node);
        void Visit(NotOperator node);
        void Visit(NegateOperator node);
        void Visit(Identifier node);
        void Visit(BooleanLiteral node);
        void Visit(IntegerLiteral node);
        void Visit(FunctionCall node);
        void Visit(Actual node);
    }
}
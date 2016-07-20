using System.Threading;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class TypeDeclaration : Ast
    {
        protected TypeDeclaration(int position, PrimitiveType primitiveType) : base(position)
        {
            PrimitiveType = primitiveType;
        }

        public PrimitiveType PrimitiveType { get; }
    }

    public class BooleanTypeDeclaration : TypeDeclaration
    {
        public BooleanTypeDeclaration(int position) : base(position, new BooleanType())
        {

        }
        public override bool Equals(object obj)
        {
            var node = obj as BooleanTypeDeclaration;
            return node != null;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }

    public class IntegerTypeDeclaration : TypeDeclaration
    {
        public IntegerTypeDeclaration(int position) : base(position, new IntegerType())
        {
        }
        public override bool Equals(object obj)
        {
            var node = obj as IntegerTypeDeclaration;
            return node != null;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }
    }
}
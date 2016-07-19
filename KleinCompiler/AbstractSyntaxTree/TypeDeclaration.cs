using System.Threading;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class TypeDeclaration : Ast
    {
        protected TypeDeclaration(int position)
        {
            Position = position;
        }
        public int Position { get; }

        public abstract PrimitiveType ToKType();
    }

    public class BooleanTypeDeclaration : TypeDeclaration
    {
        public BooleanTypeDeclaration(int position) : base(position)
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

        public override PrimitiveType ToKType()
        {
            return new BooleanType();
        }
    }

    public class IntegerTypeDeclaration : TypeDeclaration
    {
        public IntegerTypeDeclaration(int position) : base(position)
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

        public override PrimitiveType ToKType()
        {
            return new IntegerType();
        }
    }
}
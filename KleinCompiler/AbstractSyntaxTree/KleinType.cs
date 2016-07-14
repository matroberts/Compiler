using System.Threading;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class KleinType : Ast
    {
        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }

        public abstract PrimitiveType ToType2();
    }

    public class BooleanTypeDeclaration : KleinType
    {
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

        public override PrimitiveType ToType2()
        {
            return new BooleanType();
        }
    }

    public class IntegerTypeDeclaration : KleinType
    {
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

        public override PrimitiveType ToType2()
        {
            return new IntegerType();
        }
    }
}
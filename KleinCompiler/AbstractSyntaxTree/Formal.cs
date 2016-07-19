namespace KleinCompiler.AbstractSyntaxTree
{
    public class Formal : Ast
    {
        public Formal(Identifier identifier, TypeDeclaration typeDeclaration)
        {
            TypeDeclaration = typeDeclaration;
            Name = identifier.Value;
            Position = identifier.Position;
        }

        public TypeDeclaration TypeDeclaration { get; }
        public string Name { get; }
        public int Position { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Formal;
            if (node == null)
                return false;

            if (this.TypeDeclaration.Equals(node.TypeDeclaration) == false)
                return false;
            if (this.Name.Equals(node.Name) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}({Name})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override TypeValidationResult CheckType()
        {
            throw new System.NotImplementedException();
        }

        public PrimitiveType ToKType()
        {
            return TypeDeclaration.ToKType();
        }
    }
}
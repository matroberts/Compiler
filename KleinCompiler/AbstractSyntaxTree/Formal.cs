namespace KleinCompiler.AbstractSyntaxTree
{
    public class Formal : Ast
    {
        public Formal(Identifier identifier, KleinType kleinType)
        {
            KleinType = kleinType;
            Identifier = identifier;
            Name = identifier.Value;
        }

        public KleinType KleinType { get; }
        public Identifier Identifier { get; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Formal;
            if (node == null)
                return false;

            if (this.KleinType.Equals(node.KleinType) == false)
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
    }
}
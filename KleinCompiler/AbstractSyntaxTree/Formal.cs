namespace KleinCompiler.AbstractSyntaxTree
{
    public class Formal : Ast
    {
        public Formal(Identifier identifier, KleinType kleinType)
        {
            KleinType = kleinType;
            Identifier = identifier;
        }

        public KleinType KleinType { get; }
        public Identifier Identifier { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Formal;
            if (node == null)
                return false;

            if (this.KleinType.Equals(node.KleinType) == false)
                return false;
            if (this.Identifier.Equals(node.Identifier) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }
        public override string ToString()
        {
            return $"{GetType().Name}({Identifier.Value})";
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
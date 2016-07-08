namespace KleinCompiler.AbstractSyntaxTree
{
    public class Identifier : Expr
    {
        public Identifier(string value)
        {
            Value = value;
        }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Identifier;
            if (node == null)
                return false;

            if (this.Value.Equals(node.Value) == false)
                return false;

            return true;
        }

        public override void Accept(IAstVisitor visior)
        {
            visior.Visit(this);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Value})";
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
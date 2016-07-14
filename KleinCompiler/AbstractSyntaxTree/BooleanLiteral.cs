namespace KleinCompiler.AbstractSyntaxTree
{
    public class BooleanLiteral : Expr
    {
        public BooleanLiteral(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as BooleanLiteral;
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
            Type = new BooleanType();
            return TypeValidationResult.Valid(Type);
        }
    }
}
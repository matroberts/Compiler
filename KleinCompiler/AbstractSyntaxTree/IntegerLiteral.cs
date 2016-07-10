namespace KleinCompiler.AbstractSyntaxTree
{
    public class IntegerLiteral : Expr
    {
        public IntegerLiteral(string value)
        {
            Value = uint.Parse(value);
        }

        public uint Value { get; }

        public override bool Equals(object obj)
        {
            var node = obj as IntegerLiteral;
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
            Type = KType.Integer;

            return TypeValidationResult.Valid(Type);
        }
    }
}
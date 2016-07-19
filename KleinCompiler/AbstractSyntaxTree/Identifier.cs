namespace KleinCompiler.AbstractSyntaxTree
{
    public class Identifier : Expr
    {
        public Identifier(int position, string value) : base(position)
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
            if(SymbolTable.FormalExists(Value)==false)
                return TypeValidationResult.Invalid($"Use of undeclared identifier {Value} in function {SymbolTable.CurrentFunction}");

            Type = SymbolTable.FormalType(Value);
            return TypeValidationResult.Valid(Type);
        }
    }
}
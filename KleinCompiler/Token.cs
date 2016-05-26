using System;

namespace KleinCompiler
{
    public class Token
    {
        public Token(SymbolName name, string value, int position)
        {
            Name = name;
            Value = value;
            Position = position;
        }

        public SymbolName Name { get; }
        public string Value { get; }
        public int Position { get; }

        public int Length => Value.Length;

        public override bool Equals(object obj)
        {
            var token = obj as Token;
            if (token == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return Name == token.Name && Value == token.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Name.ToString().PadRight(25)} : '{Value.Replace("\r", "").Replace("\n", "").TruncateWithElipses(25)}'";
        }
    }

    public class ErrorToken : Token
    {
        public ErrorToken(string value, int position, string errorMessage) : base(SymbolName.Error, value, position)
        {
            Message = errorMessage;
        }

        public string Message { get; }

        public override bool Equals(object obj)
        {
            var token = obj as ErrorToken;
            if (token == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return Value == token.Value && Message == token.Message;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + $" '{Message}'";
        }
    }
}
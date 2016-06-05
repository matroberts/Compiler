using System;

namespace KleinCompiler
{
    public class Token
    {
        public Token(Symbol symbol, string value, int position)
        {
            Symbol = symbol;
            Value = value;
            Position = position;
        }

        public Symbol Symbol { get; }
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

            return Symbol == token.Symbol && Value == token.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Symbol.ToString().PadRight(25)} : '{Value.Replace("\r", "").Replace("\n", "").TruncateWithElipses(25)}'";
        }
    }

    public class ErrorToken : Token
    {
        public ErrorToken(string value, int position, string errorMessage) : base(Symbol.Error, value, position)
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
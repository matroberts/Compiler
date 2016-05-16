using System;

namespace KleinCompiler
{
    public class Token
    {
        public Token(string value, int position)
        {
            Value = value;
            Position = position;
        }

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

            return Value == token.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} : '{Value}'";
        }
    }

    public class IdentifierToken : Token
    {
        public IdentifierToken(string value, int position) : base(value, position) { }
    }

    public class KeywordToken : Token
    {
        public KeywordToken(string value, int position) : base(value, position) { }
    }

    public class LineCommentToken : Token
    {
        public LineCommentToken(string value, int position) : base(value, position) { }
    }

    public class BlockCommentToken : Token
    {
        public BlockCommentToken(string value, int position) : base(value, position) { }
    }

    public class IntegerLiteralToken : Token
    {
        public IntegerLiteralToken(string value, int position) : base(value, position) { }
    }

    public class ErrorToken : Token
    {
        public ErrorToken(string value, int position, string errorMessage) : base(value, position)
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
            return $"{this.GetType().Name} : '{Message}'";
        }
    }
}
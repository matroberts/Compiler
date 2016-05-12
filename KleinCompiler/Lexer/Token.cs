using System;

namespace KleinCompiler
{
    public class Token
    {
        public Token(string value)
        {
            Value = value;
        }
        public string Value { get; }

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
        public IdentifierToken(string value) : base(value) { }
    }

    public class KeywordToken : Token
    {
        public KeywordToken(string value) : base(value) { }
    }

    public class LineCommentToken : Token
    {
        public LineCommentToken(string value) : base(value) { }
    }

    public class BlockCommentToken : Token
    {
        public BlockCommentToken(string value) : base(value) { }
    }

    public class IntegerLiteralToken : Token
    {
        public IntegerLiteralToken(string value) : base(value) { }
    }
}
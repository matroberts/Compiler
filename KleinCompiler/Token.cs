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

    public class IdentifierToken : Token
    {
        public IdentifierToken(string value, int position) : base(SymbolName.Identifier, value, position) { }
    }

    public class KeywordToken : Token
    {
        public KeywordToken(SymbolName name, string value, int position) : base(name, value, position) { }
    }

    public class LineCommentToken : Token
    {
        public LineCommentToken(string value, int position) : base(SymbolName.LineComment, value, position) { }
    }

    public class BlockCommentToken : Token
    {
        public BlockCommentToken(string value, int position) : base(SymbolName.BlockComment, value, position) { }
    }

    public class IntegerLiteralToken : Token
    {
        public IntegerLiteralToken(string value, int position) : base(SymbolName.IntegerLiteral, value, position) { }
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
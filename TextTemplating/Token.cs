using System.Collections.Generic;
using System.Linq;

namespace TextTemplating
{
    public class TokenList : List<Token>
    {
        public override string ToString()
        {
            return string.Join(", ", this.Select(t => t.ToString()));
        }
    }

    public class Token
    {
        public string Value { get; protected set; }

        public Token Append(int ch)
        {
            Value = Value + (char)ch;
            return this;
        }
    }

    public class LiteralToken : Token
    {
        public override string ToString()
        {
            return $"L:'{Value}'";
        }
    }

    public class VariableToken : Token
    {
        public override string ToString()
        {
            return $"V:'{Value}'";
        }
    }
}
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

        public void Append(string more)
        {
            Value = Value + more;
        }
    }

    public class LiteralToken : Token
    {
        public LiteralToken(string value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return $"L:'{Value}'";
        }
    }

    public class VariableToken : Token
    {
        public VariableToken(string value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return $"V:'{Value}'";
        }
    }
}
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

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
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public string Value => _stringBuilder.ToString();

        public Token Append(int ch)
        {
            _stringBuilder.Append((char) ch);
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

        public string Name => Value.Substring(2, Value.Length-3);
    }
}
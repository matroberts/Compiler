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

        public new TokenList Add(Token token)
        {
            base.Add(token);
            return this;
        }
    }

    public class Token
    {
        protected readonly StringBuilder _stringBuilder = new StringBuilder();

        public string Value => _stringBuilder.ToString();

        public Token Append(int ch)
        {
            _stringBuilder.Append((char) ch);
            return this;
        }

        public virtual bool IsClosed()
        {
            return true;
        }

        public LiteralToken ToLiteral()
        {
            return new LiteralToken(Value);
        }
    }

    public class LiteralToken : Token
    {
        public LiteralToken()
        {
        }
        public LiteralToken(string value)
        {
            _stringBuilder.Append(value);
        }
        public override string ToString()
        {
            return $"L:'{Value}'";
        }
    }

    public class TagToken : Token
    {
        public string Name => Value.Substring(2, Value.Length - 3).Trim();

        public override bool IsClosed()
        {
            return Value.EndsWith("}");
        }
    }

    public class VariableToken : TagToken
    {
        public VariableToken()
        {
        }
        public VariableToken(string value)
        {
            _stringBuilder.Append(value);
        }
        public override string ToString()
        {
            return $"V:'{Value}'";
        }
    }

    public class OpenToken : TagToken
    {
        public OpenToken()
        {
        }
        public OpenToken(string value)
        {
            _stringBuilder.Append(value);
        }
        public override string ToString()
        {
            return $"O:'{Value}'";
        }
    }

    public class CloseToken : TagToken
    {
        public CloseToken()
        {
        }
        public CloseToken(string value)
        {
            _stringBuilder.Append(value);
        }
        public override string ToString()
        {
            return $"C:'{Value}'";
        }
    }
}
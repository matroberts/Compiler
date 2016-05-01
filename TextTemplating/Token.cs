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

        public virtual bool IsValid(out string errorMessage)
        {
            errorMessage = null;
            return true;
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

        public string Name
        {
            get { return Value.TrimStart('{').TrimEnd('}').Trim(); }
        }

        public override bool IsValid(out string errorMessage)
        {
            if (!Value.EndsWith("}"))
            {
                errorMessage = $"Tempate variable not terminated with }}, problem text near '{Value.TruncateWithElipses(25)}'";
                return false;
            }
            errorMessage = null;
            return true;
        }
    }
}
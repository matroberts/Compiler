using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextTemplating
{
    public class Parser
    {
        public static Token GetToken(StringReader reader)
        {
            if (reader.Peek() == '{')
            {
                var ch = (char) reader.Read();
                if (reader.Peek() == '{')
                {
                    return new VariableToken().Append(ch).Append(reader.Read());
                }
                else
                {
                    return new LiteralToken().Append(ch);
                }
            }
            else
            {
                return new LiteralToken();
            }
        }

        public static TokenList Parse(string template)
        {
            var tokens = new TokenList();
            using (var reader = new StringReader(template))
            {
                Token token = GetToken(reader);
                int ch;
                while ((ch = reader.Read()) != -1)
                {
                    token.Append(ch);
                    if (reader.Peek() == '{')
                    {
                        tokens.Add(token);
                        token = GetToken(reader);
                    }
                    else if (reader.Peek() == '}')
                    {
                        token.Append(reader.Read());
                        tokens.Add(token);
                        token = GetToken(reader);
                    }
                }

                tokens.Add(token);
            }
            return tokens;
        }

    }
}
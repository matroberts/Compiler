using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextTemplating
{
    public class Parser
    {
        public static TokenList Parse(string template)
        {
            using (var reader = new StringReader(template))
            {
                return Parse(reader);
            }
        }

        public static TokenList Parse(StringReader reader)
        {
            var tokens = new TokenList();

            var token = GetToken(reader);
            tokens.Add(token);

            int ch;
            while ((ch = reader.Read()) != -1)
            {
                token.Append(ch);
                if (reader.Peek() == '{' && token is LiteralToken)
                {
                    token = GetToken(reader);
                    tokens.Add(token);
                }
                else if (reader.Peek() == '}' && token is TagToken)
                {
                    token.Append(reader.Read());
                    token = GetToken(reader);
                    tokens.Add(token);
                }
            }
            return tokens;
        }

        private static Token GetToken(StringReader reader)
        {
            if (reader.Peek() == '{')
            {
                var ch = (char)reader.Read();
                if (reader.Peek() == '{')
                {
                    return new VariableToken().Append(ch).Append(reader.Read());
                }
                else if (reader.Peek() == '?')
                {
                    return new OpenToken().Append(ch).Append(reader.Read());
                }
                else if (reader.Peek() == '!')
                {
                    return new CloseToken().Append(ch).Append(reader.Read());
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
    }
}
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
                    reader.Read(); // throw away the {'s
                    return new VariableToken("");
                }
                else
                {
                    return new LiteralToken("{");
                }
            }
            else
            {
                return new LiteralToken("");
            }
        }

        public static void CompleteToken(Token token, StringBuilder sb, StringReader reader)
        {
            if (token is VariableToken)
            {
                reader.Read(); //skip past closing }
            }
            token.Append(sb.ToString());
            sb.Clear();
        }

        public static TokenList Parse(string template)
        {
            var tokens = new TokenList();
            using (var reader = new StringReader(template))
            {
                Token token = GetToken(reader);
                var sb = new StringBuilder();
                int ch;
                while ((ch = reader.Read()) != -1)
                {
                    sb.Append((char)ch);

                    if (reader.Peek() == '{')
                    {
                        token.Append(sb.ToString());
                        tokens.Add(token);
                        sb.Clear();

                        token = GetToken(reader);
                    }
                    else if (reader.Peek() == '}')
                    {
                        CompleteToken(token, sb, reader);
                        tokens.Add(token);
                        token = GetToken(reader);
                    }
                }

                if (token.Value.Length > 0 || sb.Length > 0)
                {
                    token.Append(sb.ToString());
                    tokens.Add(token);
                }
            }
            return tokens;
        }

    }
}
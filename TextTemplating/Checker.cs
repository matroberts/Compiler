using System.Collections.Generic;

namespace TextTemplating
{
    public class Checker
    {
        public void Check(TokenList tokens, Dictionary<string, string> parameters, Errors errors)
        {
            for (int i=0; i<tokens.Count; i++)
            {
                var token = tokens[i] as TagToken;

                if(token==null)
                    continue;

                if (!token.IsClosed())
                {
                    errors.Add($"Tempate tag not terminated with }}, problem text near '{token.Value.TruncateWithElipses(25)}'");
                    tokens[i] = token.ToLiteral();
                    continue;
                }

                if (!parameters.ContainsKey(token.Name))
                {
                    errors.Add($"Missing dictionary parameter '{token.Name.TruncateWithElipses(25)}'");
                    tokens[i] = token.ToLiteral();
                    continue;
                }
            }
        }
    }
}
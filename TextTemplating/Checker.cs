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

                // tags should be correctly formed
                if (!token.IsClosed())
                {
                    errors.Add($"Tempate tag not terminated with }}, problem text near '{token.Value.TruncateWithElipses(25)}'");
                    tokens[i] = token.ToLiteral();
                    continue;
                }

                // parameters should exist for the tags
                if (!parameters.ContainsKey(token.Name))
                {
                    errors.Add($"Missing dictionary parameter '{token.Name.TruncateWithElipses(25)}'");
                    tokens[i] = token.ToLiteral();
                    continue;
                }

                // open tags only accept the parameters "true"/"false"
            }

            var stack = new Stack<OpenToken>();
            foreach (var token in tokens)
            {
                if (token is OpenToken)
                {
                    stack.Push(token as OpenToken);
                }

                if (token is CloseToken)
                {
                    var close = token as CloseToken;
                    if (close.Name == stack.Peek().Name)
                    {
                        stack.Pop();
                    }
                    else
                    {
                        errors.Add($"Boolean tag '{stack.Peek().Name}' should be closed before tag '{close.Name}' is closed.");
                    }
                }
            }

            foreach (var open in stack)
            {
                errors.Add($"Boolean tag '{open.Name}' is not closed");
            }
        }
    }
}
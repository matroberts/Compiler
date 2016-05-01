using System;
using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class Generator
    {
        public string Generate(TokenList tokens, Dictionary<string, string> parameters, Errors errors)
        {
            var output = new StringBuilder();
            string offTag = null;

            foreach (var token in tokens)
            {
                if (token is OpenToken)
                {
                    var open = token as OpenToken;
                    if (parameters[open.Name] == "false" && offTag == null)
                    {
                        offTag = open.Name;
                    }
                }
                else if (token is CloseToken)
                {
                    var close = token as CloseToken;
                    if (offTag == close.Name)
                    {
                        offTag = null;
                    }
                }

                if(offTag != null)
                    continue;

                if (token is LiteralToken)
                {
                    output.Append(token.Value);
                }
                else if (token is VariableToken)
                {
                    var variable = token as VariableToken;
                    output.Append(parameters[variable.Name]);
                }
            }
            return output.ToString();
        }
    }
}
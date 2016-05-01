using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class Generator
    {
        public string Generate(TokenList tokens, Dictionary<string, string> parameters, Errors errors)
        {
            var output = new StringBuilder();

            foreach (var token in tokens)
            {
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
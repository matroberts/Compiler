using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class Generator
    {
        public string Generate(TokenList tokens, Dictionary<string, string> dictionary, Errors errors)
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
                    string name;
                    if (dictionary.TryGetValue(variable.Name, out name))
                    {
                        output.Append(name);
                    }
                    else
                    {
                        errors.Add($"Missing dictionary parameter '{variable.Name.TruncateWithElipses(25)}'");
                        output.Append(variable.Value);
                    }
                }
            }
            return output.ToString();
        }
    }
}
using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class TemplateCompiler
    {
        public static string Compile(string template, Dictionary<string, string> dictionary, Errors errors)
        {
            var tokens = Parser.Parse(template);
            var output = new StringBuilder();
            foreach (var token in tokens)
            {
                string errorMessage;
                if (!token.IsValid(out errorMessage))
                {
                    errors.Add(errorMessage);
                }

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
using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class TemplateCompiler
    {
        public static string Compile(string template, Dictionary<string, string> dictionary, CompileErrors errors)
        {
            var tokens = Parser.Parse(template);
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
                        output.Append(dictionary[variable.Name]);
                    }
                    else
                    {
                        output.Append($"!!!MISSING TEMPLATE PARAMETER '{variable.Name}'!!!");
                    }
                }
            }
            return output.ToString();
        }
    }
}
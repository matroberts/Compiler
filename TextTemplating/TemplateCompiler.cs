using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class TemplateCompiler
    {
        public Errors Errors { get; } = new Errors();

        public string Compile(string template, Dictionary<string, string> parameters)
        {
            var tokens = Parser.Parse(template);
            new Checker().Check(tokens, parameters, Errors);
            return new Generator().Generate(tokens, parameters);
        }
    }
}
using System.Collections.Generic;
using System.Text;

namespace TextTemplating
{
    public class TemplateCompiler
    {
        public Errors Errors { get; } = new Errors();

        public string Compile(string template, Dictionary<string, string> dictionary)
        {
            var tokens = Parser.Parse(template);
            new Checker().Check(tokens, Errors);
            return new Generator().Generate(tokens, dictionary, Errors);
        }
    }
}
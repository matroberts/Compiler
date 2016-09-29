using System.Runtime.CompilerServices;
using KleinCompiler.AbstractSyntaxTree;
using KleinCompiler.FrontEndCode;

namespace KleinCompiler
{
    public class FrontEnd
    {
        public Error Error { get; private set; }
        public Program Compile(string input)
        {
            Error.FilePositionCalculator = new FilePositionCalculator(input);

            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            if (program == null)
            {
                Error = parser.Error;
                return null;
            }

            var result = program.CheckType();
            if (result.HasError)
            {
                Error = Error.CreateSemanticError(result);
                return null;
            }

            return program;
        }
    }
}
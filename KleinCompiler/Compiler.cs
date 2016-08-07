using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler
{
    public class Compiler
    {
        public Program Program { get; private set; }

        public Error Compile(string input)
        {
            Error.FilePositionCalculator = new FilePositionCalculator(input);

            var parser = new Parser();
            Program = (Program)parser.Parse(new Tokenizer(input));
            if(Program == null)
                return parser.Error;

            var result = Program.CheckType();
            if(result.HasError)
                return Error.CreateSemanticError(result);

            return Error.CreateNoError();
        }
    }
}
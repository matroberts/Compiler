using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler
{
    public class Compiler
    {
        public Error Compile(string input)
        {
            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input));
            if(program==null)
                return parser.Error;

            var result = program.CheckType();
            if(result.HasError)
                return Error.CreateSemanticError(result);

            return Error.CreateNoError();
        }
    }
}
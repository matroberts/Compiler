using System.Runtime.CompilerServices;
using KleinCompiler.AbstractSyntaxTree;
using KleinCompiler.FrontEndCode;

namespace KleinCompiler
{
    public class FrontEnd
    {
        public ErrorRecord ErrorRecord { get; private set; }
        
        public Program Compile(string input)
        {
            ErrorRecord = new ErrorRecord(input);

            var parser = new Parser();
            var program = (Program)parser.Parse(new Tokenizer(input), ErrorRecord);
            if (program == null)
            {
                return null;
            }

            var result = program.CheckType();
            if (result.HasError)
            {
                ErrorRecord.AddSemanticError(result);
                return null;
            }

            return program;
        }
    }
}
using System;
using System.Configuration;
using System.IO;
using System.Management.Automation;
using KleinCompiler;

namespace KleinCmdlets
{
    [Cmdlet(VerbsDiagnostic.Test, "KleinProgram")]
    [OutputType(typeof(bool))]
    public class ParseKleinProgram : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Path and name of the klein file to parse")]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            var input = File.ReadAllText(Path);
            var parser = new Parser(new ParsingTable());
            var isValid = parser.Parse(new Tokenizer(input));

            if (isValid == false)
            {
                var calculator = new FilePositionCalculator(input);
                var filePosition = calculator.FilePosition(parser.Error.Token.Position);
                var exceptionMessage = $"{parser.Error.Message}\r\n at {Path} {filePosition}";
                WriteError(new ErrorRecord(new Exception(exceptionMessage), "InvalidProgram", ErrorCategory.InvalidData, null));
            }
            WriteObject(isValid);
        }
    }
}
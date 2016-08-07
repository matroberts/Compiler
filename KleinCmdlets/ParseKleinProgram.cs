using System;
using System.Configuration;
using System.IO;
using System.Management.Automation;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCmdlets
{
    [Cmdlet("Parse", "KleinProgram")]
    [OutputType(typeof(Program))]
    public class ParseKleinProgram : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Path and name of the klein file to parse")]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            var input = File.ReadAllText(Path);
            var complier = new Compiler();
            var error = complier.Compile(input);

            if (error.ErrorType != Error.ErrorTypeEnum.No)
            {
                var exceptionMessage = $"{Path}{error}";
                throw new Exception(exceptionMessage);
            }
            WriteObject(complier.Program);
        }
    }
}
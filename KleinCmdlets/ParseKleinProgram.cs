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
            var frontEnd = new FrontEnd();
            var program = frontEnd.Compile(File.ReadAllText(Path));

            if (program == null)
            {
                var exceptionMessage = $"{Path}{frontEnd.Error}";
                throw new Exception(exceptionMessage);
            }
            WriteObject(program);
        }
    }
}
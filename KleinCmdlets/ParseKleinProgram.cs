﻿using System;
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
            var parser = new Parser();
            var ast = parser.Parse(new Tokenizer(input));

            if (ast == null)
            {
                var exceptionMessage = $"{parser.Error}";
                if (parser.Error.Token != null)
                {
                    var filePosition = new FilePositionCalculator(input).FilePosition(parser.Error.Token.Position);
                    exceptionMessage += $"\r\n at {Path} {filePosition}";
                }
                WriteError(new ErrorRecord(new Exception(exceptionMessage), "InvalidProgram", ErrorCategory.InvalidData, null));
            }
            WriteObject(ast);
        }
    }
}
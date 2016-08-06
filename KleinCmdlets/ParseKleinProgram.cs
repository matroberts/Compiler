﻿using System;
using System.Configuration;
using System.IO;
using System.Management.Automation;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCmdlets
{
    [Cmdlet("Parse", "KleinProgram")]
    [OutputType(typeof(Ast))]
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
                var filePosition = new FilePositionCalculator(input).FilePosition(parser.Error.Position);
                exceptionMessage += $"\r\n at {Path} {filePosition}";
                throw new Exception(exceptionMessage);
            }
            WriteObject(ast);
        }
    }
}
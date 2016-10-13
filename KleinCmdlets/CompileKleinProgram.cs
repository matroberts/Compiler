using System;
using System.IO;
using System.Management.Automation;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;
using KleinCompiler.BackEndCode;

namespace KleinCmdlets
{
    [Cmdlet("Compile", "KleinProgram")]
    [OutputType(typeof(string))]
    public class CompileKleinProgram : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "AST of a parsed klein program", ValueFromPipeline = true)]
        public Program Ast { get; set; }

        protected override void ProcessRecord()
        {
            var tacs = new ThreeAddressCodeFactory().Generate(Ast);
            var output = new CodeGenerator().Generate(tacs);
            WriteObject(output);
        }
    }
}
using System.Management.Automation;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCmdlets
{
    [Cmdlet(VerbsCommon.Format, "PrettyAst")]
    [OutputType(typeof(string))]
    public class FormatPrettyAst : Cmdlet
    {
        [Parameter(Position = 0, 
                   Mandatory = true, 
                   HelpMessage = "Klein Ast that you want to pretty print", 
                   ValueFromPipeline = true, 
                   ValueFromPipelineByPropertyName = true)]
        public Ast Ast { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(PrettyPrinter.ToString(Ast));
        }
    }
}
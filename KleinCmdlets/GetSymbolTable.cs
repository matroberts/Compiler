using System.Management.Automation;
using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCmdlets
{
    [Cmdlet(VerbsCommon.Get, "SymbolTable")]
    [OutputType(typeof(SymbolTableEntry))]
    public class GetSymbolTable : Cmdlet
    {
        [Parameter(Position = 0,
           Mandatory = true,
           HelpMessage = "Klein Ast that you want to pretty print",
           ValueFromPipeline = true,
           ValueFromPipelineByPropertyName = true)]
        public Ast Ast { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var functionInfo in Ast.SymbolTable.FunctionInfos)
            {
                WriteObject(new SymbolTableEntry()
                {
                    Callers = string.Join(", ", functionInfo.Callers),
                    Name = functionInfo.Name,
                    TypeInfo = functionInfo.FunctionType.ToString()
                });
            }
        }
    }

    public class SymbolTableEntry
    {
        public string Name { get; set; }
        public string TypeInfo { get; set; }
        public string Callers { get; set; }
    }
}
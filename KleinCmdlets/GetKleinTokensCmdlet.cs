using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using KleinCompiler;
using KleinCompiler.FrontEndCode;

namespace KleinCmdlets
{
    [Cmdlet(VerbsCommon.Get, "KleinTokens")]
    [OutputType(typeof(Token))]
    public class GetKleinTokensCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Path and name of the klein file to tokenize")]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            var input = File.ReadAllText(Path);
            var tokenizer = new Tokenizer(input);
            Token token = null;
            while ((token = tokenizer.GetNextToken()).Symbol != Symbol.End)
            {
                WriteObject(token);
            }
        }
    }
}

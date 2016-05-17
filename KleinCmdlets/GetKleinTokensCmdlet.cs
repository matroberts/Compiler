using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using KleinCompiler;

namespace KleinCmdlets
{
    [Cmdlet(VerbsCommon.Get, "KleinTokens")]
    [OutputType(typeof(Token))]
    public class GetKleinTokensCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Path and name of the klein file to tokenize")]
        public string Path { get; set; }
    }
}

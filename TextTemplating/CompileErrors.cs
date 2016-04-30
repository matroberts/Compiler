using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TextTemplating
{
    public class CompileErrors
    {
        public CompileErrors()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
        public bool HasErrors => Errors.Any();
    }
}
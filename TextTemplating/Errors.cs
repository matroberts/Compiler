using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TextTemplating
{
    public class Errors
    {
        public List<string> Messages { get; } = new List<string>();
        public bool HasErrors => Messages.Any();
        public Errors Add(string errorMessage)
        {
            Messages.Add(errorMessage);
            return this;
        }
    }
}
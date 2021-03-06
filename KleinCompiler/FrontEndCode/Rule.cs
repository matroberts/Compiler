using System.Collections.Generic;
using System.Linq;

namespace KleinCompiler.FrontEndCode
{
    public class Rule
    {
        public List<Symbol> Symbols { get; } = new List<Symbol>();
        public string Name { get; }
        public Rule(string name, params Symbol[] symbols)
        {
            Name = name;
            Symbols.AddRange(symbols);
        }

        public IEnumerable<Symbol> Reverse => (Symbols as IEnumerable<Symbol>).Reverse();
    }
}
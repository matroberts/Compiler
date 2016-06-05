using System.Collections.Generic;
using System.Linq;

namespace KleinCompiler
{
    public class Rule
    {
        public string Name { get; }
        public List<Symbol> Symbols { get; } = new List<Symbol>();
        public Rule(string name, params Symbol[] symbols)
        {
            Name = name;
            Symbols.AddRange(symbols);
        }

        public IEnumerable<Symbol> Reverse => (Symbols as IEnumerable<Symbol>).Reverse();
    }
}
using System;

namespace KleinCompiler
{
    public class ParsingTable
    {
        private readonly Rule[,] table;
        public ParsingTable()
        {
            int numberSymbols = Enum.GetNames(typeof(Symbol)).Length;
            table = new Rule[numberSymbols, numberSymbols];
        }

        public void AddRule(Rule rule, Symbol nonTerminal, params Symbol[] terminals)
        {
            foreach (var terminal in terminals)
            {
                var existingRule = table[(int) nonTerminal, (int) terminal];
                if (existingRule != null)
                    throw new ArgumentException($"Ambiguity detected in parser table [{nonTerminal}, {terminal}] rule '{existingRule.Name}' conflicts with '{rule.Name}'.");
                else
                    table[(int) nonTerminal, (int) terminal] = rule;
            }
        }

        public Rule this[Symbol symbol, Symbol token] => table[(int)symbol, (int)token];
    }
}
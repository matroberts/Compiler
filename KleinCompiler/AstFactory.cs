using System;

namespace KleinCompiler
{
    public  class AstFactory
    {
        public static Ast Create(Symbol symbol)
        {
            switch (symbol)
            {

                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }
    }
}
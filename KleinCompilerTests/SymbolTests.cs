using System;
using System.Linq;
using KleinCompiler;
using KleinCompiler.FrontEndCode;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class SymbolTests
    {
        [Test]
        public void AllTheValues_InTheSymbolEnum_ShouldHaveASymbolAttribute()
        {
            foreach (Symbol symbol in Enum.GetValues(typeof(Symbol)).Cast<Symbol>())
            {
                Assert.That(() => symbol.ToSymbolType(), Throws.Nothing);
            }
        }
    }
}
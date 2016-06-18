using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ParsingTableTests
    {
        [Test]
        public void TheParsingTable_ChecksForAmbiguousRules_OnConstruction()
        {
            Assert.That(()=> ParsingTableFactory.Create(), Throws.Nothing);
        }
    }
}
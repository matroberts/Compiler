using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class RuleTests
    {
        [Test]
        public void Reverse_ReturnsTheRulesSymbols_InReverseOrder()
        {
            var rule = new Rule("Name", Symbol.Program, Symbol.End);

            Assert.That(rule.Name, Is.EqualTo("Name"));
            Assert.That(rule.Symbols, Is.EqualTo(new [] { Symbol.Program, Symbol.End }));
            Assert.That(rule.Reverse, Is.EqualTo(new [] { Symbol.End, Symbol.Program }));
        }
    }
}
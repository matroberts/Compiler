using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Test()
        {
            var parser = new Parser(new ReducedParsingTable());

            var result = parser.Parse(new Tokenizer("main () : boolean"));

            Assert.That(result, Is.True);
        }
    }
}
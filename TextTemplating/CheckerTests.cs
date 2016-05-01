using NUnit.Framework;

namespace TextTemplating
{
    [TestFixture]
    public class CheckerTests
    {
        [Test]
        public void IfATag_IsNotClosedCorrectly_ItIsReplacedByALiteralToken()
        {
            var tokens = new TokenList().Add(new LiteralToken("begin")).Add(new VariableToken("{{notclosed")).Add(new LiteralToken("end"));
            var errors = new Errors();
            
            var checker = new Checker();
            checker.Check(tokens, errors);

            Assert.That(errors.HasErrors, Is.True);
            Assert.That(errors.Messages[0], Is.EqualTo("Tempate tag not terminated with }, problem text near '{{notclosed'"));
            Assert.That(tokens.ToString(), Is.EqualTo("L:'begin', L:'{{notclosed', L:'end'"));
        }

        [Test]
        public void IfATag_IsClosedCorrectly_NoErrorIsGenerated()
        {
            var tokens = new TokenList().Add(new LiteralToken("begin")).Add(new VariableToken("{{closed}")).Add(new LiteralToken("end"));
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, errors);

            Assert.That(errors.HasErrors, Is.False);
            Assert.That(tokens.ToString(), Is.EqualTo("L:'begin', V:'{{closed}', L:'end'"));
        }
    }
}
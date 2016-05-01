using System.Collections.Generic;
using NUnit.Framework;

namespace TextTemplating
{
    [TestFixture]
    public class CheckerTests
    {


        [Test]
        public void IfATag_IsNotClosedCorrectly_AnErrorIsGenerated_AndTagIsReplacedByALiteralToken()
        {
            var tokens = new TokenList().Add(new LiteralToken("begin")).Add(new VariableToken("{{notclosed")).Add(new LiteralToken("end"));
            var parameters = new Dictionary<string, string>();
            var errors = new Errors();
            
            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.Messages.Count, Is.EqualTo(1));
            Assert.That(errors.Messages[0], Is.EqualTo("Tempate tag not terminated with }, problem text near '{{notclosed'"));
            Assert.That(tokens.ToString(), Is.EqualTo("L:'begin', L:'{{notclosed', L:'end'"));
        }
        
        [Test]
        public void IfATagName_IsNotInTheParameterDictionary_AnErrorIsGenerated_AndTagIsReplacedByALiteralToken()
        {
            var tokens = new TokenList().Add(new VariableToken("{{notinparameters}"));
            var parameters = new Dictionary<string, string>() {["inparameters"] = "woot"};
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.Messages.Count, Is.EqualTo(1));
            Assert.That(errors.Messages[0], Is.EqualTo($"Missing dictionary parameter 'notinparameters'"));
            Assert.That(tokens.ToString(), Is.EqualTo("L:'{{notinparameters}'"));
        }

        [Test]
        public void IfATag_IsPassesAllTheChecks_NoErrorIsGenerated()
        {
            var tokens = new TokenList().Add(new LiteralToken("begin")).Add(new VariableToken("{{closed}")).Add(new LiteralToken("end"));
            var parameters = new Dictionary<string, string>() {["closed"] = "text"};
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.HasErrors, Is.False);
            Assert.That(tokens.ToString(), Is.EqualTo("L:'begin', V:'{{closed}', L:'end'"));
        }
    }
}
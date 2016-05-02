using System;
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

        [Test]
        public void ASingleOpenTag_ShouldBePairedWithACloseTags()
        {
            var tokens = new TokenList().Add(new OpenToken("{?a}")).Add(new CloseToken("{!a}"));
            var parameters = new Dictionary<string, string>() { ["a"] = "true" };
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.HasErrors, Is.False);
        }

        [Test]
        public void IfAnOpenTag_IsNotClosed_AnErrorisRaised()
        {
            var tokens = new TokenList().Add(new OpenToken("{?a}"));
            var parameters = new Dictionary<string, string>() { ["a"] = "true" };
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.Messages.Count, Is.EqualTo(1));
            Assert.That(errors.Messages[0], Is.EqualTo("Boolean tag 'a' is not closed"));
        }

        [Test]
        public void BooleanTags_CanNest()
        {
            var tokens = new TokenList().Add(new OpenToken("{?a}"))
                                        .Add(new OpenToken("{?b}"))
                                        .Add(new CloseToken("{!b}"))
                                        .Add(new CloseToken("{!a}"));
            var parameters = new Dictionary<string, string>() { ["a"] = "true", ["b"] = "true" };
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.HasErrors, Is.False);
        }

        [Test]
        public void NestedTags_MustBeClosedInTheCorrectOrder()
        {
            var tokens = new TokenList().Add(new OpenToken("{?a}"))
                                        .Add(new OpenToken("{?b}"))
                                        .Add(new CloseToken("{!a}"))
                                        .Add(new CloseToken("{!b}"));
            var parameters = new Dictionary<string, string>() { ["a"] = "true", ["b"] = "true" };
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.HasErrors, Is.True);
            Assert.That(errors.Messages[0], Is.EqualTo("Boolean tag 'b' should be closed before tag 'a' is closed."));
            Console.WriteLine(errors.Messages[1]);
        }

        [TestCase("true")]
        [TestCase("false")]
        public void OpenTags_AcceptParameters_true_or_false(string parameter)
        {
            var tokens = new TokenList().Add(new OpenToken("{?a}")).Add(new CloseToken("{!a}"));
            var parameters = new Dictionary<string, string>() { ["a"] = parameter };
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.HasErrors, Is.False);
        }

        [Test]
        public void OpenTags_DoNotAccept_OtherParameterValues()
        {
            var tokens = new TokenList().Add(new OpenToken("{?a}")).Add(new CloseToken("{!a}"));
            var parameters = new Dictionary<string, string>() { ["a"] = "other" };
            var errors = new Errors();

            var checker = new Checker();
            checker.Check(tokens, parameters, errors);

            Assert.That(errors.HasErrors, Is.True);
            Assert.That(errors.Messages[0], Is.EqualTo("Boolean tag 'a' supplied with dictionary parameter 'other'.  Only allowed values are 'true' or 'false'"));
        }
    }
}
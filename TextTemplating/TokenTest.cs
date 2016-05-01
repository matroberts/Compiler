using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;

namespace TextTemplating
{
    [TestFixture]
    public class TokenTest
    {
        [Test]
        public void TagToken_Name_StripsOpeningAndClosingCurlies()
        {
            var variable = new VariableToken("{{a}") as TagToken;

            Assert.That(variable.Name, Is.EqualTo("a"));
        }

        [Test]
        public void TagToken_Name_TrimsWhitespace()
        {
            var variable = new VariableToken("{{ a }") as TagToken;

            Assert.That(variable.Name, Is.EqualTo("a"));
        }
    }
}
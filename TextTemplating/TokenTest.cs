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
        public void VariableToken_Name_StripsOpeningAndClosingCurlies()
        {
            var variable = new VariableToken();
            variable.Append('{').Append('{').Append('a').Append('}');

            Assert.That(variable.Name, Is.EqualTo("a"));
        }

        [Test]
        public void VariableToken_Name_TrimsWhitespace()
        {
            var variable = new VariableToken();
            variable.Append('{').Append('{').Append(' ').Append('a').Append(' ').Append('}');

            Assert.That(variable.Name, Is.EqualTo("a"));
        }

        [Test]
        public void IsValid_ReturnsFalse_IfAVariableDoesNotEndWithCurley()
        {
            var variable = new VariableToken();
            variable.Append('{').Append('{').Append('a');

            string errorMessage;
            Assert.That(variable.IsValid(out errorMessage),  Is.False);
            Assert.That(errorMessage, Is.EqualTo("Tempate variable not terminated with }, problem text near '{{a'"));
        }
    }
}
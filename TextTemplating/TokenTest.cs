using System;
using System.Linq;
using NUnit.Framework;

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
    }
}
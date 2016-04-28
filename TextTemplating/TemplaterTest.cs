using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TextTemplating
{
    [TestFixture]
    public class TemplaterTest
    {
        [Test]
        public void Templater_ShouldFillIn_TemplateVariableValues()
        {
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            string result = Templater.Build(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, John."));
        }

        [Test]
        public void Templater_ShouldWriteAnErrorMessageIntoTheOutput_IfATemplteVariableIsMissing()
        {
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["WrongName"] = "John",
            };

            string result = Templater.Build(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, !!!MISSING TEMPLATE PARAMETER 'name'!!!."));
        }
    }


}
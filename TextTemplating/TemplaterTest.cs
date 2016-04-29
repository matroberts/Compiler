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

    // What about malformed tags like {{{{

    // What about nested variable tags like {{var1{{var2}}

    // What about leading and trailing space is variable name {{ var }

    // What about processing errors/warnings - indication of where happened....errors and warnings at beginning....and unprocessed tags just left in text?


}
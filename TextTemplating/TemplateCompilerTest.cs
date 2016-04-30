using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TextTemplating
{
    [TestFixture]
    public class TemplateCompilerTest
    {
        [Test]
        public void Templater_ShouldFillIn_TemplateVariableValues()
        {
            var compileErrors = new Errors();
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            string result = TemplateCompiler.Compile(template, templateParameters, compileErrors);

            Assert.That(result, Is.EqualTo("Hello, John."));
            Assert.That(compileErrors.HasErrors, Is.False);
        }

        [Test]
        public void IfATemplateVariableIsMissing_AnErrorShouldBeReturned_AndTheUnmodifiedTextWrittenIntoTheOutput()
        {
            var compileErrors = new Errors();
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["WrongName"] = "John",
            };

            string result = TemplateCompiler.Compile(template, templateParameters, compileErrors);

            Assert.That(result, Is.EqualTo("Hello, {{name}."));
            Assert.That(compileErrors.HasErrors, Is.True);
            Assert.That(compileErrors.Messages[0], Is.EqualTo("Template dictionary parameter 'name' missing"));
        }
    }

    // Check for malformed tags, i.e. no closing }
    // Strip whitespace from tag name

    // What about processing errors/warnings - indication of where happened....errors and warnings at beginning....and unprocessed tags just left in text?


}
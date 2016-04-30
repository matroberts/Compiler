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
            var compileErrors = new CompileErrors();
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
        public void Templater_ShouldWriteAnErrorMessageIntoTheOutput_IfATemplteVariableIsMissing()
        {
            var compileErrors = new CompileErrors();
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["WrongName"] = "John",
            };

            string result = TemplateCompiler.Compile(template, templateParameters, compileErrors);

            Assert.That(result, Is.EqualTo("Hello, !!!MISSING TEMPLATE PARAMETER 'name'!!!."));
            Assert.That(compileErrors.HasErrors, Is.False);
        }
    }

    // Check for malformed tags, i.e. no closing }
    // Strip whitespace from tag name
    // Report errors seperatly from output

    // What about processing errors/warnings - indication of where happened....errors and warnings at beginning....and unprocessed tags just left in text?


}
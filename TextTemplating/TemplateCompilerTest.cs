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
            Assert.That(compileErrors.Messages[0], Is.EqualTo("Missing dictionary parameter 'name'"));
        }

        [Test]
        public void IfATemplateVariableIsMissingAClosingCurley_AndErrorShouldBeRetured_AndTheUnmodifiedTextWrittenIntoTheOutput()
        {
            var compileErrors = new Errors();
            string template = "Hello, {{name. Lots and lots of text following the missing curley";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            string result = TemplateCompiler.Compile(template, templateParameters, compileErrors);

            Assert.That(result, Is.EqualTo("Hello, {{name. Lots and lots of text following the missing curley"));
            Assert.That(compileErrors.HasErrors, Is.True);
            Assert.That(compileErrors.Messages[0], Is.EqualTo("Tempate variable not terminated with }, problem text near '{{name. Lots and lots ...'"));
            Assert.That(compileErrors.Messages[1], Is.EqualTo("Missing dictionary parameter 'name. Lots and lots of...'"));
        }

        [Test]
        public void Templater_WhitespaceShouldBeStripedFrom_TemplateVariableValues()
        {
            var compileErrors = new Errors();
            string template = "Hello, {{ name }.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            string result = TemplateCompiler.Compile(template, templateParameters, compileErrors);

            Assert.That(result, Is.EqualTo("Hello, John."));
            Assert.That(compileErrors.HasErrors, Is.False);
        }
    }

    // What about processing errors/warnings - indication of where happened....errors and warnings at beginning....and unprocessed tags just left in text?


}
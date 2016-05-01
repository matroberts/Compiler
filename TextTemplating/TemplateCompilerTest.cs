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
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, John."));
            Assert.That(compiler.Errors.HasErrors, Is.False);
        }

        [Test]
        public void IfATemplateVariableIsMissing_AnErrorShouldBeReturned_AndTheUnmodifiedTextWrittenIntoTheOutput()
        {
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["WrongName"] = "John",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, {{name}."));
            Assert.That(compiler.Errors.HasErrors, Is.True);
            Assert.That(compiler.Errors.Messages[0], Is.EqualTo("Missing dictionary parameter 'name'"));
        }

        [Test]
        public void IfATemplateVariableIsMissingAClosingCurley_AndErrorShouldBeRetured_AndTheUnmodifiedTextWrittenIntoTheOutput()
        {
            string template = "Hello, {{name. Lots and lots of text following the missing curley";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, {{name. Lots and lots of text following the missing curley"));
            Assert.That(compiler.Errors.HasErrors, Is.True);
            Assert.That(compiler.Errors.Messages[0], Is.EqualTo("Tempate tag not terminated with }, problem text near '{{name. Lots and lots ...'"));
        }

        [Test]
        public void Templater_WhitespaceShouldBeStripedFrom_TemplateVariableValues()
        {
            string template = "Hello, {{ name }.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, John."));
            Assert.That(compiler.Errors.HasErrors, Is.False);
        }

        [Test]
        public void IfAnOpenTagIsTrue_TheTextWithinTheOpenCloseTags_ShouldBeInserted()
        {
            string template = "{?showtext}Optional Text{!showtext}";
            var templateParameters = new Dictionary<string, string>()
            {
                ["showtext"] = "true",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Optional Text"));
            Assert.That(compiler.Errors.HasErrors, Is.False);
        }

        [Test, Ignore("")]
        public void IfAnOpenTagIsFalse_TheTextWithinTheOpenCloseTags_ShouldNotBeInserted()
        {
            string template = "{?showtext}Optional Text{!showtext}";
            var templateParameters = new Dictionary<string, string>()
            {
                ["showtext"] = "false",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo(""));
            Assert.That(compiler.Errors.HasErrors, Is.False);
        }

        // move all error checking to happen at the start

        // boolean tag

        // non-matced boolean tag

        // nested boolean tag
    }
}
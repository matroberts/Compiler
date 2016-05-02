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
            string template = "{?showtext}Optional Text {!showtext}Text After";
            var templateParameters = new Dictionary<string, string>()
            {
                ["showtext"] = "true",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Optional Text Text After"));
            Assert.That(compiler.Errors.HasErrors, Is.False);
        }

        [Test]
        public void IfAnOpenTagIsFalse_TheTextWithinTheOpenCloseTags_ShouldNotBeInserted()
        {
            string template = "{?showtext}Optional Text {!showtext}Text After";
            var templateParameters = new Dictionary<string, string>()
            {
                ["showtext"] = "false",
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(result, Is.EqualTo("Text After"));
            Assert.That(compiler.Errors.HasErrors, Is.False);
        }

        [Test]
        public void NestedBools_OnceTheTextIsSwitchedOff_ItDoesNotSwitchOnAgain_UntilTheTagCloses()
        {
            string template = "{?first}Begin{?second}Middle{!second}End{!first}";
            var templateParameters = new Dictionary<string, string>()
            {
                ["first"] = "false",
                ["second"] = "true"
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(compiler.Errors.HasErrors, Is.False);
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void NestedBools_IfTheInnerTagIsFalse_ItsContentIsNotDisplayed()
        {
            string template = "{?first}Begin{?second}Middle{!second}End{!first}";
            var templateParameters = new Dictionary<string, string>()
            {
                ["first"] = "true",
                ["second"] = "false"
            };

            var compiler = new TemplateCompiler();
            string result = compiler.Compile(template, templateParameters);

            Assert.That(compiler.Errors.HasErrors, Is.False);
            Assert.That(result, Is.EqualTo("BeginEnd"));
        }
    }
}
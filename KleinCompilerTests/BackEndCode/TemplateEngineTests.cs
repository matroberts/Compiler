using System;
using System.Collections.Generic;
using System.Linq;
using KleinCompiler.BackEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.BackEndCode
{
    [TestFixture]
    public class TemplateEngineTests
    {
        [Test]
        public void WithNoReplacementParameters_NothingShouldHappen()
        {
            string input = @"This is my text";
            var parameters = new Dictionary<string, object>();

            var result = TemplateEngine.Render(input, parameters);

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void ReplacementParameters_AreIndicatedWithSquareBrackets()
        {
            string input = @"This is [param1] text";
            var parameters = new Dictionary<string, object>() { ["param1"] = "replacement" };

            var result = TemplateEngine.Render(input, parameters);

            Assert.That(result, Is.EqualTo("This is replacement text"));
        }

        [Test]
        public void MultipleReplacementParameters_AreAllowed()
        {
            string input = @"This is [param1] [param2]";
            var parameters = new Dictionary<string, object>()
            {
                ["param1"] = "replacement",
                ["param2"] = "text",
            };

            var result = TemplateEngine.Render(input, parameters);

            Assert.That(result, Is.EqualTo("This is replacement text"));
        }

        [Test]
        public void IfTheDictionary_DoesNotContainTheReplacementParameter_AnExceptionIsThrown()
        {
            string input = @"This is [param1] text";
            var parameters = new Dictionary<string, object>() {  };

            Assert.That(() => TemplateEngine.Render(input, parameters), Throws.Exception.InstanceOf<KeyNotFoundException>().With.Message.EqualTo("TemplateEngine could not find parameter 'param1' in dictionary"));
        }



    }
}
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
        #region RenderFunctions

        [Test]
        public void RenderFunctions_WithNoReplacementParameters_NothingShouldHappen()
        {
            string input = @"This is my text";
            var parameters = new Dictionary<string, object>();

            var result = TemplateEngine.RenderFunctions(input, parameters);

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void RenderFunctions_ReplacementParameters_AreIndicatedWithSquareBrackets()
        {
            string input = @"This is [func:param1] text";
            var parameters = new Dictionary<string, object>() {["param1"] = "replacement"};

            var result = TemplateEngine.RenderFunctions(input, parameters);

            Assert.That(result, Is.EqualTo("This is replacement text"));
        }

        [Test]
        public void RenderFunctions_MultipleReplacementParameters_AreAllowed()
        {
            string input = @"This is [func:param1] [func:param2]";
            var parameters = new Dictionary<string, object>()
            {
                ["param1"] = "replacement",
                ["param2"] = "text",
            };

            var result = TemplateEngine.RenderFunctions(input, parameters);

            Assert.That(result, Is.EqualTo("This is replacement text"));
        }

        [Test]
        public void RenderFunctions_IfTheDictionary_DoesNotContainTheReplacementParameter_AnExceptionIsThrown()
        {
            string input = @"This is [func:param1] text";
            var parameters = new Dictionary<string, object>() {};

            Assert.That(() => TemplateEngine.RenderFunctions(input, parameters),
                Throws.Exception.InstanceOf<KeyNotFoundException>()
                    .With.Message.EqualTo("RenderFunctions could not find parameter 'param1' in dictionary"));
        }

        #endregion

        #region RenderLabels

        [Test]
        public void RenderLabels_WithNoReplacementParameters_NothingShouldHappen()
        {
            string input = @"This is my text";
            var parameters = new Dictionary<string, object>();

            var result = TemplateEngine.RenderLabels(input, parameters);

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void RenderLabels_ReplacementParameters_AreIndicatedWithSquareBracketsAndLabelPrefix()
        {
            string input = @"This is [label:param1] text";
            var parameters = new Dictionary<string, object>() { ["param1"] = "replacement" };

            var result = TemplateEngine.RenderLabels(input, parameters);

            Assert.That(result, Is.EqualTo("This is replacement text"));
        }

        [Test]
        public void RenderLabels_WillIgnoreFunctionReplacments()
        {
            string input = @"This is [label:param1] [func:param2]";
            var parameters = new Dictionary<string, object>()
            {
                ["param1"] = "replacement",
                ["param2"] = "text",
            };

            var result = TemplateEngine.RenderLabels(input, parameters);

            Assert.That(result, Is.EqualTo("This is replacement [func:param2]"));
        }

        [Test]
        public void RenderLabels_IfTheDictionary_DoesNotContainTheReplacementParameter_AnExceptionIsThrown()
        {
            string input = @"This is [label:param1] text";
            var parameters = new Dictionary<string, object>() { };

            Assert.That(() => TemplateEngine.RenderLabels(input, parameters),
                Throws.Exception.InstanceOf<KeyNotFoundException>()
                    .With.Message.EqualTo("RenderLabels could not find parameter 'param1' in dictionary"));
        }

        #endregion
    }
}
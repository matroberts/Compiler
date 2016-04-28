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
        public void Test()
        {
            string template = "Hello, {{name}.";
            var templateParameters = new Dictionary<string, string>()
            {
                ["name"] = "John",
            };

            string result = Templater.Build(template, templateParameters);

            Assert.That(result, Is.EqualTo("Hello, John."));
        }
    }


}
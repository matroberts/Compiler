using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace KleinCompilerTests.Programs
{
    public class TestDatum
    {
        public static List<TestDatum> ArgsAndAsserts(string input)
        {
            var testDataLine = new Regex(@"^\s*//\s*Args(.*)Assert(.*)$", RegexOptions.Multiline);
            foreach (var match in testDataLine.Matches(input))
            {
                Console.WriteLine(match);
            }
            return testDataLine.Matches(input)
                .Cast<Match>()
                .Select(m => new TestDatum(m.Groups[1].Value, m.Groups[2].Value))
                .ToList();//ToToDictionary(m => m.Groups[1].Value, m => (object)m.Groups[2].Value);
        }

        public TestDatum(string args, string asserts)
        {
            Args = args;
            Asserts = asserts.Split(',').Select(a => a.Trim()).ToList();
        }

        public string Args { get; }
        public List<string> Asserts { get; }
    }

    [TestFixture]
    public class TestDataTests
    {
        [Test]
        public void Test_SingleLineWithOneAssert()
        {
            string input = @"// Args 2 Assert 7
";

            var testData = TestDatum.ArgsAndAsserts(input);

            Assert.That(testData.Count, Is.EqualTo(1));
            Assert.That(testData[0].Args, Is.EqualTo(" 2 "));
            Assert.That(testData[0].Asserts, Is.EquivalentTo(new[] { "7" }));
        }

        [Test]
        public void Test_SingleLineWithMultipleAssert()
        {
            string input = @"// Args 2 Assert 7, 9, 13
";

            var testData = TestDatum.ArgsAndAsserts(input);

            Assert.That(testData.Count, Is.EqualTo(1));
            Assert.That(testData[0].Args, Is.EqualTo(" 2 "));
            Assert.That(testData[0].Asserts, Is.EquivalentTo(new[] { "7", "9", "13" }));
        }

        [Test]
        public void Test_MultipleLineWithMultipleAssert()
        {
            string input = @"// Args 2 Assert 7, 9, 13
// Args 3 Assert 7, 9, 13
// Args 5 Assert 7, 9, 13";

            var testData = TestDatum.ArgsAndAsserts(input);

            Assert.That(testData.Count, Is.EqualTo(3));
        }

        [Test]
        public void Test_WithRealCode()
        {
            string input = @"// -------------------------------
// test: if expression as a value
// -------------------------------
// Args 2 Assert 7

            main(n: integer) : integer
  6 + if n < 0
      then
         10
      else
         n* n";

            var testData = TestDatum.ArgsAndAsserts(input);

            Assert.That(testData.Count, Is.EqualTo(1));
            Assert.That(testData[0].Args, Is.EqualTo(" 2 "));
            Assert.That(testData[0].Asserts, Is.EquivalentTo(new [] {"7"}));
        }
    }
}
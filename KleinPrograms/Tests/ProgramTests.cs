using System;
using System.IO;
using System.Linq;
using System.Text;
using KleinCompiler;
using NUnit.Framework;

namespace KleinPrograms.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Compiler_ShouldCompile_AllOfTheValidSampleKleinPrograms()
        {
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\fullprograms");
            var files = Directory.GetFiles(folder, "*.kln");
            bool allPass = true;
            var result = new StringBuilder();
            foreach (var file in files)
            {
                var input = File.ReadAllText(file);
                var error = new Compiler().Compile(input);
                if (error.ErrorType != Error.ErrorTypeEnum.No)
                {
                    allPass = false;
                    result.AppendLine($"{Path.GetFileName(file)}{error.FilePosition} {error.ToString()}");
                }
            }
            ConsoleWriteLine.If(allPass != true, result.ToString());
            Assert.That(allPass, Is.True);
        }

        [Test]
        public void Compiler_ShouldCompile_AllOfMyPrograms()
        {
            var folder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms");
            var files = Directory.GetFiles(folder, "*.kln");
            bool allPass = true;
            var result = new StringBuilder();
            foreach (var file in files)
            {
                var input = File.ReadAllText(file);
                var error = new Compiler().Compile(input);
                if (error.ErrorType != Error.ErrorTypeEnum.No)
                {
                    allPass = false;
                    result.AppendLine($"{Path.GetFileName(file)}{error.FilePosition} {error.ToString()}");
                }
            }
            ConsoleWriteLine.If(allPass != true, result.ToString());
            Assert.That(allPass, Is.True);
        }

        [TestCase("bad-identifier.kln", "(2, 6) Lexical Error: Unknown character '.'")]
        [TestCase("bad-number.kln", "(2, 6) Lexical Error: Unknown character '.'")]
        [TestCase("empty-file.kln", "(1, 1) Syntax Error: Attempting to parse symbol 'Program' found token End")]
        [TestCase("greater-than.kln", "(3, 8) Lexical Error: Unknown character '>'")]
        [TestCase("identifier-too-long.kln", "(5, 3) Lexical Error: Max length of an identifier is 256 characters")]
        [TestCase("int-with-char.kln", "(9, 12) Syntax Error: Attempting to parse symbol 'OpenBracket' found token End")] // ?
        [TestCase("leading-zero.kln", "(2, 5) Syntax Error: Attempting to parse symbol 'Body' found token Plus '+'")] //?
        public void Compiler_ShouldProduceCorrectErrorMessages_ForSampleScannerErrors(string filename, string message)
        {
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\scanner", filename);
            var input = File.ReadAllText(file);
            var error = new Compiler().Compile(input);

            Assert.That(error.ErrorType, Is.Not.EqualTo(Error.ErrorTypeEnum.No));
            Assert.That(error.ToString(), Is.EqualTo(message));
        }
    }
}
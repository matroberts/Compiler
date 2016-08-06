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
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using KleinCompiler.AbstractSyntaxTree;
using NUnit.Framework;

namespace KleinPrograms.Tests
{
    public class Result
    {
        public Collection<PSObject> Output { get; set; }
        public List<ErrorRecord> Errors { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Errors.Count > 0)
            {
                sb.AppendLine("Errors");
                sb.AppendLine("======");
                for (int i = 0; i < Errors.Count; i++)
                {
                    sb.AppendLine($"{i}) {Errors[i]}");
                }
            }

            if (Output.Count > 0)
            {
                sb.AppendLine("Data");
                sb.AppendLine("====");
                for (int i = 0; i < Output.Count; i++)
                {
                    sb.AppendLine($"{i}) {Output[i]}");
                }
            }
            return sb.ToString();
        }

        public bool HasErrors => Errors.Count > 0;
    }

    public class ScriptRunner
    {
        public static Result Execute(string workingDirectory, string script)
        {
            using (var powerShell = PowerShell.Create())
            {
                powerShell.AddScript($"Set-Location {workingDirectory}");
                powerShell.AddScript($"Invoke-Expression \"{script}\"");
                var output = powerShell.Invoke();
                return new Result
                {
                    Output = output,
                    Errors = powerShell.Streams.Error.ToList()
                };
            }
        }
    }

    public class ConsoleWriteLine
    {
        public static void If(bool condition, string message)
        {
            if (condition)
                Console.WriteLine(message);
        }
    }

    [TestFixture]
    public class ScriptTests
    {
        private string originalWorkingDirectory;

        [SetUp]
        public void SetUp()
        {
            originalWorkingDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [TearDown]
        public void TearDown()
        {
            Environment.CurrentDirectory = originalWorkingDirectory;
        }

        [Test]
        public void Kleins_ShouldProduceTextRepresentation_OfTheTokensOfAProgram()
        {
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory,
                                              @".\kleins.ps1 ..\..\Programs\scanner\reserved-words-and-symbols.kln");

            ConsoleWriteLine.If(result.HasErrors, result.ToString());
            Assert.That(result.HasErrors, Is.False);
            Assert.That(result.Output.Count, Is.EqualTo(23));
            Assert.That(result.Output[0].ToString(), Is.EqualTo("IntegerType 'integer'"));
        }

        [Test]
        public void Kleinf_ShouldReturnErrorInfo_IfTheProgramDoesntParse()
        {
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory, 
                                              @".\kleinf.ps1 ..\..\Programs\scanner\reserved-words-and-symbols.kln");

            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors[0].Exception.Message, Does.Contain("Syntax Error: Attempting to parse symbol 'Program' found token IntegerType 'integer'"));
        }

        [Test]
        public void Kleinf_ShouldReturnTheAst_IfTheProgramDoesParse()
        {
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory,
                                              @".\kleinf.ps1 ..\..\Programs\fullprograms\circular-prime.kln");

            Assert.That(result.HasErrors, Is.False);
            Assert.That(result.Output.Count, Is.EqualTo(1));
            Assert.That(result.Output[0].BaseObject, Is.TypeOf<Program>());
        }

        [Test]
        public void Kleinp_ShouldOutputAPrettyPrint_OfTheAst()
        {
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory,
                                               @".\kleinp.ps1 ..\..\Programs\fullprograms\circular-prime.kln");

            Assert.That(result.HasErrors, Is.False);
            Assert.That(result.Output.Count, Is.EqualTo(1));
            Assert.That(result.Output[0].ToString(), Does.StartWith("Program"));
        }

        [Test]
        public void Kleinp_ShouldPrintError_IfTheProgramDoesNotParse()
        {
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory,
                                               @".\kleinp.ps1 ..\..\Programs\scanner\reserved-words-and-symbols.kln");

            Assert.That(result.HasErrors, Is.True);
            Assert.That(result.Errors[0].Exception.Message, Does.Contain("Syntax Error: Attempting to parse symbol 'Program' found token IntegerType 'integer'"));
        }

        [Test]
        public void Kleinv_ShouldOutput_TheSymbolTable()
        {
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory,
                                               @".\kleinv.ps1 ..\..\Programs\fullprograms\circular-prime.kln | Out-String");

            Assert.That(result.HasErrors, Is.False);
            Assert.That(result.Output.Count, Is.EqualTo(1));
            Assert.That(result.Output[0].ToString(), Does.Contain("circularPrimesTo       (integer):integer                 main   "));
        }

        [Test]
        public void Kleinc_ShouldCompileTheKlienProgram_AndWriteItToFileWithExtension_tm()
        {
            File.Delete("print-one.tm");
            var result = ScriptRunner.Execute(TestContext.CurrentContext.TestDirectory,
                                   @".\kleinc.ps1 ..\..\Programs\fullprograms\print-one.kln | Out-String");

            Assert.That(result.HasErrors, Is.False);
            Assert.That(File.Exists("print-one.tm"), Is.True);
            Assert.That(File.ReadAllLines("print-one.tm")[0], Is.EqualTo("0: LDC 6, 9(0) ; set registers 6 to value 9"));
        }
    }
}
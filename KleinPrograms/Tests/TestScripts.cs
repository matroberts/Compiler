using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
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
    public class TestScripts
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
            Assert.That(result.Output.Count, Is.EqualTo(23));
            Assert.That(result.Output[0].ToString(), Is.EqualTo("IntegerType 'integer'"));
        }

        [Test]
        public void Test()
        {
            var result = ScriptRunner.Execute(@"C:\github\Compiler\KleinPrograms\kleinf.ps1", @"C:\github\Compiler\KleinPrograms\Programs\scanner\reserved-words-and-symbols.kln");

            Console.WriteLine(result.ToString());

//            Assert.That(result.);
            // currently this file just exists to make the project compile
            //powershell C:\github\Compiler\KleinPrograms\kleinf.ps1 C:\github\Compiler\KleinPrograms\Programs\scanner\reserved-words-and-symbols.kln
        }
    }
}
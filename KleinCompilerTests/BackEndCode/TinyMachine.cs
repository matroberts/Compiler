using System;
using System.Diagnostics;
using System.IO;

namespace KleinCompilerTests.BackEndCode
{
    public class TinyMachine
    {
        public string ExePath { get; }
        public string TestFilePath { get; }

        public TinyMachine(string exePath, string testFilePath)
        {
            this.ExePath = exePath;
            this.TestFilePath = testFilePath;
        }

        public string[] Execute(string objectCode)
        {
            File.WriteAllText(TestFilePath, objectCode);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ExePath,
                    Arguments = TestFilePath,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Trim().Split(new []{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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

        public TinyOut Execute(string objectCode, params int[] args)
        {
            File.WriteAllText(TestFilePath, objectCode);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ExePath,
                    Arguments = TestFilePath + " " + string.Join(" ", args),
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return new TinyOut(output.Trim().Split(new []{"\r\n"}, StringSplitOptions.RemoveEmptyEntries));
        }
    }

    public class TinyOut : List<string>
    {
        public TinyOut(IEnumerable<string> enumerableOfStrings) : base(enumerableOfStrings)
        {
        }

        public TinyOut()
        {
            
        }
        public override string ToString()
        {
            return string.Join(", ", this);
        }
    }
}

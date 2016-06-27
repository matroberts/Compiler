using System;

namespace KleinCompilerTests
{
    public class ConsoleWriteLine
    {
        public static void If(bool condition, string message)
        {
            if(condition)
                Console.WriteLine(message);
        }
    }
}
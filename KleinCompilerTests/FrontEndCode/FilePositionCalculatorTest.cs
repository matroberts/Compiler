using KleinCompiler.FrontEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.FrontEndCode
{
    [TestFixture]
    public class FilePositionCalculatorTest
    {
        [Test]
        public void Lines_ShouldReturnTheNumberOfLines_InATextString()
        {
            string input = @"1 line 1
2 line 2
3 line 3";

            var calculator = new FilePositionCalculator(input);

            Assert.That(calculator.Lines, Is.EqualTo(3));
        }

        [Test]
        public void Lines_AnEmptyString_HasOneLine()
        {
            string input = @"";

            var calculator = new FilePositionCalculator(input);

            Assert.That(calculator.Lines, Is.EqualTo(1));
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(2, 1, 3)]
        [TestCase(3, 1, 4)]
        [TestCase(4, 1, 5)]
        public void FilePosition_OnFileWithNoNewLines_WorksCorrectly(int position, int linenumber, int lineposition)
        {
            string input = "01234";

            var calculator = new FilePositionCalculator(input);

            Assert.That(calculator.FilePosition(position), Is.EqualTo(new FilePosition(linenumber, lineposition)));
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 2, 1)]
        [TestCase(2, 3, 1)]
        [TestCase(3, 4, 1)]
        [TestCase(4, 5, 1)]
        public void FilePosition_OnFileWithOnlyNewLines_WorksCorrectly(int position, int linenumber, int lineposition)
        {
            string input = "\n\n\n\n\n";

            var calculator = new FilePositionCalculator(input);

            Assert.That(calculator.FilePosition(position), Is.EqualTo(new FilePosition(linenumber, lineposition)));
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(2, 1, 3)]
        [TestCase(3, 1, 4)]
        [TestCase(4, 2, 1)]
        [TestCase(5, 2, 2)]
        [TestCase(6, 2, 3)]
        [TestCase(7, 2, 4)]
        [TestCase(8, 3, 1)]
        [TestCase(9, 3, 2)]
        public void FilePosition_OnFileWithMixedCharactersAndNewLines_WorksCorrectly(int position, int linenumber, int lineposition)
        {
            string input = "012\n456\n89";

            var calculator = new FilePositionCalculator(input);

            Assert.That(calculator.FilePosition(position), Is.EqualTo(new FilePosition(linenumber, lineposition)));
        }

        [Test]
        public void IfTheRequestedFilePosition_IsBeyondTheEndOfTheFile_TheResultIsAsIfTheFileHadBeenExtendedWithCharactersOnItsFinalLine()
        {
            string input = "012\n456\n89";

            var calculator = new FilePositionCalculator(input);

            Assert.That(calculator.FilePosition(10), Is.EqualTo(new FilePosition(3, 3)));
        }
    }
}
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
                else
                {
                    result.AppendLine($"{Path.GetFileName(file)} Pass");
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
        public void ScannerErrors_Compiler_ShouldProduceCorrectErrorMessages(string filename, string message)
        {
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\scanner", filename);
            var input = File.ReadAllText(file);
            var error = new Compiler().Compile(input);

            Assert.That(error.ErrorType, Is.Not.EqualTo(Error.ErrorTypeEnum.No));
            Assert.That(error.ToString(), Is.EqualTo(message));
        }

        // derive-left-to-right.kln seems to be a valid program
        [TestCase("euclid-body.kln", "(6, 7) Syntax Error: Attempting to parse symbol 'FuncTail' found token Colon ':'")] 
        [TestCase("euclid-colon-parms.kln", "(18, 21) Syntax Error: Attempting to parse symbol 'Colon' found token IntegerType 'integer'")] 
        [TestCase("euclid-comma-call.kln", "(15, 26) Syntax Error: Attempting to parse symbol 'ActualsTail' found token Identifier 'b'")] 
        [TestCase("euclid-comma-parms.kln", "(4, 23) Syntax Error: Attempting to parse symbol 'FormalTail' found token Identifier 'b'")] 
        [TestCase("euclid-else-clause.kln", "(16, 8) Syntax Error: Attempting to parse symbol 'FuncTail' found token Colon ':'")] 
        [TestCase("euclid-parms-type.kln", "(18, 7) Syntax Error: Attempting to parse symbol 'Colon' found token Comma ','")] 
        [TestCase("euclid-return-type.kln", "(12, 4) Syntax Error: Attempting to parse symbol 'Type' found token If 'if'")] 
        [TestCase("euclid-then.kln", "(6, 7) Syntax Error: Attempting to parse symbol 'Then' found token Identifier 'a'")] 
        [TestCase("euclid-then.kln", "(6, 7) Syntax Error: Attempting to parse symbol 'Then' found token Identifier 'a'")] 
        public void ParserErrors_Compiler_ShouldProduceCorrectErrorMessages(string filename, string message)
        {
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\parser", filename);
            var input = File.ReadAllText(file);
            var error = new Compiler().Compile(input);

            Assert.That(error.ErrorType, Is.Not.EqualTo(Error.ErrorTypeEnum.No));
            Assert.That(error.ToString(), Is.EqualTo(message));
        }

        [TestCase("01-boolean.kln", "(1, 1) Semantic Error: Function 'main' has a return type 'boolean', but its body has a type 'integer'")]
        [TestCase("01-integer.kln", "(1, 1) Semantic Error: Function 'main' has a return type 'integer', but its body has a type 'boolean'")]
        [TestCase("02-argument.kln", "(1, 1) Semantic Error: Function 'main' has a return type 'boolean', but its body has a type 'integer'")]
        [TestCase("03-arithmetic-computed.kln", "(2, 5) Semantic Error: Plus right expression is not integer")]
        [TestCase("03-arithmetic-plus.kln", "(2, 8) Semantic Error: Plus left expression is not integer")]
        [TestCase("03-arithmetic-times.kln", "(2, 5) Semantic Error: Times right expression is not integer")]
        [TestCase("04-boolean.kln", "(2, 5) Semantic Error: LessThan right expression is not integer")]
        [TestCase("05-if-expression-else.kln", "(2, 3) Semantic Error: IfThenElse, type of then and else expression must be the same")]
        [TestCase("05-if-expression-then.kln", "(2, 3) Semantic Error: IfThenElse, type of then and else expression must be the same")]
        [TestCase("06-function-arg-boolean.kln", "(2, 3) Semantic Error: Function inc(integer):integer called with mismatched arguments inc(boolean)")]
        [TestCase("06-function-arg-computed.kln", "(2, 3) Semantic Error: Function inc(integer):integer called with mismatched arguments inc(boolean)")]
        [TestCase("06-function-arg-count.kln", "(2, 3) Semantic Error: Function inc(integer):integer called with mismatched arguments inc(integer,boolean)")]
        [TestCase("06-function-arg-integer.kln", "(2, 3) Semantic Error: Function boolToInt(boolean):integer called with mismatched arguments boolToInt(integer)")]
        [TestCase("06-function-val-returned.kln", "(1, 1) Semantic Error: Function 'main' has a return type 'integer', but its body has a type 'boolean'")]
        [TestCase("06-function-val-used.kln", "(2, 10) Semantic Error: Or left expression is not boolean")]
        [TestCase("07-two-args.kln", "(2, 3) Semantic Error: Function f(integer,integer):integer called with mismatched arguments f(integer,boolean)")]
        [TestCase("08-print-as-val.kln", "(4, 1) Syntax Error: Attempting to parse symbol 'Body' found token End")]
        [TestCase("09-no-main.kln", "(1, 1) Semantic Error: Program must contain a function 'main'")]
        [TestCase("10-two-main.kln", "(1, 1) Semantic Error: Program contains duplicate function name 'main'")]
        [TestCase("11-two-other.kln", "(1, 1) Semantic Error: Program contains duplicate function name 'f'")]
        [TestCase("12-print.kln", "(5, 1) Syntax Error: Attempting to parse symbol 'FactorTail' found token PrintKeyword 'print'")]
        [TestCase("13-invisible.kln", "(5, 5) Semantic Error: Use of undeclared identifier n in function f")]
        public void TypeErrors_Compiler_ShouldProduceCorrectErrorMessages(string filename, string message)
        {
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\KleinPrograms\Programs\typeerrors", filename);
            var input = File.ReadAllText(file);
            var error = new Compiler().Compile(input);

            Assert.That(error.ErrorType, Is.Not.EqualTo(Error.ErrorTypeEnum.No));
            Console.WriteLine(error.ToString());
            Assert.That(error.ToString(), Is.EqualTo(message));
        }

    }
}
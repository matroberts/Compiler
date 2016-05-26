using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class TokenizerTests
    {
        #region empty

        [Test]
        public void GetNextToken_ReturnsNull_WhenThereAreNoMoreTokens()
        {
            var input = string.Empty;

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void GetNextToken_ReturnsNull_WhenTheInputIsWhitespace()
        {
            var input = "    ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.Null);
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region identifiers

        [Test]
        public void Identifier_ASingleCharacter_IsAnIdentifier()
        {
            var input = "a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "a", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_AStringOfCharacters_IsAnIdentifier()
        {
            var input = "aa";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "aa", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_LeadingWhiteSpace_ShouldBeThrownAway()
        {
            var input = " a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "a", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_TrailingWhiteSpace_ShouldBeThrownAway()
        {
            var input = "abc ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "abc", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_SeriesOfIdentifiers_SeparatedByWhiteSpace()
        {
            var input = " aa bb c ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "aa", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "bb", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "c", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [TestCase("print")]
        [TestCase("main")]
        public void Identifier_Print_and_Main_areIdentifiers(string input)
        {
            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, input, 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifiers_NumbersAreAllowedInAnIdentifier_AfterTheInitialCharacter()
        {
            var input = "log10";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "log10", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_MaxLengthIs256_Characters()
        {
            var input = new string('a', 256);

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, new string('a', 256), 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_IfMaxLengthOf256IsExceeded_AnErrorTokenIsReturned()
        {
            var input = new string('a', 257);

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken(new string('a', 257), 0, "Max length of a token is 256 characters")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region keywords

        [TestCase(SymbolName.IntegerType, "integer")]
        [TestCase(SymbolName.BooleanType, "boolean")]
        [TestCase(SymbolName.If, "if")]
        [TestCase(SymbolName.Then, "then")]
        [TestCase(SymbolName.Else, "else")]
        [TestCase(SymbolName.Not, "not")]
        [TestCase(SymbolName.Or, "or")]
        [TestCase(SymbolName.And, "and")]
        [TestCase(SymbolName.BooleanTrue, "true")]
        [TestCase(SymbolName.BooleanFalse, "false")]
        [TestCase(SymbolName.Plus, "+")]
        [TestCase(SymbolName.Minus, "-")]
        [TestCase(SymbolName.Multiply, "*")]
        [TestCase(SymbolName.Divide, "/")]
        [TestCase(SymbolName.LessThan, "<")]
        [TestCase(SymbolName.Equality, "=")]
        [TestCase(SymbolName.OpenBracket, "(")]
        [TestCase(SymbolName.CloseBracket, ")")]
        [TestCase(SymbolName.Comma, ",")]
        [TestCase(SymbolName.Colon, ":")]
        public void Keywords_ShouldBeRecognised_AsKeywords(SymbolName name, string keyword)
        {
            var tokenizer = new Tokenizer(keyword);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken(name, keyword, 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByEndOfFile_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "intege", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByWhitespace_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "intege", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_ExtendedKeyword_ShouldBeRecognisedAsAnIdentifier_Ie_LongestTokenIsPreferred()
        {
            var input = "integerr";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "integerr", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void IdentifierAndKeyword_ShouldBeRecognised()
        {
            var input = " woot integer ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "woot", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken(SymbolName.IntegerType, "integer", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region line comments

        [Test]
        public void ASingleForwardSlash_AtTheEndOfTheFile_IsADivide()
        {
            var input = "    /";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken(SymbolName.Divide, "/", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void ASingleForwardSlash_InTheMiddleOfAFile_IsADivide()
        {
            var input = "    /    ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken(SymbolName.Divide, "/", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_TwoForwardSlashed_StartALineComment()
        {
            var input = "//";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.LineComment, "//", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_EndOfFile_EndsTheComment()
        {
            var input = "// comment";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.LineComment, "// comment", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_EndOfLine_EndsTheComment_AndTheCommentDoesNotIncludeTheNewLine()
        {
            var input = @"// comment
identifier";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.LineComment, "// comment", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "identifier", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_ALineComment_CanStartInTheMiddleOfALine()
        {
            var input = @"hey
hey // my
my";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "hey", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "hey", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.LineComment, "// my", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.Identifier, "my", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region Block Comment

        [Test]
        public void BlockComment_IsFormedByOpeningAndClosingCurly()
        {
            var input = "{}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.BlockComment, "{}", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void BlockComment_WhichIsOpened_ButWhereTheFileEndsWithoutAClose_IsAnError()
        {
            var input = "{";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken("{", 0, "missing } in block comment")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void BlockComment_TextWithinTheCurlys_CountsAsTheComment()
        {
            var input = "{text}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.BlockComment, "{text}", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void BlockComment_CanSpanMultipleLines()
        {
            var input = @"{
   ident
   true
   4
   *
}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.BlockComment, input, 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region integer literal

        [Test]
        public void ASingleNumber_AtTheEndOfTheFile_IsANumberLiteral()
        {
            var input = "0";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.IntegerLiteral, "0", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void ASingleNumber_DelimitedByWhitespace_IsANumberLiteral()
        {
            var input = "0 ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.IntegerLiteral, "0", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void AStringOfNumbers_IsANumberLiteral()
        {
            var input = "1234567890 ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.IntegerLiteral, "1234567890", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [TestCase("00")]
        [TestCase("09")]
        public void IntegerLiteralsAreNotAllowedLeadingZeros_IfTheyDoAnErrorIsRaised(string input)
        {
            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken(input, 0, "Number literals are not allowed leading zeros")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void MaxiumSizeOfIntegerLiteralIs_2Power32minus1_ie_4294967295()
        {
            var input = "4294967295";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(SymbolName.IntegerLiteral, "4294967295", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void IfMaxIntegerSizeIfExceeded_AnErrorTokenIsGenerated()
        {
            var input = "4294967296";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken("4294967296", 0, "Maximum size of integer literal is 4294967295")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region position

        [Test]
        public void Postion_EachTypeOfToken_ShouldRecordItsPostionInTheInputStream_ZeroBasedIndexing()
        {
            var input = "  23 variable true {bc} //lc";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken().Position, Is.EqualTo(2));
            Assert.That(tokenizer.GetNextToken().Position, Is.EqualTo(5));
            Assert.That(tokenizer.GetNextToken().Position, Is.EqualTo(14));
            Assert.That(tokenizer.GetNextToken().Position, Is.EqualTo(19));
            Assert.That(tokenizer.GetNextToken().Position, Is.EqualTo(24));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region error token

        [Test]
        public void IfAnUnknownCharacterIsEncountered_AnErrorToken_IsProduced()
        {
            var input = "!";
        
            var tokenizer = new Tokenizer(input);
        
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken("!", 0, "Unknown character '!'")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }


        #endregion

        // should a number be delimited by whitespace?
    }
}


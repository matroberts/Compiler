using KleinCompiler.FrontEndCode;
using NUnit.Framework;

namespace KleinCompilerTests.FrontEndCode
{
    [TestFixture]
    public class TokenizerTests
    {
        #region empty

        [Test]
        public void GetNextToken_ReturnsEnd_WhenThereAreNoMoreTokens()
        {
            var input = string.Empty;

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void GetNextToken_ReturnsEnd_WhenTheInputIsWhitespace()
        {
            var input = "    ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion

        #region identifiers

        [Test]
        public void Identifier_ASingleCharacter_IsAnIdentifier()
        {
            var input = "a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifier_AStringOfCharacters_IsAnIdentifier()
        {
            var input = "aa";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "aa", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifier_LeadingWhiteSpace_ShouldBeThrownAway()
        {
            var input = " a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifier_TrailingWhiteSpace_ShouldBeThrownAway()
        {
            var input = "abc ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "abc", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifier_SeriesOfIdentifiers_SeparatedByWhiteSpace()
        {
            var input = " aa bb c ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "aa", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "bb", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "c", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [TestCase("main")]
        public void Identifier_Main_IsAnIdentifiers(string input)
        {
            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, input, 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifiers_NumbersAreAllowedInAnIdentifier_AfterTheInitialCharacter()
        {
            var input = "log10";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "log10", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifier_MaxLengthIs256_Characters()
        {
            var input = new string('a', 256);

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, new string('a', 256), 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Identifier_IfMaxLengthOf256IsExceeded_AnErrorTokenIsReturned()
        {
            var input = new string('a', 257);

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken(new string('a', 257), 0, "Max length of an identifier is 256 characters")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion

        #region keywords

        [TestCase(Symbol.IntegerType, "integer")]
        [TestCase(Symbol.BooleanType, "boolean")]
        [TestCase(Symbol.If, "if")]
        [TestCase(Symbol.Then, "then")]
        [TestCase(Symbol.Else, "else")]
        [TestCase(Symbol.Not, "not")]
        [TestCase(Symbol.Or, "or")]
        [TestCase(Symbol.And, "and")]
        [TestCase(Symbol.BooleanTrue, "true")]
        [TestCase(Symbol.BooleanFalse, "false")]
        [TestCase(Symbol.Plus, "+")]
        [TestCase(Symbol.Minus, "-")]
        [TestCase(Symbol.Times, "*")]
        [TestCase(Symbol.Divide, "/")]
        [TestCase(Symbol.LessThan, "<")]
        [TestCase(Symbol.Equality, "=")]
        [TestCase(Symbol.OpenBracket, "(")]
        [TestCase(Symbol.CloseBracket, ")")]
        [TestCase(Symbol.Comma, ",")]
        [TestCase(Symbol.Colon, ":")]
        [TestCase(Symbol.PrintKeyword, "print")]
        public void KeywordsAndOperators_ShouldBeRecognised_AsKeywords(Symbol symbol, string text)
        {
            var tokenizer = new Tokenizer(text);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(symbol, text, 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByEndOfFile_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "intege", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByWhitespace_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "intege", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Keyword_ExtendedKeyword_ShouldBeRecognisedAsAnIdentifier_Ie_LongestTokenIsPreferred()
        {
            var input = "integerr";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "integerr", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void IdentifierAndKeyword_ShouldBeRecognised()
        {
            var input = " woot integer ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "woot", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.IntegerType, "integer", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion

        #region line comments

        [Test]
        public void ASingleForwardSlash_AtTheEndOfTheFile_IsADivide()
        {
            var input = "    /";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Divide, "/", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void ASingleForwardSlash_InTheMiddleOfAFile_IsADivide()
        {
            var input = "    /    ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Divide, "/", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void LineComment_TwoForwardSlashed_StartALineComment()
        {
            var input = "//";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.LineComment, "//", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void LineComment_EndOfFile_EndsTheComment()
        {
            var input = "// comment";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.LineComment, "// comment", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void LineComment_EndOfLine_EndsTheComment_AndTheCommentDoesNotIncludeTheNewLine()
        {
            var input = @"// comment
identifier";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.LineComment, "// comment", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "identifier", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void LineComment_ALineComment_CanStartInTheMiddleOfALine()
        {
            var input = @"hey
hey // my
my";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "hey", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "hey", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.LineComment, "// my", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.Identifier, "my", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion

        #region Block Comment

        [Test]
        public void BlockComment_IsFormedByOpeningAndClosingCurly()
        {
            var input = "{}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.BlockComment, "{}", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void BlockComment_WhichIsOpened_ButWhereTheFileEndsWithoutAClose_IsAnError()
        {
            var input = "{";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken("{", 0, "missing } in block comment")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void BlockComment_TextWithinTheCurlys_CountsAsTheComment()
        {
            var input = "{text}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.BlockComment, "{text}", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
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

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.BlockComment, input, 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion

        #region integer literal

        [Test]
        public void ASingleNumber_AtTheEndOfTheFile_IsANumberLiteral()
        {
            var input = "0";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.IntegerLiteral, "0", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void ASingleNumber_DelimitedByWhitespace_IsANumberLiteral()
        {
            var input = "0 ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.IntegerLiteral, "0", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void AStringOfNumbers_IsANumberLiteral()
        {
            var input = "1234567890 ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.IntegerLiteral, "1234567890", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [TestCase("00")]
        [TestCase("09")]
        public void IntegerLiteralsAreNotAllowedLeadingZeros_IfTheyDoAnErrorIsRaised(string input)
        {
            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken(input, 0, "Number literals are not allowed leading zeros")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void MaxiumSizeOfIntegerLiteralIs_2Power32minus1_ie_4294967295()
        {
            var input = "4294967295";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.IntegerLiteral, "4294967295", 0)));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void IfMaxIntegerSizeIfExceeded_AnErrorTokenIsGenerated()
        {
            var input = "4294967296";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken("4294967296", 0, "Maximum size of integer literal is 4294967295")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
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
            Assert.That(tokenizer.GetNextToken().Position, Is.EqualTo(28)); //End token
        }

        #endregion

        #region error token

        [Test]
        public void IfAnUnknownCharacterIsEncountered_AnErrorToken_IsProduced()
        {
            var input = "!";
        
            var tokenizer = new Tokenizer(input);
        
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new ErrorToken("!", 0, "Unknown character '!'")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }


        #endregion

        #region Pop

        [Test]
        public void Pop_ShouldReturnTheNextToken()
        {
            var input = "main () : boolean";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "main", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.OpenBracket, "(", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.CloseBracket, ")", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Colon, ":", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.BooleanType, "boolean", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Pop_ShouldIgnoreLineComments()
        {
            var input = @"// begin line comment
                          main () // middle line comment
                          : boolean
                          // end line comment";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "main", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.OpenBracket, "(", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.CloseBracket, ")", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Colon, ":", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.BooleanType, "boolean", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Pop_ShouldIgnoreBlockComments()
        {
            var input = @"{ begin block comment }
                          main () { middle block comment }
                          : boolean
                          { end block comment }";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "main", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.OpenBracket, "(", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.CloseBracket, ")", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Colon, ":", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.BooleanType, "boolean", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion

        #region Peek

        [Test]
        public void Peek_ShouldReturnTheNextToken_WithoutAdvancingTheStream()
        {
            var input = "a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Peek(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Peek_ShouldIgnoreLineComments()
        {
            var input = @"// line comment
                          a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Peek(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Peek_ShouldIgnoreBlockComments()
        {
            var input = @"{ line comment }
                          a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Peek(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "a", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        [Test]
        public void Peek_ShouldNotScrewUp_IfPeekAtTheEndOfTheStream()
        {
            var input = " aaa";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.Identifier, "aaa", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
            Assert.That(tokenizer.Peek(), Is.EqualTo(new Token(Symbol.End, "", 0)));
            Assert.That(tokenizer.Pop(), Is.EqualTo(new Token(Symbol.End, "", 0)));
        }

        #endregion
    }
}


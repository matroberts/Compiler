using System;
using System.Linq;
using System.Runtime.InteropServices;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests.Lexer
{
/*
Tokens
======
LineComment                  - // continues to end of line
                             - //.*\n

BlockComment                 - { all the text within the curleys is comment }
                             - {[^}]*}

Identifier                   - Up to 256 characters case sensitive.  main and print are primitive identifiers
                             - [a-zA-Z]+
Literal
    IntegerLiteral           - -2^32 to 2^32-1
                             - (-)?[0-9]+
Keyword
    BooleanLiteral           - true or false
                             - (true|false)
    IntegerType              - integer
    BooleanType              - boolean
    IfKeyword                - if
    ThenKeyword              - then
    ElseKeyword              - else
    NotOperator              - not
    OrOperator               - or
    AndOperator              - and
    MainIdentifier           - main
    PrintIdentifier          - print

    PlusOperator             - +
    MinusOperator            - -
    MultiplicationOperator   - *
    DivisionOperator         - \
    LessThanOperator         - <
    EqualityOperator         - =
    OpenBracket              - (
    CloseBracket             - )
    Comma                    - ,
    Colon                    - :


Notes
=====
minus (-)  is used twice

*/

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

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("a")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_AStringOfCharacters_IsAnIdentifier()
        {
            var input = "aa";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("aa")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_LeadingWhiteSpace_ShouldBeThrownAway()
        {
            var input = " a";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("a")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_TrailingWhiteSpace_ShouldBeThrownAway()
        {
            var input = "abc ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("abc")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Identifier_SeriesOfIdentifiers_SeparatedByWhiteSpace()
        {
            var input = " aa bb c ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("aa")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("bb")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("c")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region keywords

        [TestCase("integer")]
        [TestCase("boolean")]
        [TestCase("if")]
        [TestCase("then")]
        [TestCase("else")]
        [TestCase("not")]
        [TestCase("or")]
        [TestCase("and")]
        [TestCase("main")]
        [TestCase("print")]
        [TestCase("true")]
        [TestCase("false")]
        [TestCase("+")]
        [TestCase("-")]
        [TestCase("*")]
        [TestCase("\\")]
        [TestCase("<")]
        [TestCase("=")]
        [TestCase("(")]
        [TestCase(")")]
        [TestCase(",")]
        [TestCase(":")]
        public void Keywords_ShouldBeRecognised_AsKeywords(string keyword)
        {
            var tokenizer = new Tokenizer(keyword);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken(keyword)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByEndOfFile_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("intege")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_PartialKeyword_DelimitedByWhitespace_ShouldBeRecognisedAsAnIdentifier()
        {
            var input = "intege ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("intege")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void Keyword_ExtendedKeyword_ShouldBeRecognisedAsAnIdentifier_Ie_LongestTokenIsPreferred()
        {
            var input = "integerr";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("integerr")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void IdentifierAndKeyword_ShouldBeRecognised()
        {
            var input = " woot integer ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("woot")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new KeywordToken("integer")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region line comments

        [Test]
        public void ASingleForwardSlash_AtTheEndOfTheFile_IsNothing_AndIsIgnored()
        {
            var input = "    /";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void ASingleForwardSlash_InTheMiddleOfAFile_IsNothing_AndIsIgnored()
        {
            var input = "    /    ";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_TwoForwardSlashed_StartALineComment()
        {
            var input = "//";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new LineCommentToken("//")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_EndOfFile_EndsTheComment()
        {
            var input = "// comment";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new LineCommentToken("// comment")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_EndOfLine_EndsTheComment_AndTheCommentDoesNotIncludeTheNewLine()
        {
            var input = @"// comment
identifier";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new LineCommentToken("// comment")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("identifier")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void LineComment_ALineComment_CanStartInTheMiddleOfALine()
        {
            var input = @"hey
hey // my
my";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("hey")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("hey")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new LineCommentToken("// my")));
            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new IdentifierToken("my")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        #region Block Comment

        [Test]
        public void BlockComment_IsFormedByOpeningAndClosingCurly()
        {
            var input = "{}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new BlockCommentToken("{}")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void BlockComment_WhichIsOpened_ButWhereTheFileEndsWithoutAClose_CountsAsABlockComment()
        {
            var input = "{";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new BlockCommentToken("{")));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        [Test]
        public void BlockComment_TextWithinTheCurlys_CountsAsTheComment()
        {
            var input = "{text}";

            var tokenizer = new Tokenizer(input);

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new BlockCommentToken("{text}")));
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

            Assert.That(tokenizer.GetNextToken(), Is.EqualTo(new BlockCommentToken(input)));
            Assert.That(tokenizer.GetNextToken(), Is.Null);
        }

        #endregion

        // numbers
    }
}
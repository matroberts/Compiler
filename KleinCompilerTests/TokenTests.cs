using System;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void Token_HasValueEquality()
        {
            var token = new Token(Symbol.BooleanFalse, "false", 0);

            // null
            Assert.That(token.Equals(null), Is.False);
            // different type
            Assert.That(token.Equals("false"), Is.False);
            // different symbol
            Assert.That(token.Equals(new Token(Symbol.Identifier, "false", 0)), Is.False);
            // different value
            Assert.That(token.Equals(new Token(Symbol.BooleanFalse, "false", 0)), Is.True);

            // same object
            Assert.That(token.Equals(token), Is.True);
            // same symbol and value
            Assert.That(new Token(Symbol.Identifier, "false", 0).Equals(new Token(Symbol.Identifier, "true", 0)), Is.False);
        }

        [Test]
        public void ErrorToken_Equality_ConsidersTheMessage_AsWellAsValue()
        {
            var token = new ErrorToken("{", 0, "message");

            Assert.That(token.Equals(null), Is.False);
            Assert.That(token.Equals("identifier"), Is.False);
            Assert.That(token.Equals(new Token(Symbol.Identifier, "{", 0)), Is.False);

            Assert.That(token.Equals(token), Is.True);
            Assert.That(token.Equals(new ErrorToken("{", 0, "different message")), Is.False);
            Assert.That(token.Equals(new ErrorToken("/", 0, "message")), Is.False);
            Assert.That(token.Equals(new ErrorToken("{", 0, "message")), Is.True);
        }

        [Test]
        public void Token_ToString_ShouldFormatInAnAcceptableWay()
        {
            Console.WriteLine(new Token(Symbol.End, "", 0));
            Console.WriteLine(new Token(Symbol.Identifier, "identifier", 0));
            Console.WriteLine(new Token(Symbol.Identifier, "longIdentifierHasNoTruncationInToStringEvenIfItsReallyLong", 0));
            Console.WriteLine(new ErrorToken(new string('a', 257), 0, "Max length of a token is 256 characters"));
            Console.WriteLine(new Token(Symbol.IntegerType, Symbol.IntegerType.ToKeyword(), 0));
            Console.WriteLine(new Token(Symbol.LineComment, "// comment", 0));
            Console.WriteLine(new Token(Symbol.LineComment, "// line comment truncates if its longer than 50 characters", 0));
            Console.WriteLine(new Token(Symbol.BlockComment, "{}", 0));
            Console.WriteLine(new Token(Symbol.BlockComment, @"{ block comments 
strips newlines and is truncated at 50 charcters
}", 0));
            Console.WriteLine(new ErrorToken("{", 0, "missing } in block comment"));
            Console.WriteLine(new ErrorToken("09", 0, "Number literals are not allowed leading zeros"));
            Console.WriteLine(new Token(Symbol.IntegerLiteral, "4294967295", 0));
            Console.WriteLine(new ErrorToken("4294967296", 0, "Maximum size of integer literal is 4294967295"));
            Console.WriteLine(new ErrorToken("!", 0, "Unknown character '!'"));
        }
    }
}
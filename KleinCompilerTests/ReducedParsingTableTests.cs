using System;
using KleinCompiler;
using NUnit.Framework;
using NUnit.Framework.Api;

namespace KleinCompilerTests
{
    [TestFixture]
    public class ReducedParsingTableTests
    {
        [Test]
        public void TheReducedParserTable_ShouldBeInitialisedWithNulls()
        {
            var parsingTable = new ReducedParsingTable();

            Assert.That(parsingTable[0,0], Is.Null);
            Assert.That(parsingTable[Symbol.Equality, Symbol.Identifier], Is.Null);
        }

        [Test]
        public void ParsingTable_ShouldReturnTheCorrectlyNamedRule()
        {
            //    M[Program, identifier] = R1
            //    M[DefTail, identifier] = R2
            //    M[DefTail, $ ] = R3
            //    M[Def, identifier] = R4
            //    M[Formals, ) ]                  = R5
            //    M[Formals, identifier] = R6
            //    M[NonEmptyFormals, identifier] = R7
            //    M[FormalsTail, ","] = R8
            //    M[FormalsTail, ) ]              = R9
            //    M[Formal, identifier] = R10
            //    M[Type, integer] = R11
            //    M[Type, boolean] = R12

            var table = new ReducedParsingTable();
            Assert.That(table[Symbol.Program, Symbol.Identifier].Name, Is.EqualTo("R1"));
            Assert.That(table[Symbol.DefTail, Symbol.Identifier].Name, Is.EqualTo("R2"));
            Assert.That(table[Symbol.DefTail, Symbol.End].Name, Is.EqualTo("R3"));
        }

        [Test]
        public void ParsingTable_ShouldReturnTheRuleWithCorrectSymbols()
        {
            // R1 < Program >             ::= < Def > < DefTail >
            // R2 < DefTail >             ::= < Def > < DefTail >
            // R3                           | e
            // R4 < Def >                 ::= < Identifier > ( < Formals > ) : < Type >
            // R5 < Formals >             ::= e
            // R6                           | < NonEmptyFormals >
            // R7 < NonEmptyFormals >     ::= < Formal > < FormalsTail >
            // R8 < FormalsTail >         ::= , < Formal >< FormalsTail >
            // R9                           | e
            // R10 < Formal >             ::= < Identifier > : < Type >
            // R11 < Type >               ::= integer
            // R12                          | boolean
            var table = new ReducedParsingTable();

            Assert.That(table[Symbol.Program, Symbol.Identifier].Symbols, Is.EqualTo(new[] { Symbol.Def, Symbol.DefTail, }));
            Assert.That(table[Symbol.DefTail, Symbol.Identifier].Symbols, Is.EqualTo(new[] { Symbol.Def, Symbol.DefTail, }));
            Assert.That(table[Symbol.DefTail, Symbol.End].Symbols, Is.Empty);
            Assert.That(table[Symbol.Def, Symbol.Identifier].Symbols, Is.EqualTo(new [] {Symbol.Identifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type, }));
            Assert.That(table[Symbol.Formals, Symbol.CloseBracket].Symbols, Is.Empty);
            Assert.That(table[Symbol.Formals, Symbol.Identifier].Symbols, Is.EqualTo(new [] {Symbol.NonEmptyFormals}));
            Assert.That(table[Symbol.NonEmptyFormals, Symbol.Identifier].Symbols, Is.EqualTo(new [] { Symbol.Formal, Symbol.FormalsTail, }));
            Assert.That(table[Symbol.FormalsTail, Symbol.Comma].Symbols, Is.EqualTo(new [] {Symbol.Comma, Symbol.Formal, Symbol.FormalsTail, }));
            Assert.That(table[Symbol.FormalsTail, Symbol.CloseBracket].Symbols, Is.Empty);
            Assert.That(table[Symbol.Formal, Symbol.Identifier].Symbols, Is.EqualTo(new [] {Symbol.Identifier, Symbol.Colon, Symbol.Type, }));
            Assert.That(table[Symbol.Type, Symbol.IntegerType].Symbols, Is.EqualTo(new [] {Symbol.IntegerType}));
            Assert.That(table[Symbol.Type, Symbol.BooleanType].Symbols, Is.EqualTo(new [] {Symbol.BooleanType}));
        }
    }
}
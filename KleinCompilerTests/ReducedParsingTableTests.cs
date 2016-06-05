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
            Assert.That(parsingTable[SymbolName.Equality, SymbolName.Identifier], Is.Null);
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
            Assert.That(table[SymbolName.Program, SymbolName.Identifier].Name, Is.EqualTo("R1"));
            Assert.That(table[SymbolName.DefTail, SymbolName.Identifier].Name, Is.EqualTo("R2"));
            Assert.That(table[SymbolName.DefTail, SymbolName.End].Name, Is.EqualTo("R3"));
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

            Assert.That(table[SymbolName.Program, SymbolName.Identifier].Symbols, Is.EqualTo(new[] { SymbolName.Def, SymbolName.DefTail, }));
            Assert.That(table[SymbolName.DefTail, SymbolName.Identifier].Symbols, Is.EqualTo(new[] { SymbolName.Def, SymbolName.DefTail, }));
            Assert.That(table[SymbolName.DefTail, SymbolName.End].Symbols, Is.Empty);
            Assert.That(table[SymbolName.Def, SymbolName.Identifier].Symbols, Is.EqualTo(new [] {SymbolName.Identifier, SymbolName.OpenBracket, SymbolName.Formals, SymbolName.CloseBracket, SymbolName.Colon, SymbolName.Type, }));
            Assert.That(table[SymbolName.Formals, SymbolName.CloseBracket].Symbols, Is.Empty);
            Assert.That(table[SymbolName.Formals, SymbolName.Identifier].Symbols, Is.EqualTo(new [] {SymbolName.NonEmptyFormals}));
            Assert.That(table[SymbolName.NonEmptyFormals, SymbolName.Identifier].Symbols, Is.EqualTo(new [] { SymbolName.Formal, SymbolName.FormalsTail, }));
            Assert.That(table[SymbolName.FormalsTail, SymbolName.Comma].Symbols, Is.EqualTo(new [] {SymbolName.Comma, SymbolName.Formal, SymbolName.FormalsTail, }));
            Assert.That(table[SymbolName.FormalsTail, SymbolName.CloseBracket].Symbols, Is.Empty);
            Assert.That(table[SymbolName.Formal, SymbolName.Identifier].Symbols, Is.EqualTo(new [] {SymbolName.Identifier, SymbolName.Colon, SymbolName.Type, }));
            Assert.That(table[SymbolName.Type, SymbolName.IntegerType].Symbols, Is.EqualTo(new [] {SymbolName.IntegerType}));
            Assert.That(table[SymbolName.Type, SymbolName.BooleanType].Symbols, Is.EqualTo(new [] {SymbolName.BooleanType}));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    /*
     Declaration Grammar

R1      <Program>             ::= <Def> <DefTail>                       
R2      <DefTail>             ::= <Def> <DefTail>
R3                              | ε                                     
R4      <Def>                 ::= <Identifier> MakeIdentifier ( <Formals> ) : <Type> MakeDefinition
R5      <Formals>             ::= ε
R6                              | <NonEmptyFormals>
R7      <NonEmptyFormals>     ::= <Formal><FormalTail>                                         
R8      <FormalTail>          ::= , <Formal><FormalTail>
R9                              | ε
R10     <Formal>              ::= <Identifier> MakeIdentifier : <Type> MakeFormal
R11     <Type>                ::= integer MakeType
R12                             | boolean MakeType
     */

    /*
    First and Follow

        First(Type)              = integer boolean
        First(Formal)            = identifier
        First(FormalTail)        = , ε
        First(NonEmptyFormals)   = First(Formal)
                                 = identifier
        First(Formals)           = ε First(NonEmptyFormals)
                                 = ε identifier
        First(Def)               = identifier
        First(DefTail)           = First(Def) ε
                                 = identifier ε
        First(Program)           = identifier

        Follow(Program)          = END
        Follow(DefTail)          = Follow(Program)
                                 = END
        Follow(Def)              = First(DefTail - ε) Follow(Program) Follow(DefTail)
                                 = identifier END
        Follow(Formals)          = ) 
        Follow(NonEmptyFormals)  = Follow(Formals)
                                 = )
        Follow(FormalTail)       = Follow(NonEmptyFormals)
                                 = )
        Follow(Formal)           = First(FormalTail - ε) Follow(NonEmptyFormals) Follow(FormalTail)
                                 = , )
        Follow(Type)             = Follow(Def) Follow(Formal)
                                 = identifier END , )
      */

    /* Parsing Table

      M[Program, identifier]        = R1
      M[DefTail, identifier]        = R2
      M[DefTail, END]               = R3
      M[Def, identifier]            = R4
      M[Formals, ) ]                = R5
      M[Formals, identifier]        = R6
      M[NonEmptyFormals, identifier]= R7
      M[FormalTail, ","]            = R8
      M[FormalTail, ) ]             = R9
      M[Formal, identifier]         = R10
      M[Type, integer]              = R11
      m[Type, boolean]              = R12 
     */

    public class DeclarationGrammarParsingTableFactory
    {
        private static Rule R1 => new Rule("R1", Symbol.Def, Symbol.DefTail);
        private static Rule R2 => new Rule("R2", Symbol.Def, Symbol.DefTail);
        private static Rule R3 => new Rule("R3");
        private static Rule R4 => new Rule("R4", Symbol.Identifier, Symbol.MakeIdentifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type, Symbol.MakeDefinition);
        private static Rule R5 => new Rule("R5");
        private static Rule R6 => new Rule("R6", Symbol.NonEmptyFormals);
        private static Rule R7 => new Rule("R7", Symbol.Formal, Symbol.FormalTail);
        private static Rule R8 => new Rule("R8", Symbol.Comma, Symbol.Formal, Symbol.FormalTail);
        private static Rule R9 => new Rule("R9");
        private static Rule R10 => new Rule("R10", Symbol.Identifier, Symbol.MakeIdentifier, Symbol.Colon, Symbol.Type, Symbol.MakeFormal);
        private static Rule R11 => new Rule("R11", Symbol.IntegerType, Symbol.MakeType);
        private static Rule R12 => new Rule("R12", Symbol.BooleanType, Symbol.MakeType);

        public static ParsingTable Create()
        {
            var parsingTable = new ParsingTable(Symbol.Program, Symbol.End);

            parsingTable.AddRule(R1, Symbol.Program, Symbol.Identifier);
            parsingTable.AddRule(R2, Symbol.DefTail, Symbol.Identifier);
            parsingTable.AddRule(R3, Symbol.DefTail, Symbol.End);
            parsingTable.AddRule(R4, Symbol.Def, Symbol.Identifier);
            parsingTable.AddRule(R5, Symbol.Formals, Symbol.CloseBracket);
            parsingTable.AddRule(R6, Symbol.Formals, Symbol.Identifier);
            parsingTable.AddRule(R7, Symbol.NonEmptyFormals, Symbol.Identifier);
            parsingTable.AddRule(R8, Symbol.FormalTail, Symbol.Comma);
            parsingTable.AddRule(R9, Symbol.FormalTail, Symbol.CloseBracket);
            parsingTable.AddRule(R10, Symbol.Formal, Symbol.Identifier);
            parsingTable.AddRule(R11, Symbol.Type, Symbol.IntegerType);
            parsingTable.AddRule(R12, Symbol.Type, Symbol.BooleanType);

            return parsingTable;
        }

    }

    [TestFixture]
    public class DeclarationGrammarTests
    {
        [Test]
        public void SimplestPossibleDefinition_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"main() : boolean";

            // act
            var parser = new Parser(DeclarationGrammarParsingTableFactory.Create()) { EnableStackTrace = true };
            var isValid = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(isValid, Is.True);
            Assert.That(parser.Ast, Is.AstEqual(new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType("boolean"),
                                                        formals: new  List<Formal>()
                                                    )));
        }

        [Test]
        public void Definition_WithOneFormal_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"main(arg1 : integer) : boolean";

            // act
            var parser = new Parser(DeclarationGrammarParsingTableFactory.Create()) { EnableStackTrace = true };
            var isValid = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(isValid, Is.True);
            Assert.That(parser.Ast, Is.AstEqual(new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType("boolean"),
                                                        formals: new List<Formal> { new Formal(new Identifier("arg1"), new KleinType("integer"))}
                                                    )));
        }

        [Test]
        public void Definition_WithTwoFormals_ShouldBeConstructedCorrectly()
        {
            // arrange
            var input = @"main(arg1 : integer, arg2 : boolean) : boolean";

            // act
            var parser = new Parser(DeclarationGrammarParsingTableFactory.Create()) { EnableStackTrace = true };
            var isValid = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(isValid, Is.True);
            Assert.That(parser.Ast, Is.AstEqual(new Definition
                                                    (
                                                        identifier: new Identifier("main"),
                                                        type: new KleinType("boolean"),
                                                        formals: new List<Formal>
                                                        {
                                                            new Formal(new Identifier("arg1"), new KleinType("integer")),
                                                            new Formal(new Identifier("arg2"), new KleinType("boolean")),
                                                        }
                                                    )));
        }
    }
}
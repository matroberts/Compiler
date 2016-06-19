using System;
using System.Linq;
using KleinCompiler;
using NUnit.Framework;

namespace KleinCompilerTests
{
    /* Arithmetic Grammar
        
R1      Expr                   := Term SimpleExprTail
R2      SimpleExprTail         := + Term SimpleExprTail MakePlus
R3                              | ε
R4      Term                   := Factor TermTail
R5      TermTail               := * Factor TermTail MakeTimes
R6                              | ε
R7      Factor                 := ( Expr )
R8                              | identifier MakeIdentifier 
    */

    /* First and Follow                           

        First(Factor)          =  ( identifier
        First(TermTail)        = * ε 
        First(Term)            = First(Factor) 
                               = ( identifier
        First(SimpleExprTail)  = + ε
        First(Expr)            = First(Term)
                               = ( identifier
                               
        Follow(Expr)           = END )
        Follow(SimpleExprTail) = Follow(Expr)
                               = END )
        Follow(Term)           = First(SimpleExprTail - ε) Follow(Expr) Follow(SimpleExprTail)
                               = + END )
        Follow(TermTail)       = Follow(Term)
                               = + END )
        Follow(Factor)         = First(TermTail - ε) Follow(Term) Follow(TermTail)
                               = * + END )

    */

    /* Parsing Table

        M[Expr, ( identifier]     = R1
        M[SimpleExprTail, + ]     = R2
        M[SimpleExprTail, END ) ] = R3
        M[Term, ( identifier]     = R4
        M[TermTail, * ]           = R5
        M[TermTail, + END ) ]     = R6
        M[Factor, ( ]             = R7
        M[Factor, identifier]     = R8 
      
     */

    public class ArithmeticGrammarParserTableFactory
    {
        private static Rule R1 => new Rule("R1", Symbol.Term, Symbol.SimpleExprTail);
        private static Rule R2 => new Rule("R2", Symbol.Plus, Symbol.Term, Symbol.SimpleExprTail, Symbol.MakePlus);
        private static Rule R3 => new Rule("R3");
        private static Rule R4 => new Rule("R4", Symbol.Factor, Symbol.TermTail);
        private static Rule R5 => new Rule("R5", Symbol.Factor, Symbol.TermTail, Symbol.MakeTimes);
        private static Rule R6 => new Rule("R6");
        private static Rule R7 => new Rule("R7", Symbol.OpenBracket, Symbol.Expr, Symbol.CloseBracket);
        private static Rule R8 => new Rule("R7", Symbol.Identifier, Symbol.MakeIdentifier);

        public static ParsingTable Create()
        {
            var parsingTable = new ParsingTable(Symbol.Expr, Symbol.End);

            parsingTable.AddRule(R1, Symbol.Expr, Symbol.OpenBracket, Symbol.Identifier);
            parsingTable.AddRule(R2, Symbol.SimpleExprTail, Symbol.Plus);
            parsingTable.AddRule(R3, Symbol.SimpleExprTail, Symbol.End, Symbol.CloseBracket);
            parsingTable.AddRule(R4, Symbol.Term, Symbol.OpenBracket, Symbol.Identifier);
            parsingTable.AddRule(R5, Symbol.TermTail, Symbol.Multiply);
            parsingTable.AddRule(R6, Symbol.TermTail, Symbol.Plus, Symbol.End, Symbol.CloseBracket);
            parsingTable.AddRule(R7, Symbol.Factor, Symbol.OpenBracket);
            parsingTable.AddRule(R8, Symbol.Factor, Symbol.Identifier);

            return parsingTable;
        }
    }

    [TestFixture]
    public class ArithmeticGrammarTests
    {
        [Test]
        public void ParserShould_ParseSlightlyMoreComplexProgram()
        {
            // arrange
            var input = @"x + y";

            // act
            var parser = new Parser(ArithmeticGrammarParserTableFactory.Create()) {EnableStackTrace = true};
            var isValid = parser.Parse(new Tokenizer(input));

            // assert
            Assert.That(isValid, Is.True, parser.StackTrace);
            Assert.That(parser.Ast, Is.EqualTo(new BinaryOperator()
                                                {
                                                    Left = new Identifier() {Value = "x"},
                                                    Operator = "+",
                                                    Right = new Identifier() {Value = "y"}
                                                }));

            //Console.WriteLine(parser.StackTrace);
            AstPrettyPrinter.ToConsole(parser.Ast);
        }
    }
}
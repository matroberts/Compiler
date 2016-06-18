using System;
using System.Linq;
using NUnit.Framework;

namespace KleinCompilerTests
{
    /*
        Arithmetic Grammar
        ==================
R1      Expr                   := Term SimpleExprTail
R2      SimpleExprTail         := + Term SimpleExprTail
R3                              | ε
R4      Term                   := Factor TermTail
R5      TermTail               := * Factor TermTail
R6                              | ε
R7      Factor                 := ( Expr )
R8                              | identifier   
                               


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

        M[Expr, ( identifier]     = R1
        M[SimpleExprTail, + ]     = R2
        M[SimpleExprTail, END ) ] = R3
        M[Term, ( identifier]     = R4
        M[TermTail, * ]           = R5
        M[TermTail, + END ) ]     = R6
        M[Factor, ( ]             = R7
        M[Ractor, identifier]     = R8 
      
     */
    [TestFixture]
    public class ArithmeticGrammarTests
    {
        [Test]
        public void Test()
        {
            
        }
    }
}
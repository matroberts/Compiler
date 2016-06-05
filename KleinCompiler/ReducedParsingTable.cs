using System;

namespace KleinCompiler
{
    /*
     * This is a reduced set of the Klien grammar, with only declarations
     * So can get started and test somthing quicker
     * 
R1     <Program>             ::= <Def> <DefTail>                                      
R2     <DefTail>             ::= <Def> <DefTail>
R3                             | e                                                    
R4     <Def>                 ::= <Identifier> ( <Formals> ) : <Type>                  
R5     <Formals>             ::= e
R6                             | <NonEmptyFormals>
R7     <NonEmptyFormals>     ::= <Formal> <FormalsTail>                                                             
R8     <FormalsTail>         ::= , <Formal><FormalsTail>
R9                             | e
R10    <Formal>              ::= < Identifier > : <Type>
R11    <Type>                ::= integer
R12                            | boolean

    First(Type)                = { integer boolean }
    First(Formal)              = { identifier } 
    First(FormalTail)          = { , e }
    First(NonEmptyFormals)     = { First(Formal) }
                               = identifier
    First(Formals)             = { e First(NonEmptyFormals) }
                               = e identifier
    First(Def)                 = { identifier }
    First(DefTail)             = { First(Def) e }
                               = identifier e
    First(Program)             = { First(Def) }
                               = identifier

    Follow(Program)            = { $ }
    Follow(DefTail)            = { Follow(Program) Follow(DefTail) }  
                               = { $ }
    Follow(Def)                = { First(DefTail - e) Follow(Program) Follow(DefTail) } 
                               = identifier $ 
    Follow(Formals)            = { ) }
    Follow(NonEmptyFormals)    = { Follow(Formals) }
                               = { ) }   
    Follow(FormalsTail)        = Follow(NonEmptyFormals) 
                               = { ) }
    Follow(Formal)             = { First(FormalsTail - e) Follow(NonEmptyFormals) Follow(FormalsTail) }
                               = { , ) }
    Follow(Type)               = { Follow(Def) Follow(Formal) }
                               = identifier $ , )

    
    M[Program, identifier]          = R1
    M[DefTail, identifier]          = R2
    M[DefTail, $ ]                  = R3
    M[Def, identifier]              = R4
    M[Formals, ) ]                  = R5
    M[Formals, identifier]          = R6
    M[NonEmptyFormals, identifier ] = R7
    M[FormalsTail, ","]             = R8
    M[FormalsTail, ) ]              = R9
    M[Formal, identifier]           = R10
    M[Type, integer]                = R11
    M[Type, boolean]                = R12
    */

    public interface IParsingTable
    {
        Rule this[Symbol symbol, Symbol token] { get; }
    }

    public class ReducedParsingTable : IParsingTable
    {
        private readonly Rule[,] table;
        public ReducedParsingTable()
        {
            int numberSymbols = Enum.GetNames(typeof(Symbol)).Length;
            table = new Rule[numberSymbols, numberSymbols];

            table[(int)Symbol.Program,          (int)Symbol.Identifier]      = new Rule("R1", Symbol.Def, Symbol.DefTail);
            table[(int)Symbol.DefTail,          (int)Symbol.Identifier]      = new Rule("R2", Symbol.Def, Symbol.DefTail);
            table[(int)Symbol.DefTail,          (int)Symbol.End]             = new Rule("R3" );
            table[(int)Symbol.Def,              (int)Symbol.Identifier]      = new Rule("R4", Symbol.Identifier, Symbol.OpenBracket, Symbol.Formals, Symbol.CloseBracket, Symbol.Colon, Symbol.Type);
            table[(int)Symbol.Formals,          (int)Symbol.CloseBracket]    = new Rule("R5" );
            table[(int)Symbol.Formals,          (int)Symbol.Identifier]      = new Rule("R6", Symbol.NonEmptyFormals);
            table[(int)Symbol.NonEmptyFormals,  (int)Symbol.Identifier]      = new Rule("R7", Symbol.Formal, Symbol.FormalsTail);
            table[(int)Symbol.FormalsTail,      (int)Symbol.Comma]           = new Rule("R8", Symbol.Comma, Symbol.Formal, Symbol.FormalsTail);
            table[(int)Symbol.FormalsTail,      (int)Symbol.CloseBracket]    = new Rule("R9" );
            table[(int)Symbol.Formal,           (int)Symbol.Identifier]      = new Rule("R10", Symbol.Identifier, Symbol.Colon, Symbol.Type);
            table[(int)Symbol.Type,             (int)Symbol.IntegerType]     = new Rule("R11", Symbol.IntegerType);
            table[(int)Symbol.Type,             (int)Symbol.BooleanType]     = new Rule("R12", Symbol.BooleanType);
        }

        public Rule this[Symbol symbol, Symbol token] => table[(int)symbol, (int)token];
    }
}
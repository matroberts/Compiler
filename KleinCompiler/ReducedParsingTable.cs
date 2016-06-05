using System;
using System.Collections.Generic;

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
    public class ReducedParsingTable
    {
        private readonly Rule[,] table;
        public ReducedParsingTable()
        {
            int numberSymbols = Enum.GetNames(typeof(SymbolName)).Length;
            table = new Rule[numberSymbols, numberSymbols];

            table[(int)SymbolName.Program,          (int)SymbolName.Identifier]      = new Rule("R1", SymbolName.Def, SymbolName.DefTail);
            table[(int)SymbolName.DefTail,          (int)SymbolName.Identifier]      = new Rule("R2", SymbolName.Def, SymbolName.DefTail);
            table[(int)SymbolName.DefTail,          (int)SymbolName.End]             = new Rule("R3" );
            table[(int)SymbolName.Def,              (int)SymbolName.Identifier]      = new Rule("R4", SymbolName.Identifier, SymbolName.OpenBracket, SymbolName.Formals, SymbolName.CloseBracket, SymbolName.Colon, SymbolName.Type); //< Identifier > ( < Formals > ) : < Type >
            table[(int)SymbolName.Formals,          (int)SymbolName.CloseBracket]    = new Rule("R5" );
            table[(int)SymbolName.Formals,          (int)SymbolName.Identifier]      = new Rule("R6", SymbolName.NonEmptyFormals);
            table[(int)SymbolName.NonEmptyFormals,  (int)SymbolName.Identifier]      = new Rule("R7", SymbolName.Formal, SymbolName.FormalsTail);
            table[(int)SymbolName.FormalsTail,      (int)SymbolName.Comma]           = new Rule("R8", SymbolName.Comma, SymbolName.Formal, SymbolName.FormalsTail);
            table[(int)SymbolName.FormalsTail,      (int)SymbolName.CloseBracket]    = new Rule("R9" );
            table[(int)SymbolName.Formal,           (int)SymbolName.Identifier]      = new Rule("R10", SymbolName.Identifier, SymbolName.Colon, SymbolName.Type);
            table[(int)SymbolName.Type,             (int)SymbolName.IntegerType]     = new Rule("R11", SymbolName.IntegerType);
            table[(int)SymbolName.Type,             (int)SymbolName.BooleanType]     = new Rule("R12", SymbolName.BooleanType);
        }

        public Rule this[SymbolName symbol, SymbolName token] => table[(int)symbol, (int)token];
    }

    public class Rule
    {
        public string Name { get; }
        public List<SymbolName> Symbols { get; } = new List<SymbolName>();
        public Rule(string name, params SymbolName[] symbols)
        {
            Name = name;
            Symbols.AddRange(symbols);
        }
    }
}
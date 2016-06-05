namespace KleinCompiler
{
    /*
     * This is a reduced set of the Klien grammar, with only declarations
     * So can get started and test somthing quicker
     * 
R1     <Program>             ::= <Def> <DefTail>                                            * left factored the original definitions rule
R2     <DefTail>             ::= <Def> <DefTail>
R3                             | e                                                        <-- here 'e' means no more tokens at all
R4     <Def>                 ::= <Identifier> ( <Formals> ) : <Type>                      <-- body remved from this rule, to truncate the grammar
R5     <Formals>             ::= e
R6                             | <NonEmptyFormals>
R7     <NonEmptyFormals>     ::= <Formal> <FormalsTail>                                     * left factored the original rule
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
        
    }
}
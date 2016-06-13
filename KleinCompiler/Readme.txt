KleinCompiler
=============

General
-------

1) Extensions class contains a bunch of helper methods
   which are used thoughout the project

2) FilePositionCalculator - calculates the linenumber and lineposition of a position in a string.
                          - used to calculate where an error occurs in a file.

Scanner
-------

1) Tokens are scanned by the Tokenizer class.

   The tokeniser class is constructed by passing it the input stream as a string
   var tokenizer = new Tokenizer("my input");

   Tokens are retrieved one at a time with the method
   public Token GetNextToken()

   When the stream is complete a special END token is returned

   The tokeniser is implemented as a series of procedural finite state machines
   in the StateMachine class.

2) Tokens all derive from a common base class, Token.
   Tokens have value equality.

3) Errors are signled by an ErrorToken,
   which is returned as part of the token stream.
   The error token also contains a message, which describes the error.

Parser
------

1) Parser - a file is parsed with the Parser class.

   bool Parse() is called to parse the input.

   Error information is retrived with the Error property.

   A stack trace of the parsing process can be retrived with the SymbolStackTrace property - useful for bebugging.

2) ParsingTable - The parser is table driven.  
                  Parsing table holds the parsing table for the klein grammar

   Rule this[Symbol symbol, Symbol token] - returns the rule for the given symbol and token
                                          - a null return indicates a parsing error

3) Symbol - terminal and nonterminal symbols of the klein grammar
    
4) Rule - representation of a grammar rule

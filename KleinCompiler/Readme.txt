KleinCompiler
=============

General
-------

1) Extensions class contains a bunch of helper methods
   which are used thoughout the project

Scanner
-------

1) Tokens are scanned by the Tokenizer class.

   The tokeniser class is constructed by passing it the input stream as a string
   var tokenizer = new Tokenizer("my input");

   Tokens are retrieved one at a time with the method
   public Token GetNextToken()

   The tokeniser is implemented as a series of procedural finite state machines
   in the StateMachine class.

2) Tokens all derive from a common base class, Token.
   Tokens have value equality.

3) Errors are signled by an ErrorToken,
   which is returned as part of the token stream.
   The error token also contains a message, which describes the error.

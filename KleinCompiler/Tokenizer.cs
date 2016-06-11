﻿using System;
using System.Collections.Generic;

namespace KleinCompiler
{
    /*
    Tokens
    ======
    LineComment                  - // continues to end of line
                                 - //.*\n
                                 - if a single / is encountered an error is produced

    BlockComment                 - { all the text within the curleys is comment }
                                 - {[^}]*}
                                 - if a block comment is not closed by the end of the file, an error is produced

    Identifier                   - Up to 256 characters case sensitive.  
                                 - Numbers are not allowed for the first character, but are allowed after that 
                                 - main is a primitive identifiers
                                 - [a-zA-Z][a-zA-Z0-9]*
    
    IntegerLiteral               - integers have the range -2^32 to 2^32-1, 
                                 - an integer literal is a string of digits.
                                 - there is no leading + or - to indicate sign, all integer literals are positive
                                 - leading zeros are not allowed for non-zero literals
                                 - 0 | [1-9][0-9]*      

    Keyword                        the keyword pattern is an exact match to the string of charcters

        BooleanTrue              - true
        BooleanFalse             - false
        IntegerType              - integer
        BooleanType              - boolean
        IfKeyword                - if
        ThenKeyword              - then
        ElseKeyword              - else
        NotOperator              - not
        OrOperator               - or
        AndOperator              - and
        PlusOperator             - +
        MinusOperator            - -
        MultiplicationOperator   - *
        DivisionOperator         - /
        LessThanOperator         - <
        EqualityOperator         - =
        OpenBracket              - (
        CloseBracket             - )
        Comma                    - ,
        Colon                    - :
        PrintKeyword             - print


    * If two tokens match the input the longer token is taken
    * If two tokens of the same length match the input, keywords are chosen in preference to identifiers

    */
    public class Tokenizer
    {
        private readonly string _input;
        private int _startPos;

        public Tokenizer(string input)
        {
            _input = input;
            _startPos = 0;
        }

        public Token GetNextToken()
        {
            while (_startPos < _input.Length)
            {
                Token token = null;

                var tokens = StateMachine.GetCandidateTokens(_input, _startPos);
                foreach (var t in tokens)
                {
                    if (token == null)
                        token = t;
                    if (t.Length > token.Length)
                        token = t;
                }

                if (token == null)
                {
                    _startPos++;  // advance over whitespace
                }
                else
                {
                    _startPos += token.Length;  // move start position to after recognised token
                    return token;
                }
            }
            return new Token(Symbol.End, "", _input.Length);
        }

        public Token Pop()
        {
            while (true)
            {
                var token = GetNextToken();
                if (token.Symbol == Symbol.LineComment || token.Symbol == Symbol.BlockComment)
                    continue;
                return token;
            }
        }

        public Token Peek()
        {
            var token = Pop();
            _startPos -= token.Length;
            return token;
        }
    }

    public class StateMachine
    {
        public static List<Token> GetCandidateTokens(string input, int startPos)
        {
            var tokens = new List<Token>();
            tokens
                .AddIfNotNull(GetKeyword(Symbol.IntegerType, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.BooleanType, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.If, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Then, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Else, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Not, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Or, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.And, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.BooleanTrue, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.BooleanFalse, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Plus, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Minus, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Multiply, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Divide, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.LessThan, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Equality, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.OpenBracket, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.CloseBracket, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Comma, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.Colon, input, startPos, startPos))
                .AddIfNotNull(GetKeyword(Symbol.PrintKeyword, input, startPos, startPos))
                .AddIfNotNull(GetIdentifier(input, startPos))
                .AddIfNotNull(GetIntegerLiteral(input, startPos))
                .AddIfNotNull(GetLineComment(input, startPos))
                .AddIfNotNull(GetBlockComment(input, startPos));

            if(tokens.Count == 0 && input[startPos].IsWhitespace()==false)
                tokens.Add(new ErrorToken(input.Substring(startPos, 1), startPos, $"Unknown character '{input[startPos]}'"));

            return tokens;
        }

        private static Token GetIdentifier(string input, int startPos)
        {
            var token = GetIdentifier1(input, startPos);
            if (token == null)
                return null;
            else if(token.Value.Length>256)
                return new ErrorToken(token.Value, token.Position, "Max length of an identifier is 256 characters");
            else
                return token;
        }

        private static Token GetIdentifier1(string input, int startPos)
        {
            if (input[startPos].IsAlpha())
                return GetIdentifier2(input, startPos, startPos + 1);
            return null;
        }

        private static Token GetIdentifier2(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new Token(Symbol.Identifier, input.Substring(startPos, pos-startPos), startPos);
            else if (input[pos].IsAlpha() || input[pos].IsNumeric())
                return GetIdentifier2(input, startPos, pos + 1);
            return new Token(Symbol.Identifier, input.Substring(startPos, pos-startPos), startPos);
        }

        private static Token GetKeyword(Symbol name, string input, int startPos, int pos)
        {
            string keyword = name.ToKeyword();
            if (pos >= input.Length)
                return null;
            if (input[pos] == keyword[pos - startPos])
            {
                if (keyword.Length == pos - startPos + 1)
                {
                    return new Token(name, input.Substring(startPos, pos - startPos + 1), startPos);
                }
                else
                {
                    return GetKeyword(name, input, startPos, pos + 1);
                }
            }
            else
            {
                return null;
            }
        }

        private static Token GetLineComment(string input, int startPos)
        {
            if (input[startPos] == '/')
                return GetLineComment1(input, startPos, startPos + 1);
            return null;
        }

        private static Token GetLineComment1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return null;
            if (input[pos] == '/')
                return GetLineComment2(input, startPos, pos + 1);
            return null;
        }

        private static Token GetLineComment2(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new Token(Symbol.LineComment, input.Substring(startPos, pos-startPos), startPos);
            if (input[pos] == '\n')
                return new Token(Symbol.LineComment, input.Substring(startPos, pos - startPos).TrimEnd('\r', '\n'), startPos);
            return GetLineComment2(input, startPos, pos+1);
        }

        private static Token GetBlockComment(string input, int startPos)
        {
            if (input[startPos] == '{')
                return GetBlockComment1(input, startPos, startPos + 1);
            return null;
        }

        private static Token GetBlockComment1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new ErrorToken(input.Substring(startPos, pos - startPos), startPos, "missing } in block comment"); // malformed block comment with no closing }
            if (input[pos] == '}')
                return new Token(Symbol.BlockComment, input.Substring(startPos, pos-startPos+1), startPos);
            return GetBlockComment1(input, startPos, pos+1);
        }

        private static Token GetIntegerLiteral(string input, int startPos)
        {
            var token = GetIntegerLiteral1(input, startPos);
            if (token == null)
                return null;

            if(token.Value.StartsWith("0") && token.Length >1)
                return new ErrorToken(token.Value, token.Position, "Number literals are not allowed leading zeros");

            // this seems a bit bizarre
            // the max size of interger literal according to the spec is 2^32-1
            // but this is the size of a c# uint, not a c# int
            uint result = 0;
            if(!uint.TryParse(token.Value, out result))
                return new ErrorToken(token.Value, token.Position, "Maximum size of integer literal is 4294967295");

            return token;
        }

        private static Token GetIntegerLiteral1(string input, int startPos)
        {
            if (input[startPos].IsNumeric())
                return GetIntegerLiteral2(input, startPos, startPos + 1);
            return null;
        }

        private static Token GetIntegerLiteral2(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new Token(Symbol.IntegerLiteral, input.Substring(startPos, pos - startPos), startPos);
            if (input[pos].IsNumeric())
                return GetIntegerLiteral2(input, startPos, pos + 1);
            return new Token(Symbol.IntegerLiteral, input.Substring(startPos, pos-startPos), startPos);
        }
    }
}
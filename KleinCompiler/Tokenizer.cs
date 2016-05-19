using System;
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

    Identifier                   - Up to 256 characters case sensitive.  Numbers are not allowed for the first character, but are allowed after that 
                                 - main and print are primitive identifiers
                                 - [a-zA-Z][a-zA-Z0-9]*
    
    IntegerLiteral               - integers have the range -2^32 to 2^32-1, 
                                 - but in the tokenizer ignore the minus sign and have only positive literals
                                 - treat the minus always as a MinusOperator 
                                 - [0-9]+      

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
            return null; // return null when no more tokens
        }
    }

    public class StateMachine
    {
        public static List<Token> GetCandidateTokens(string input, int startPos)
        {
            var tokens = new List<Token>();
            tokens
                .AddIfNotNull(KeywordState0("integer", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("boolean", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("if", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("then", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("else", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("not", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("or", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("and", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("true", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("false", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("+", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("-", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("*", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("/", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("<", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("=", input, startPos, startPos))
                .AddIfNotNull(KeywordState0("(", input, startPos, startPos))
                .AddIfNotNull(KeywordState0(")", input, startPos, startPos))
                .AddIfNotNull(KeywordState0(",", input, startPos, startPos))
                .AddIfNotNull(KeywordState0(":", input, startPos, startPos))
                .AddIfNotNull(IdentifierState0(input, startPos))
                .AddIfNotNull(IntegerLiteralState0(input, startPos))
                .AddIfNotNull(LineCommentState0(input, startPos))
                .AddIfNotNull(BlockCommentState0(input, startPos));

            if(tokens.Count == 0 && input[startPos].IsWhitespace()==false)
                tokens.Add(new ErrorToken(input.Substring(startPos, 1), startPos, $"Unknown character '{input[startPos]}'"));

            return tokens;
        }

        private static Token IdentifierState0(string input, int startPos)
        {
            if (input[startPos].IsAlpha())
                return IdentifierState1(input, startPos, startPos + 1);
            return null;
        }

        private static Token IdentifierState1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new IdentifierToken(input.Substring(startPos, pos-startPos), startPos);
            else if (input[pos].IsAlpha() || input[pos].IsNumeric())
                return IdentifierState1(input, startPos, pos + 1);
            return new IdentifierToken(input.Substring(startPos, pos-startPos), startPos);
        }

        private static Token KeywordState0(string keyword, string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return null;
            if (input[pos] == keyword[pos - startPos])
            {
                if (keyword.Length == pos - startPos + 1)
                {
                    return new KeywordToken(input.Substring(startPos, pos - startPos + 1), startPos);
                }
                else
                {
                    return KeywordState0(keyword, input, startPos, pos + 1);
                }
            }
            else
            {
                return null;
            }
        }

        private static Token LineCommentState0(string input, int startPos)
        {
            if (input[startPos] == '/')
                return LineCommentState1(input, startPos, startPos + 1);
            return null;
        }

        private static Token LineCommentState1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return null;
            if (input[pos] == '/')
                return LineCommentState2(input, startPos, pos + 1);
            return null;
        }

        private static Token LineCommentState2(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new LineCommentToken(input.Substring(startPos, pos-startPos), startPos);
            if (input[pos] == '\n')
                return new LineCommentToken(input.Substring(startPos, pos - startPos).TrimEnd('\r', '\n'), startPos);
            return LineCommentState2(input, startPos, pos+1);
        }

        private static Token BlockCommentState0(string input, int startPos)
        {
            if (input[startPos] == '{')
                return BlockCommentState1(input, startPos, startPos + 1);
            return null;
        }

        private static Token BlockCommentState1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new ErrorToken(input.Substring(startPos, pos - startPos), startPos, "missing } in block comment"); // malformed block comment with no closing }
            if (input[pos] == '}')
                return new BlockCommentToken(input.Substring(startPos, pos-startPos+1), startPos);
            return BlockCommentState1(input, startPos, pos+1);
        }

        private static Token IntegerLiteralState0(string input, int startPos)
        {
            if (input[startPos].IsNumeric())
                return IntegerLiteralState1(input, startPos, startPos + 1);
            return null;
        }

        private static Token IntegerLiteralState1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new IntegerLiteralToken(input.Substring(startPos, pos - startPos), startPos);
            if (input[pos].IsNumeric())
                return IntegerLiteralState1(input, startPos, pos + 1);
            return new IntegerLiteralToken(input.Substring(startPos, pos-startPos), startPos);
        }
    }
}
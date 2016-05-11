using System;
using System.Collections.Generic;

namespace KleinCompiler
{
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
                Token token;
                token = StateMachine.KeywordState0(_input, _startPos);
                if(token == null)  // this is wrong
                    token = StateMachine.IdentifierState0(_input, _startPos);

                if (token == null)
                {
                    _startPos++; // advance over unknown token
                }
                else
                {
                    _startPos += token.Lenth;  // move start position to after recognised token
                    return token;
                }
            }
            return null; // return null when no more tokens
        }
    }

    public class StateMachine
    {
        // identifier [A-Za-z]+
        // (0 IsAlpha) -> (1 IsAlpha) <>
        public static Token IdentifierState0(string input, int startPos)
        {
            if (input[startPos].IsAlpha())
                return IdentifierState1(input, startPos, startPos + 1);
            return null;
        }

        private static Token IdentifierState1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new IdentifierToken(input.Substring(startPos, pos-startPos));
            else if (input[pos].IsAlpha())
                return IdentifierState1(input, startPos, pos + 1);
            return new IdentifierToken(input.Substring(startPos, pos-startPos));
        }

        public static Token KeywordState0(string input, int startPos)
        {
            string keyword = "integer";
            if (input[startPos] == keyword[0])
                return KeywordState1(keyword, input, startPos, startPos + 1);
            return null;
        }

        private static Token KeywordState1(string keyword, string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return null;
            if (input[pos] == keyword[pos - startPos])
            {
                if (keyword.Length == pos - startPos + 1)
                {
                    return new KeywordToken(input.Substring(startPos, pos - startPos + 1));
                }
                else
                {
                    return KeywordState1(keyword, input, startPos, pos + 1);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
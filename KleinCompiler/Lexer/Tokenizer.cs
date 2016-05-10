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
                var token = StateMachine.IdentifierState0(_input, _startPos);

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

        public static Token IdentifierState1(string input, int startPos, int pos)
        {
            if (pos >= input.Length)
                return new IdentifierToken(input.Substring(startPos, pos-startPos));
            else if (input[pos].IsAlpha())
                return IdentifierState1(input, startPos, pos + 1);
            return new IdentifierToken(input.Substring(startPos, pos-startPos));
        }
    }
}
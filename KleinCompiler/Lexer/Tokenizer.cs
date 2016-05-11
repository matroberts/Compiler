﻿using System;
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
                    _startPos++; // advance over unknown token
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
                  .AddIfNotNull(KeywordState0(input, startPos))
                  .AddIfNotNull(IdentifierState0(input, startPos));
            return tokens;
        }

        // identifier [A-Za-z]+
        // (0 IsAlpha) -> (1 IsAlpha) <>
        private static Token IdentifierState0(string input, int startPos)
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

        private static Token KeywordState0(string input, int startPos)
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
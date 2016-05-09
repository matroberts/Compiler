using System;
using System.Collections.Generic;

namespace KleinCompiler
{
    public class Tokenizer
    {
        public List<Token> GetTokens(string input)
        {
            var tokens = new List<Token>();
            // identifier [A-Za-z]+
            var token = State0(input, 0);
            if(token != null)
                tokens.Add(token);
            return tokens;
        }

        public static Token State0(string input, int pos)
        {
            if (pos >= input.Length)
                return null;
            else if (input[pos].IsAlpha())
                return State1(input, pos+1);
            throw new ArgumentException();
//            return null;
        }

        public static Token State1(string input, int pos)
        {
            if(pos >= input.Length)
                return new IdentifierToken(input);
            else if (input[pos].IsAlpha())
                return State1(input, pos+1);
            throw new ArgumentException();
//            else
//                return new IdentifierToken(input.Substring(0, pos - 1));
        }
    }
}
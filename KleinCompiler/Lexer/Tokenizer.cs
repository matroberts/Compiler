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
            // identifier [A-Za-z]+
            var token = StateMachine.State0(_input, 0);
            return token;
        }


    }

    public class StateMachine
    {
        public static Token State0(string input, int pos)
        {
            if (pos >= input.Length)
                return null;
            else if (input[pos].IsAlpha())
                return State1(input, pos + 1);
            throw new ArgumentException();
            //            return null;
        }

        public static Token State1(string input, int pos)
        {
            if (pos >= input.Length)
                return new IdentifierToken(input);
            else if (input[pos].IsAlpha())
                return State1(input, pos + 1);
            throw new ArgumentException();
            //            else
            //                return new IdentifierToken(input.Substring(0, pos - 1));
        }
    }
}
using System;
using System.Collections.Generic;
using KleinCompiler;

public static class Extensions
{
    public static bool IsAlpha(this char ch)
    {
        return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
    }

    public static bool IsNumeric(this char ch)
    {
        return (ch >= '0' && ch <= '9');
    }

    public static bool IsWhitespace(this char ch)
    {
        return char.IsWhiteSpace(ch);
    }

    public static List<Token> AddIfNotNull(this List<Token> list, Token token)
    {
        if (token != null)
            list.Add(token);

        return list;
    }

    public static string TruncateWithElipses(this string text, int length)
    {
        if (length <= 3)
            throw new ArgumentException("TruncateWithElipses must be called with length 4 or more");
        if (text.Length > length)
        {
            return text.Substring(0, length - 3) + "...";
        }
        return text;
    }
}

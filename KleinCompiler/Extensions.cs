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
}

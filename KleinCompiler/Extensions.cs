using System.Collections.Generic;
using KleinCompiler;

public static class Extensions
{
    public static bool IsAlpha(this char ch)
    {
        return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
    }

    public static List<Token> AddIfNotNull(this List<Token> list, Token token)
    {
        if (token != null)
            list.Add(token);

        return list;
    } 
}

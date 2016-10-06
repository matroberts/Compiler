using System;
using System.Collections.Generic;
using System.Text;
using KleinCompiler;
using KleinCompiler.BackEndCode;
using KleinCompiler.FrontEndCode;

[AttributeUsage(AttributeTargets.Field)]
public class SymbolTypeAttribute : Attribute
{
    public SymbolType SymbolType { get; }

    public SymbolTypeAttribute(SymbolType symbolType)
    {
        SymbolType = symbolType;
    }
}

public static class Extensions
{
    public static SymbolType ToSymbolType(this Enum symbolName)
    {
        var attributes = symbolName.GetType().GetField(symbolName.ToString()).GetCustomAttributes(typeof(SymbolTypeAttribute), false);
        if (attributes.Length > 0)
            return ((SymbolTypeAttribute)attributes[0]).SymbolType;
        throw new ArgumentException($"Add SymbolTypeAttribute to {symbolName.GetType().Name}.{symbolName} if you want to call ToSymbolType().");
    }

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

    public static string PadAndTruncate(this string text, int length)
    {
        if (text.Length > length)
        {
            return text.Substring(0, length);
        }
        return text.PadRight(length);
    }

    public static void Push(this Stack<Symbol> stack, IEnumerable<Symbol> symbols)
    {
        foreach (var symbol in symbols)
        {
            stack.Push(symbol);
        }
    }

    public static void ToConsole(this string[] strings)
    {
        foreach (var s in strings)
        {
            Console.WriteLine(s);
        }
    }
}

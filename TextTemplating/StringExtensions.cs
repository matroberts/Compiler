using System;

namespace TextTemplating
{
    public static class StringExtensions
    {
        public static string TruncateWithElipses(this string text, int length)
        {
            if(length <= 3)
                throw new ArgumentException("TruncateWithElipses must be called with length 4 or more");
            if (text.Length > length)
            {
                return text.Substring(0, length-3) + "...";
            }
            return text;
        }
    }
}
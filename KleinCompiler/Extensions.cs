public static class Extensions
{
    public static bool IsAlpha(this char ch)
    {
        return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
    }
}

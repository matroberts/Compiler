namespace KleinCompiler
{
    public class Token
    {
        public string Value { get; set; }
    }

    public class IdentifierToken : Token
    {
        public IdentifierToken(string value)
        {
            Value = value;
        }
    }

    public class UnknownToken : Token
    {
        public UnknownToken(string value)
        {
            Value = value;
        }
    }
}
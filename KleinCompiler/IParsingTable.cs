namespace KleinCompiler
{
    public interface IParsingTable
    {
        Rule this[Symbol symbol, Symbol token] { get; }
    }
}
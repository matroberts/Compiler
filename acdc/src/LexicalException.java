public class LexicalException extends Exception
{
    public LexicalException( String s )
    {
        super( s );
    }

    public String toString()
    {
        return "LEXICAL EXCEPTION -- " + super.toString();
    }
}

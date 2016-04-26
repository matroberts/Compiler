public class SemanticException extends Exception
{
    public SemanticException( String s )
    {
        super( s );
    }

    public String toString()
    {
        return "SEMANTIC EXCEPTION -- " + super.toString();
    }
}

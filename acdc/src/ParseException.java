public class ParseException extends Exception
{
    public ParseException( String s )
    {
        super( s );
    }

    public String toString()
    {
        return "PARSE EXCEPTION -- " + super.toString();
    }
}

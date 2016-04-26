public class CodeGenerationException extends Exception
{
    public CodeGenerationException( String s )
    {
        super( s );
    }

    public String toString()
    {
        return "CODE GENERATION EXCEPTION -- " + super.toString();
    }
}

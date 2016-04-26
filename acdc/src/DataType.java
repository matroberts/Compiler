
// This class is an example of a type-safe enum in Java.

public class DataType
{
    public static final DataType Float   = new DataType( "float"   );
    public static final DataType Integer = new DataType( "integer" );

    private String typeName;

    private DataType( String name )
    {
        typeName = name;
    }

    public String toString()
    {
        return typeName;
    }
}

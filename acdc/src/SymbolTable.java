import java.util.HashMap;

public class SymbolTable
{
    private HashMap table;

    public SymbolTable()
    {
         table = new HashMap( 23, 1 ); // one for each possible identifier
    }

    public boolean contains( char id )
    {
        return table.containsKey( new Character(id) );
    }

    public DataType lookup( char id ) throws SemanticException
    {
        if ( !contains(id) )
            throw new SemanticException( "Identifier not found: " + id );
        return (DataType) table.get( new Character(id) );
    }

    public void add( char id, DataType type ) throws SemanticException
    {
        if ( contains(id) )
            throw new SemanticException( "Identifier already exists: " +  id );
        table.put( new Character(id), type );
    }

    public String toString()
    {
        return table.toString();
    }
}

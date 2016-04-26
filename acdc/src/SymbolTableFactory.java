public class SymbolTableFactory
{
    private Program program;

    public SymbolTableFactory( Program ast )
    {
        program = ast;
    }

    public SymbolTable build() throws SemanticException
    {
        SymbolTable t = new SymbolTable();
        Declaration d = program.declarations();

        while ( d != null )
        {
            Declaration current     = d.first();
            DataType    currentType = null;
            if (current instanceof FloatDeclaration)
                currentType = DataType.Float;
            else
                if (current instanceof IntegerDeclaration)
                    currentType = DataType.Integer;
                else
                    throw new SemanticException( "invalid type declaration" );
            t.add( current.identifier().value(), currentType );
            d = d.rest();
        }

        return t;
    }
}

public class TypeChecker
{
    private SymbolTable table;

    public TypeChecker( SymbolTable t )
    {
        table = t;
    }

    public void check( Program program ) throws SemanticException
    {
        Statement statements = program.statements();
        while ( statements != null )
        {
            checkStatement( statements.first() );
            statements = statements.rest();
        }
    }

    protected void checkStatement( Statement statement )
                   throws SemanticException
    {
        if ( statement instanceof AssignmentStatement )
        {
            AssignmentStatement s = (AssignmentStatement) statement;
            checkExpression( s.value() );

            Identifier id   = s.identifier();
            DataType   type = table.lookup(id.value());

            id.setType( type );
            s.setValue( convertToType( s.value(), id.type() ));
        }
        else if ( statement instanceof PrintStatement )
        {
            PrintStatement s = (PrintStatement) statement;
            Identifier id = s.identifier();
            id.setType( table.lookup(id.value()) );
        }
        else
            throw new SemanticException(
                      "type checker expected assignment or print statement" );
    }

    public void checkExpression( Value value ) throws SemanticException
    {
        if ( value instanceof Identifier )
        {
            Identifier id = (Identifier) value;
            value.setType( table.lookup(id.value()) );
        }
        else if ( value instanceof FloatValue )
            value.setType( DataType.Float );
        else if ( value instanceof IntegerValue )
            value.setType( DataType.Integer );
        else if ( value instanceof ExpressionTail )
        {
            ExpressionTail exp   = (ExpressionTail) value;

            Value left  = exp.leftOperand ();
            Value right = exp.rightOperand();

            checkExpression( left );
            checkExpression( right );

            DataType type = generalize( left, right );
            exp.setLeftOperand ( convertToType( left,  type ) );
            exp.setRightOperand( convertToType( right, type ) );
            exp.setType( type );
        }
        else
            throw new SemanticException(
                      "type checker found unhandled type of expression" );
    }

    protected DataType generalize( Value value1, Value value2 )
    {
        if ( value1.type().equals(DataType.Float) ||
             value2.type().equals(DataType.Float) )
            return DataType.Float;
        return DataType.Integer;
    }

    protected Value convertToType( Value value, DataType newType )
                    throws SemanticException
    {
        DataType valueType = value.type();
        if ( valueType.equals(DataType.Float  ) &&
               newType.equals(DataType.Integer) )
            throw new SemanticException( "cannot convert float to int" );
        if ( valueType.equals(DataType.Integer) &&
               newType.equals(DataType.Float  ) )
            return new IntToFloatConversion( value );
        return value;
    }
}

public class AbstractSyntaxTreeFactory
{
    public static AbstractSyntaxTree makeProgram(
                                         AbstractSyntaxTree declarations,
                                         AbstractSyntaxTree statements  )
    {
        return new Program( (Declaration) declarations,
                            (Statement)   statements );
    }

    public static AbstractSyntaxTree makeDeclarations(
                                         AbstractSyntaxTree declaration,
                                         AbstractSyntaxTree rest)
    {
        return new Declaration( (Declaration) declaration,
                                (Declaration) rest );
    }

    public static AbstractSyntaxTree makeStatements(
                                         AbstractSyntaxTree statement,
                                         AbstractSyntaxTree rest )
    {
        return new Statement( (Statement) statement, (Statement) rest );
    }

    public static AbstractSyntaxTree makeTree(Token token)
                                     throws ParseException
    {
        if ( token.type() == Token.Identifier )
            return new Identifier( token.cvalue() );
        if ( token.type() == Token.FloatValue )
            return new FloatValue( token.fvalue() );
        if ( token.type() == Token.IntegerValue )
            return new IntegerValue( token.ivalue() );
        throw new ParseException( "Expected identifier or number, saw " +
                                  token );
    }

    public static AbstractSyntaxTree makeTree( Token token, Token value )
                                     throws ParseException
    {
        if ( token.type() == Token.FloatDeclaration )
            return new FloatDeclaration( value );
        if ( token.type() == Token.IntegerDeclaration )
            return new IntegerDeclaration( value );
        if ( token.type() == Token.PrintOp )
            return new PrintStatement( value );
        throw new ParseException(
                     "Expected identifier or print statement, saw " +
                     token );
    }

    public static AbstractSyntaxTree makeTree( Token              operation,
                                               AbstractSyntaxTree v,
                                               AbstractSyntaxTree tail )
                                     throws ParseException
    {
        Value          value = (Value) v;
        ExpressionTail rest  = (ExpressionTail) tail;

        switch ( operation.type() )
        {
            case Token.PlusOp:
                if ( rest == null )
                    return new PlusOperation( value );
                else
                {
                    rest.addLeftOperand( value );
                    return new PlusOperation( rest ); 
                }
            case Token.MinusOp:
                if ( rest == null )
                    return new MinusOperation( value );
                else
                {
                    rest.addLeftOperand( value );
                    return new MinusOperation( rest );
                }
            default:
                throw new ParseException( "Expected arithmetic operator, saw "
                                        + operation );
        }
    }

    public static AbstractSyntaxTree makeTree( Token token, Token identifier,
                                               AbstractSyntaxTree v,
                                               AbstractSyntaxTree tail )
                                     throws ParseException
    {
        Value          value = (Value) v;
        ExpressionTail rest  = (ExpressionTail) tail;

        if ( token.type() == Token.AssignmentOp )
	    if ( rest == null )
                return new AssignmentStatement( identifier, value );
            else
            {
                rest.addLeftOperand( value );
                return new AssignmentStatement( identifier, rest ); 
            }
        else throw new ParseException( "Expected assignment operator, saw " +
                                       token );
    }
}

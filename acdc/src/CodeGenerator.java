import java.io.PrintStream;

public class CodeGenerator
{
    private PrintStream target;

    public CodeGenerator( PrintStream t )
    {
        target = t;
    }

    public void generate( AbstractSyntaxTree ast )
                throws CodeGenerationException
    {
        Program program = (Program) ast;

        Statement statements = program.statements();
        while ( statements != null )
        {
            generateStatement( statements.first() );
            statements = statements.rest();
        }
    }

    protected void generateStatement( Statement statement )
                   throws CodeGenerationException
    {
        if ( statement instanceof AssignmentStatement )
        {
            AssignmentStatement s = (AssignmentStatement) statement;

            generateExpression( s.value() );
            emitRegisterCode( "s" );
            emitCode( String.valueOf( s.identifier().value() ));
            emitCode( "0 k" );
        }
        else if ( statement instanceof PrintStatement )
        {
            PrintStatement s = (PrintStatement) statement;

            generateExpression( s.identifier() );
            emitCode( "p" );
            emitCode( "si" );
        }
        else
            throw new CodeGenerationException(
                  "code generator expected assignment or print statement" );
    }

    protected void generateExpression( Value value )
                   throws CodeGenerationException
    {
        if ( value instanceof Identifier )
        {
            Identifier id = (Identifier) value;

            emitRegisterCode( "l" );
            emitCode( String.valueOf( id.value() ));
        }
        else if ( value instanceof FloatValue )
        {
            FloatValue f = (FloatValue) value;

            emitCode( String.valueOf( f.value() ));
        }
        else if ( value instanceof IntegerValue )
        {
            IntegerValue i = (IntegerValue) value;

            emitCode( Integer.toString( i.value() ));
        }
        else if ( value instanceof ExpressionTail )
        {
            ExpressionTail exp   = (ExpressionTail) value;

            generateExpression(exp.leftOperand ());
            generateExpression(exp.rightOperand());

            if (value instanceof PlusOperation)
                emitCode( "+" );
            else if (value instanceof MinusOperation)
                emitCode( "-" );
            else
                throw new CodeGenerationException(
                     "code generator found an invalid arithmetic operation" );
        }
        else if ( value instanceof IntToFloatConversion )
        {
            IntToFloatConversion converted = (IntToFloatConversion) value;

            generateExpression( converted.value() );
            emitCode( "5 k" );
        }
        else
            throw new CodeGenerationException(
                      "code generator found unhandled type of expression" );
    }

    protected void emitCode( String newCode )
    {
        target.println( newCode );
    }

    protected void emitRegisterCode( String newCode )
    {
        target.print( newCode );
    }
}

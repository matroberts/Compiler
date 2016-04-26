import java.io.IOException;
import java.io.PushbackInputStream;

public class ParserTest
{
    public static void main( String[] args )
    {
        Token token = null;

        Parser parser = new Parser(
                            new Scanner(
                                new PushbackInputStream(System.in)));
        AbstractSyntaxTree ast = parser.parse();
	showAST( ast, 0 );
    }

    public static void showAST( AbstractSyntaxTree ast, int indent )
    {
        if (ast instanceof Program)
        {
            Program p = (Program) ast;
            showSpaces( indent );
            System.out.println( "PROGRAM" );
            showAST( p.declarations(), indent+5 );
            showAST( p.statements  (), indent+5 );
        }
        else if (ast instanceof Declaration)
        {
            Declaration d = (Declaration) ast;
            showSpaces( indent );
            System.out.print( "DECLARATION  " );
            showSpaces( indent );
            if (d.first() instanceof FloatDeclaration)
                System.out.println( "FLOAT " +
                           ((FloatDeclaration) d.first()).identifier() );
            else
                System.out.println( "INT   " +
                           ((IntegerDeclaration) d.first()).identifier() );
            if (d.rest() != null)
                showAST( d.rest(), indent );
        }
        else if (ast instanceof Statement)
        {
            Statement s = (Statement) ast;
            showSpaces( indent );
            System.out.print( "STATEMENT    " );
            showSpaces( indent );
            if (s.first() instanceof PrintStatement)
                System.out.println( "PRINT  " +
                           ((PrintStatement) s.first()).identifier() );
            else
            {
                System.out.print( "ASSIGN " +
                           ((AssignmentStatement) s.first()).identifier() );
                showAST( ((AssignmentStatement) s.first()).value(), 1 );
            }
            if (s.rest() != null)
                showAST( s.rest(), indent );
        }
        else if (ast instanceof Identifier)
        {
            showSpaces( indent );
            System.out.println( ((Identifier) ast).value() );
        }
        else if (ast instanceof FloatValue)
        {
            showSpaces( indent );
            System.out.println( ((FloatValue) ast).value() );
        }
        else if (ast instanceof IntegerValue)
        {
            showSpaces( indent );
            System.out.println( ((IntegerValue) ast).value() );
        }
        else if (ast instanceof PlusOperation)
        {
            PlusOperation operation = (PlusOperation) ast;
            showSpaces( indent );
            showAST( operation.leftOperand(), 0 );
            System.out.println( " + " );
            showAST( operation.rightOperand(), 0 );
        }
        else if (ast instanceof MinusOperation)
        {
            MinusOperation operation = (MinusOperation) ast;
            showSpaces( indent );
            showAST( operation.leftOperand(), 0 );
            System.out.println( " - " );
            showAST( operation.rightOperand(), 0 );
        }
    }

    public static void showSpaces( int count )
    {
        for (int i = 0; i < count; i++)
            System.out.print( " " );
    }
}

import java.io.IOException;
import java.io.PushbackInputStream;

public class TypeCheckerTest
{
    public static void main( String[] args )
    {
        Parser parser = new Parser(
                            new Scanner(
                                new PushbackInputStream(System.in)));
        AbstractSyntaxTree ast = parser.parse();
        SymbolTableFactory stf = new SymbolTableFactory( (Program) ast );

        try {
            SymbolTable table = stf.build();
            TypeChecker tc    = new TypeChecker( table );
            tc.check( (Program) ast );
            System.out.println( ast );
        } catch (SemanticException e) {
            System.out.println( e );
        }
    }
}

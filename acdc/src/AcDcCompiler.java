import java.io.PushbackInputStream;
import java.io.PrintStream;

public class AcDcCompiler
{
    PushbackInputStream source;
    PrintStream         target;

    public AcDcCompiler( PushbackInputStream s, PrintStream t )
    {
        source = s;
        target = t;
    }

    public void run()
    {
        Parser parser = new Parser( new Scanner(source) );
        AbstractSyntaxTree ast = parser.parse();
        SymbolTableFactory stf = new SymbolTableFactory( (Program) ast );

        try {
            SymbolTable table = stf.build();
            TypeChecker tc    = new TypeChecker( table );

            tc.check( (Program) ast );

            CodeGenerator generator = new CodeGenerator( target );
            generator.generate( ast );
        } catch (Exception e) {
            System.out.println( e );
        }
    }
}

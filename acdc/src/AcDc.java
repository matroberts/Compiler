import java.io.PushbackInputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.PrintStream;
import java.io.IOException;

public class AcDc
{
    public static void main( String[] args )
    {
        if (args.length > 2)
        {
            System.out.println("Usage: java AcDc [ input [output] ]");
            System.exit( -1 );
        }

        PushbackInputStream source = makeInputStream (args);
        PrintStream         target = makeOutputStream(args);

        AcDcCompiler compiler = new AcDcCompiler( source, target );
        compiler.run();
    }

    private static PushbackInputStream makeInputStream( String[] args )
    {
        try {
            if (args.length >= 1)
                return new PushbackInputStream(new FileInputStream(args[0]));
            return new PushbackInputStream(System.in);
	} catch (IOException e) {
            System.out.println( e );
            System.exit( -1 );
        }
        return null;
    }

    private static PrintStream makeOutputStream( String[] args )
    {
        try {
            if (args.length == 2)
                return new PrintStream( new FileOutputStream(args[1]) );
            return new PrintStream( System.out );
	} catch (IOException e) {
            System.out.println( e );
            System.exit( -1 );
        }
        return null;
    }
}

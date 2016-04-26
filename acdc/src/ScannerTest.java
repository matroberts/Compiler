import java.io.IOException;
import java.io.PushbackInputStream;

public class ScannerTest
{
    public static void main( String[] args )
    {
        Token token = null;

        Scanner scan = new Scanner(new PushbackInputStream(System.in));
        while (true)
        {
            try {
                token = scan.nextToken();
            } catch (Exception e) {
                System.out.println( e );
                return;
            }

            if (token.type() == Token.EOFsymbol)
                break;
                
            System.out.println();
            System.out.print( token.type() );
            System.out.print( ' ' );
            if (token.type() == Token.Identifier)
                System.out.print( token.cvalue() );
            else if (token.type() == Token.FloatValue)
                System.out.print( token.fvalue() );
            else if (token.type() == Token.IntegerValue)
                System.out.print( token.ivalue() );
        }
        
        System.out.println();
    }
}

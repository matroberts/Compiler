import java.io.PushbackInputStream;
import java.io.IOException;

public class Scanner
{
    private PushbackInputStream source;
    private Token               lookahead;

    public Scanner( PushbackInputStream in )
    {
        source    = in;
        lookahead = null;
    }

    public Token peek() throws IOException, LexicalException
    {
        if (lookahead == null)
            lookahead = getNextToken();
        return lookahead;
    }
    
    public Token nextToken() throws IOException, LexicalException
    {
        Token answer;
        if (lookahead !=null)
        {
            answer    = lookahead;
            lookahead = null;
        }
        else
            answer = getNextToken();
        return answer;
    }
    
    protected Token getNextToken() throws IOException, LexicalException
    {
       int nextByte = getNextByte();
       if ( nextByte == -1 )
          return new Token( Token.EOFsymbol );

       char nextChar = (char) nextByte;
       if ( isNumericDigit(nextChar) )
       {
          source.unread( nextByte );
          return numericToken();
       }
       
       Token answer;
       switch (nextChar)
       {
          case 'f':
             answer = new Token( Token.FloatDeclaration, nextChar );
             break;
          case 'i':
             answer = new Token( Token.IntegerDeclaration, nextChar );
             break;
          case 'p':
             answer = new Token( Token.PrintOp );
             break;
          case '=':
             answer = new Token( Token.AssignmentOp );
             break;
          case '+':
             answer = new Token( Token.PlusOp );
             break;
          case '-':
             answer = new Token( Token.MinusOp );
             break;
          case 'a': case 'b': case 'c': case 'd': case 'e': case 'g':
          case 'h': case 'j': case 'k': case 'l': case 'm': case 'n':
          case 'o': case 'q': case 'r':	case 's': case 't': case 'u':
          case 'v': case 'w': case 'x': case 'y': case 'z':
             answer = new Token( Token.Identifier, nextChar );
             break;
          default:
             source.unread(nextByte);
             throw new LexicalException( "Invalid character " + nextChar );
       }
       
       return answer;
    }
    
    protected int getNextByte() throws IOException
    {
       int nextChar;
       while (true)
       {
          nextChar = source.read();
          if ( nextChar == -1 )
             return -1;
          if ( !isWhitespace((char) nextChar) )
             return nextChar;
       }
    }
    
    protected Token numericToken() throws IOException, LexicalException
    {
       String buffer   = "";
       char   nextChar = (char) source.read();
       
       while ( isNumericDigit(nextChar) )
       {
          buffer  += nextChar; 
          nextChar = (char) source.read();
       }
       
       if (nextChar != '.')
       {
          source.unread( (int) nextChar );
          return new Token( Token.IntegerValue, Integer.parseInt(buffer) );
       }
       
       buffer  += nextChar;
       nextChar = (char) source.read();
       
       if ( !isNumericDigit(nextChar) )
       {
          source.unread( (int) nextChar );
          throw new LexicalException( "Expected a digit " + nextChar );
       }
       
       while ( isNumericDigit(nextChar) )
       {
          buffer  += nextChar; 
          nextChar = (char) source.read();
       }
       
       source.unread( (int) nextChar );
       return new Token( Token.FloatValue, Double.parseDouble(buffer) );
    }
    
    protected boolean isNumericDigit(char c)
    {
       return ('0' <= c) && (c <= '9');
    }
    
    protected boolean isWhitespace(char c)
    {
       return c == ' '  ||
              c == '\b' ||
              c == '\f' ||
              c == '\n' ||
              c == '\r' ||
              c == '\t';
    }
}

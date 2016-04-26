public class Token
{    public static final int FloatDeclaration   = 0;    public static final int IntegerDeclaration = 1;    public static final int PrintOp            = 2;    public static final int AssignmentOp       = 3;    public static final int PlusOp             = 4;    public static final int MinusOp            = 5;    public static final int Identifier         = 6;    public static final int FloatValue         = 7;    public static final int IntegerValue       = 8;    public static final int EOFsymbol          = 9;    private int    type;    private char   cvalue;    private int    ivalue;    private double fvalue;    public Token(int t)
    {
       this( t, 'A', 0, 0 );    }
        public Token(int t, char v)
    {
       this( t, v, 0, 0 );
    }
        public Token(int t, int v)
    {
       this( t, 'A', v, 0 );
    }
        public Token(int t, double v)
    {
       this( t, 'A', 0, v );
    }
        protected Token(int t, char c, int i, double d )
    {       type   = t;
       cvalue = c;
       ivalue = i;
       fvalue = d;
    }    public int    type  () { return type;   }
    public char   cvalue() { return cvalue; }
    public int    ivalue() { return ivalue; }
    public double fvalue() { return fvalue; }
}
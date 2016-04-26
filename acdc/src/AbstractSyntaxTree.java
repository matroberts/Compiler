public class AbstractSyntaxTree
{
    public String toString() { return "AST"; }
}

class Program extends AbstractSyntaxTree
{
    private Declaration declaration;
    private Statement   statement;

    public Program( Declaration d, Statement s )
    {
        declaration = d;
        statement   = s;
    }

    public Declaration declarations() { return declaration; }
    public Statement   statements  () { return statement;   }

    public String toString()
    {
        return "PROGRAM\n" + declaration + statement;
    }
}

class Declaration extends AbstractSyntaxTree
{
    private Declaration declaration;
    private Declaration declarations;

    public Declaration()
    {
        declaration  = null;
        declarations = null;
    }

    public Declaration( Declaration d, Declaration rest )
    {
        declaration  = d;
        declarations = rest;
    }

    public Declaration first() { return declaration;  }
    public Declaration rest () { return declarations; }

    public Identifier identifier() { return null; }
    public String toString()
    {
        String result = first().toString() + "\n";
        if ( rest() != null )
            result += rest().toString() + "\n";
        return result;
    }
}

class FloatDeclaration extends Declaration
{
    private Identifier identifier;

    public FloatDeclaration( Token identifier )
    {
        this.identifier = new Identifier(identifier.cvalue());
    }

    public Identifier identifier() { return identifier; }

    public String toString()
    {
        return "DECLARATION float " + identifier + "\n";
    }
}

class IntegerDeclaration extends Declaration
{
    private Identifier identifier;

    public IntegerDeclaration( Token identifier )
    {
        this.identifier = new Identifier(identifier.cvalue());
    }

    public Identifier identifier() { return identifier; }
    public String toString()
    {
        return "DECLARATION integer " + identifier + "\n";
    }
}

class Statement extends AbstractSyntaxTree
{
    private Statement statement;
    private Statement statements;

    protected Statement()
    {
        statement  = null;
        statements = null;
    }

    public Statement( Statement s, Statement rest )
    {
        statement  = s;
        statements = rest;
    }

    public Statement first() { return statement;  }
    public Statement rest () { return statements; }

    public String toString()
    {
        String result = first().toString() + "\n";
        if ( rest() != null )
            result += rest().toString() + "\n";
        return result;
    }
}

class PrintStatement extends Statement
{
    private Identifier identifier;

    public PrintStatement( Token t )
    {
        identifier = new Identifier( t.cvalue() );
    }

    public Identifier identifier() { return identifier; }

    public String toString()
    {
        return "PRINT " + identifier + "\n";
    }
}

class AssignmentStatement extends Statement
{
    private Identifier identifier;
    private Value      value;
    private DataType   type;

    public AssignmentStatement( Token t, Value v )
    {
        identifier = new Identifier( t.cvalue() );
        value      = v;
        type       = null;
    }

    public Identifier identifier() { return identifier; }
    public Value      value     () { return value;      }

    public void setValue( Value    v ) { value = v; }
    public void setType ( DataType t ) { type  = t; }

    public String toString()
    {
        return "ASSIGN " + identifier + " = " + value + "\n";
    }
}

abstract class Value extends AbstractSyntaxTree
{
    private DataType type;

    public Value( DataType t ) { type = t; }

    public DataType type()                { return type; }
    public void     setType( DataType t ) { type = t; }

    public String toString()
    {
        String typeString;
        if ( type == null )
            typeString = "none";
        else if (type.equals( DataType.Float ))
            typeString = "float";
        else if (type.equals( DataType.Integer ))
            typeString = "int";
        else
            typeString = "other datatype";

        return " (type " + typeString + ")";
    }
}

class Identifier extends Value
{
    private char identifier;

    public Identifier( char c )
    {
        super( null );
        identifier = c;
    }

    public char   value   () { return identifier; }
    public String toString() { return "" + identifier + super.toString(); }

    public boolean equals( Object obj )
    {
        if (! (obj instanceof Identifier))
            return false;
        Identifier other = (Identifier) obj;
        return identifier == other.value();
    }

    public int hashCode()
    {
        return (int) identifier;
    }
}

class FloatValue extends Value
{
    private double value;

    public FloatValue( double d )
    {
        super( null );
        value = d;
    }

    public double value() { return value; }
    public String toString() { return "" + value + super.toString(); }
}

class IntegerValue extends Value
{
    private int value;

    public IntegerValue( int i )
    {
        super( null );
        value = i;
    }

    public int value() { return value; }
    public String toString() { return "" + value + super.toString(); }
}

class ExpressionTail extends Value
{
    private Value leftOperand;
    private Value rightOperand;

    public ExpressionTail( Value v )
    {
        super( null );
        leftOperand  = null;
        rightOperand = v;
    }

    public void addLeftOperand( Value v )
    {
        leftOperand = v;
    }

    public Value leftOperand () { return leftOperand;  }
    public Value rightOperand() { return rightOperand; }

    public void  setLeftOperand ( Value v ) { leftOperand  = v; }
    public void  setRightOperand( Value v ) { rightOperand = v; }

    protected String operatorString() { return "?"; }
    public String toString()
    {
        return leftOperand.toString() + " " +
                     operatorString() + " " +
              rightOperand.toString() + super.toString();
    }
}

class PlusOperation extends ExpressionTail
{
    public PlusOperation( Value v ) { super( v ); }

    protected String operatorString() { return "+"; }
}

class MinusOperation extends ExpressionTail
{
    public MinusOperation( Value v ) { super( v ); }

    protected String operatorString() { return "-"; }
}

class IntToFloatConversion extends Value
{
    private Value baseValue;

    public IntToFloatConversion( Value decoratedValue )
    {
        super( DataType.Float );
        baseValue = decoratedValue;
    }

    public Value value() { return baseValue; }

    public DataType type()                { return baseValue.type(); }
    public void     setType( DataType t ) { baseValue.setType( t ); }

    public String toString()
    {
        return "Int to float conversion of [" + baseValue.toString() + "]";
    }
}

import java.io.IOException;

public class Parser
{
    private Scanner source;

    public Parser( Scanner source )
    {
        this.source = source;
    }

    public AbstractSyntaxTree parse()
    {
        AbstractSyntaxTree answer = null;
        try {
            answer = parseProgram();
        }
        catch (Exception e) {
            System.out.println(e);
        }
        return answer;
    }
    
    protected AbstractSyntaxTree parseProgram() throws ParseException,
                                                       LexicalException,
                                                       IOException
    {
        return AbstractSyntaxTreeFactory.makeProgram( parseDeclarations(),
                                                      parseStatements() );
    }
    
    protected AbstractSyntaxTree parseDeclarations() throws ParseException,
                                                            LexicalException,
                                                            IOException
    {
        switch ( source.peek().type() )
        {
            case Token.FloatDeclaration:
            case Token.IntegerDeclaration:
                return AbstractSyntaxTreeFactory.makeDeclarations(
                                                    parseDeclaration(),
                                                    parseDeclarations() );
            case Token.Identifier:
            case Token.PrintOp:
            case Token.EOFsymbol:
                return null;
            default:
                throw new ParseException( "Expected declarations, saw " +
                                          source.peek() );
        }
    }
    
    protected AbstractSyntaxTree parseDeclaration() throws ParseException,
                                                           LexicalException,
                                                           IOException
    {
        switch ( source.peek().type() )
        {
            case Token.FloatDeclaration:
            case Token.IntegerDeclaration:
                Token declaration = source.nextToken();
                Token identifier  = source.nextToken();
                return AbstractSyntaxTreeFactory.makeTree(
                                                    declaration, identifier );
            default:
                throw new ParseException( "Expected declaration, saw " +
                                          source.peek() );
        }
    }
    
    protected AbstractSyntaxTree parseStatements() throws ParseException,
                                                          LexicalException,
                                                          IOException
    {
        switch ( source.peek().type() )
        {
            case Token.Identifier:
            case Token.PrintOp:
                return AbstractSyntaxTreeFactory.makeStatements(
                                                     parseStatement(),
                                                     parseStatements() );
            case Token.EOFsymbol:
                return null;
            default:
                throw new ParseException( "Expected statements, saw " +
                                          source.peek() );
        }
    }
    
    protected AbstractSyntaxTree parseStatement() throws ParseException,
                                                         LexicalException,
                                                         IOException
    {
        switch ( source.peek().type() )
        {
            case Token.Identifier:
                Token identifier = source.nextToken();
                if (source.peek().type() == Token.AssignmentOp)
                    return AbstractSyntaxTreeFactory.makeTree(
                               source.nextToken(),
                               identifier,
                               parseValue(),
                               parseExpressionTail() );
                else
                    throw new ParseException( "Expected assignment op, saw " +
                                              source.peek() );
            case Token.PrintOp:
                Token printToken = source.nextToken();
                if (source.peek().type() == Token.Identifier)
                    return AbstractSyntaxTreeFactory.makeTree( printToken,
                                                        source.nextToken() );
                else
                    throw new ParseException( "Expected identifier, saw " +
                                              source.peek() );
            default:
                throw new ParseException( "Expected statement, saw " +
                                          source.peek() );
        }
    }
    
    protected AbstractSyntaxTree parseValue() throws ParseException,
                                                     LexicalException,
                                                     IOException
    {
        switch ( source.peek().type() )
        {
            case Token.Identifier:
            case Token.FloatValue:
            case Token.IntegerValue:
                return AbstractSyntaxTreeFactory.makeTree(
                                                     source.nextToken() );
            default:
                throw new ParseException( "Expected numeric value, saw " +
                                          source.peek() );
        }
    }
    
    protected AbstractSyntaxTree parseExpressionTail() throws ParseException,
                                                              LexicalException,
                                                              IOException
    {
        switch ( source.peek().type() )
        {
            case Token.PlusOp:
            case Token.MinusOp:
                return AbstractSyntaxTreeFactory.makeTree(
                                                    source.nextToken(),
                                                    parseValue(),
                                                    parseExpressionTail() );
            case Token.Identifier:
            case Token.PrintOp:
            case Token.EOFsymbol:
                return null;
            default:
                throw new ParseException( "Expected numeric value, saw " +
                                          source.peek() );
        }
    }
}

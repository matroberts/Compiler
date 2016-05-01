namespace TextTemplating
{
    public class Checker
    {
        public void Check(TokenList tokens, Errors errors)
        {
            for (int i=0; i<tokens.Count; i++)
            {
                var token = tokens[i] as TagToken;

                if(token==null)
                    continue;

                if (!token.IsClosed())
                {
                    errors.Add($"Tempate tag not terminated with }}, problem text near '{token.Value.TruncateWithElipses(25)}'");
                    tokens[i] = token.ToLiteral();
                }
            }
        }
    }
}
namespace TextTemplating
{
    public class Checker
    {
        public void Check(TokenList tokens, Errors errors)
        {
            for (int i=0; i<tokens.Count; i++)
            {
                var token = tokens[i];
                string errorMessage;
                if (!token.IsValid(out errorMessage))
                {
                    errors.Add(errorMessage);
                    tokens[i] = token.ToLiteral();
                }
            }
        }
    }
}
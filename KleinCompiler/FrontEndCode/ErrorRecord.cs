using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler.FrontEndCode
{
    public enum ErrorTypeEnum
    {
        No,
        Lexical,
        Syntax,
        Semantic
    }
    public class ErrorRecord
    {
        public FilePositionCalculator FilePositionCalculator { get; }
        public ErrorRecord(string input)
        {
            FilePositionCalculator = new FilePositionCalculator(input);
        }

        public ErrorTypeEnum ErrorType { get; private set; }

        private int _position;
        public int Position
        {
            get { return _position; }
            private set
            {
                _position = value;
                FilePosition = FilePositionCalculator.FilePosition(value);
            }
        }

        public FilePosition FilePosition { get; private set; }
        public string Message { get; private set; }
        public string StackTrace { get; private set; }

        public override string ToString()
        {
            string output = $"{FilePosition} {ErrorType} Error: {Message}";
            if (string.IsNullOrWhiteSpace(StackTrace) == false)
                output += $"\r\n\r\n{StackTrace}";
            return output;
        }

        public void AddNoError()
        {
            ErrorType = ErrorTypeEnum.No;
            Position = 0;
            Message = "No Error Has Occured";
            StackTrace = "";
        }
        public void AddLexicalError(ErrorToken token, string stackTrace)
        {
            ErrorType = ErrorTypeEnum.Lexical;
            Position = token.Position;
            Message = token.Message;
            StackTrace = stackTrace;
        }
        public void AddSyntaxError(Symbol symbol, Token token, string stackTrace)
        {
            ErrorType = ErrorTypeEnum.Syntax;
            Position = token.Position;
            Message = $"Attempting to parse symbol '{symbol.ToString()}' found token {token.ToString()}";
            StackTrace = stackTrace;
        }

        public void AddSemanticError(TypeValidationResult result)
        {
            if (result.HasError)
            {
                ErrorType = ErrorTypeEnum.Semantic;
                Position = result.Position;
                Message = result.Message;
                StackTrace = null;
            }
            else
                AddNoError();
        }
    }
}
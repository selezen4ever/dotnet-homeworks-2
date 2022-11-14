namespace Hw10.Services.MathCalculator.Parser;

public class ParsedExpression
{
    public ParsedExpression(List<Character>? characters)
    {
        IsSuccess = true;
        Characters = characters; 
    }
    
    public ParsedExpression(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }
    
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    
    public List<Character>? Characters { get; }
}
namespace Hw11.Services.MathCalculator.Parser;

public interface IParser
{
    public List<Character> GetParsedExpression(string? expression);
}
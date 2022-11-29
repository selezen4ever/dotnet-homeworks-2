namespace Hw9.Services.MathCalculator.Parser;

public interface IParser
{
    public ParsedExpression ParseExpressionToTokens(string? expression);
}
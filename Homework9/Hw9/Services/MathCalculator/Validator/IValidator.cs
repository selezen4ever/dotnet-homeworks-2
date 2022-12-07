namespace Hw9.Services.MathCalculator.Validator;

public interface IValidator
{
    public string ValidateExpression(List<Character> tokens);
}
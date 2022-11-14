using Hw10.Services.MathCalculator.Parser;

namespace Hw10.Services.MathCalculator.Validator;

public interface IValidator
{
    public bool TryValidateExpression(List<Character> tokens, out string errorMessage);
}
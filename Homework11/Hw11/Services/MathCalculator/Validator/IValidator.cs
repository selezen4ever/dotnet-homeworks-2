namespace Hw11.Services.MathCalculator.Validator;

public interface IValidator
{
    public void TryValidateExpression(List<Character> characters);
}
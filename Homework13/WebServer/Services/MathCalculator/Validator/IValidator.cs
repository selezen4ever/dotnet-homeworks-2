
namespace WebServer.Services.MathCalculator.Validator;

public interface IValidator
{
    public bool TryValidateExpression(List<Character> tokens, out string errorMessage);
}
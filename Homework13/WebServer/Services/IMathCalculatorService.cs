using WebServer.Dto;

namespace WebServer.Services;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}
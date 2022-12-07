using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using WebServer.Dto;
using WebServer.ErrorMessages;
using WebServer.Services.MathCalculator.ExpressionBuilder;
using WebServer.Services.MathCalculator.ExpressionGraphBuilder;
using WebServer.Services.MathCalculator.Parser;
using WebServer.Services.MathCalculator.Validator;

namespace WebServer.Services;

public class MathCalculatorService : IMathCalculatorService
{
    private readonly IValidator _validator;
    private readonly IParser _parser;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IExpressionGraphBuilder _expressionGraphBuilder;

    public MathCalculatorService(
        IValidator validator, 
        IParser parser,
        IExpressionGraphBuilder expressionGraphBuilder, 
        IExpressionBuilder expressionBuilder)
    {
        _validator = validator;
        _parser = parser;
        _expressionGraphBuilder = expressionGraphBuilder;
        _expressionBuilder = expressionBuilder;
    }
    
     public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var parseResult = _parser.GetParsedExpression(expression);
        if (!parseResult.IsSuccess)
            return new CalculationMathExpressionResultDto(parseResult.ErrorMessage!);

        if (!_validator.TryValidateExpression(parseResult.Characters!, out var errorMessage))
            return new CalculationMathExpressionResultDto(errorMessage);
        
        var convertedExpression = _expressionBuilder.GetExpressionFromCharacters(parseResult.Characters!);
        var graph = _expressionGraphBuilder.BuildGraph(convertedExpression);
        var result = await CalculateAsync(convertedExpression, graph);

        return result is double.NaN ? 
            new CalculationMathExpressionResultDto(MathErrorMessager.DivisionByZero) :
            new CalculationMathExpressionResultDto(result);
    }

    private static async Task<double> CalculateAsync(Expression current,
        IReadOnlyDictionary<Expression, MyExpression> dependencies)
    {
        if (!dependencies.ContainsKey(current))
        {
            return double.Parse(current.ToString());
        }
        
        var left = Task.Run(() => 
            CalculateAsync(dependencies[current].LeftExpression, dependencies));
        var right = Task.Run(() => 
            CalculateAsync(dependencies[current].RightExpression, dependencies));

        var results = await Task.WhenAll(left, right);
        return CalculateExpression(results[0], current.NodeType, results[1]);
    }

    [ExcludeFromCodeCoverage]
    private static double CalculateExpression(double value1, ExpressionType expressionType, double value2) =>
        expressionType switch
        {
            ExpressionType.Add => value1 + value2,
            ExpressionType.Subtract => value1 - value2,
            ExpressionType.Divide => value2 == 0 ? double.NaN : value1 / value2,
            ExpressionType.Multiply => value1 * value2,
            _ => 0
        };
}
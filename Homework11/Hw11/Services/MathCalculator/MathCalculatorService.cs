using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw11.ErrorMessages;
using Hw11.Services.MathCalculator.ExpressionBuilder;
using Hw11.Services.MathCalculator.ExpressionGraphBuilder;
using Hw11.Services.MathCalculator.Parser;
using Hw11.Services.MathCalculator.Validator;

namespace Hw11.Services.MathCalculator;

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
    
     public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var characters = _parser.GetParsedExpression(expression);
        _validator.TryValidateExpression(characters);

        var convertedExpression = _expressionBuilder.GetExpressionFromCharacters(characters);
        var graph = _expressionGraphBuilder.BuildGraph(convertedExpression);
        var result = await CalculateAsync(convertedExpression, graph);

        return result is double.NaN ? throw new DivideByZeroException(MathErrorMessager.DivisionByZero) : result;
    }

    private static async Task<double> CalculateAsync(Expression current,
        IReadOnlyDictionary<Expression, MyExpression> dependencies)
    {
        if (!dependencies.ContainsKey(current))
        {
            return double.Parse(current.ToString());
        }
        
        await Task.Delay(1000);
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
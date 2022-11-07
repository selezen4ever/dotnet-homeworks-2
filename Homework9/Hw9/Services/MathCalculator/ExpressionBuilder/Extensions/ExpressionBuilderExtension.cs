using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator.ExpressionBuilder.Extensions;

public static class ExpressionBuilderExtension
{
    public enum CalculatorOperation
    {
        Plus,
        Minus,
        Multiply,
        Divide,
    }
    
    [ExcludeFromCodeCoverage]
    public static ExpressionType TryParse(string str)
    {
        return str switch
        {
            "+" => ExpressionType.Add,
            "-" => ExpressionType.Subtract,
            "*" => ExpressionType.Multiply,
            "/" => ExpressionType.Divide
        };
    }
}
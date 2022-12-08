using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace WebServer.Services.MathCalculator.ExpressionBuilder.Extensions;

public static class ExpressionBuilderExtension
{
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
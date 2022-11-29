using Hw11.Services.MathCalculator;
using Hw11.Services.MathCalculator.ExpressionBuilder;
using Hw11.Services.MathCalculator.ExpressionGraphBuilder;
using Hw11.Services.MathCalculator.Parser;
using Hw11.Services.MathCalculator.Validator;

namespace Hw11.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<IMathCalculatorService, MathCalculatorService>()
            .AddTransient<IExpressionGraphBuilder, ExpressionGraphBuilder>()
            .AddSingleton<IValidator, Validator>()
            .AddSingleton<IExpressionBuilder, ExpressionBuilder>()
            .AddSingleton<IParser, Parser>();
    }
}
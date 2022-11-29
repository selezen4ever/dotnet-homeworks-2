using Hw9.Services.MathCalculator;
using Hw9.Services.MathCalculator.ExpressionBuilder;
using Hw9.Services.MathCalculator.ExpressionGraphBuilder;
using Hw9.Services.MathCalculator.Parser;
using Hw9.Services.MathCalculator.Validator;

namespace Hw9.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<IMathCalculatorService, MathCalculatorService>()
            .AddTransient<IExpressionGraphBuilder, ExpressionGraphBuilder>()
            .AddSingleton<IValidator, Validator>()
            .AddSingleton<IExpressionBuilder, ExpressionBuilder>()
            .AddSingleton<IParser, Parser>();
    }
}
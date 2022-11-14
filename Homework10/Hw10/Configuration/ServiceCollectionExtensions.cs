using Hw10.DbModels;
using Hw10.Services;
using Hw10.Services.CachedCalculator;
using Hw10.Services.MathCalculator.ExpressionBuilder;
using Hw10.Services.MathCalculator.ExpressionGraphBuilder;
using Hw10.Services.MathCalculator.Parser;
using Hw10.Services.MathCalculator.Validator;

namespace Hw10.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services
            .AddTransient<MathCalculatorService>()
            .AddTransient<IExpressionGraphBuilder, ExpressionGraphBuilder>()
            .AddSingleton<IValidator, Validator>()
            .AddSingleton<IExpressionBuilder, ExpressionBuilder>()
            .AddSingleton<IParser, Parser>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<ApplicationContext>(), 
                s.GetRequiredService<MathCalculatorService>()));
    }
}
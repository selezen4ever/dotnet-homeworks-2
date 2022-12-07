using WebServer.Services;
using WebServer.Services.CachedCalculator;
using WebServer.Services.MathCalculator.ExpressionBuilder;
using WebServer.Services.MathCalculator.ExpressionGraphBuilder;
using WebServer.Services.MathCalculator.Parser;
using WebServer.Services.MathCalculator.Validator;

namespace WebServer.Configuration;

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
                s.GetRequiredService<MathCalculatorService>()));
    }
}
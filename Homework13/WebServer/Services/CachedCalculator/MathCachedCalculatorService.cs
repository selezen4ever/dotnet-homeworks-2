using WebServer.Dto;

namespace WebServer.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private static readonly Lazy<Dictionary<string, double>> CalcCacheDictionaryLazy = new (() => new Dictionary<string, double>());
	public static readonly Dictionary<string, double> CalcCacheDictionary = CalcCacheDictionaryLazy.Value;
	
	private readonly IMathCalculatorService _simpleCalculator;
	
	public MathCachedCalculatorService(IMathCalculatorService simpleCalculator)
	{
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var filteredExpression = expression?.Replace(" ", "");
		var resultFromCache = GetCachedExpression(filteredExpression);
		if (resultFromCache is not null)
			return new CalculationMathExpressionResultDto((double)resultFromCache);

		var calculationResultDto = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (!calculationResultDto.IsSuccess)
			return calculationResultDto;
		
		CacheExpression(filteredExpression!, calculationResultDto.Result);
		return new CalculationMathExpressionResultDto(calculationResultDto.Result);
	}
	
	private double? GetCachedExpression(string? expression)
	{
		if (expression == null || !CalcCacheDictionary.ContainsKey(expression))
			return null;
		return CalcCacheDictionary[expression];
	}
	
	private void CacheExpression(string expression, double expressionResult)
	{
		CalcCacheDictionary.Add(expression, expressionResult);
	}
	
}
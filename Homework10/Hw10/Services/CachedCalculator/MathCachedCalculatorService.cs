using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public new async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var filteredExpression = expression?.Replace(" ", "");
		var resultFromDb = await GetCachedExpression(filteredExpression);
		if (resultFromDb is not null)
		{
			await Task.Delay(1000);
			return new CalculationMathExpressionResultDto((double) resultFromDb);
		}

		var calculationResult = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (!calculationResult.IsSuccess)
			return calculationResult;
		_dbContext.Add(new SolvingExpression
		{
			Expression = filteredExpression!,
			Result = calculationResult.Result
		});
		await _dbContext.SaveChangesAsync();
		
		return new CalculationMathExpressionResultDto(calculationResult.Result);
	}
	
	private async Task<double?> GetCachedExpression(string? expression)
	{
		return await _dbContext.SolvingExpressions
			.Where(x => x.Expression == expression)
			.FirstOrDefaultAsync()
			.ContinueWith(x => x.Result?.Result);
	}
}
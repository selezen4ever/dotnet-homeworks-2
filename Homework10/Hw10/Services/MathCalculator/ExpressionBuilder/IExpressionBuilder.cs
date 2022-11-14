using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator.ExpressionBuilder;

public interface IExpressionBuilder
{
    public Expression GetExpressionFromCharacters(IEnumerable<Character> characters);
}
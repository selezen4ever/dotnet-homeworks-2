using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator.ExpressionBuilder;

public interface IExpressionBuilder
{
    public Expression GetExpressionFromCharacters(IEnumerable<Character> characters);
}
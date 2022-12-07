using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator.ExpressionBuilder;

public interface IExpressionBuilder
{
    public Expression GetExpressionFromCharacters(IEnumerable<Character> characters);
}
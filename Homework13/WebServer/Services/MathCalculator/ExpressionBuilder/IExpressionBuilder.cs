using System.Linq.Expressions;

namespace WebServer.Services.MathCalculator.ExpressionBuilder;

public interface IExpressionBuilder
{
    public Expression GetExpressionFromCharacters(IEnumerable<Character> characters);
}
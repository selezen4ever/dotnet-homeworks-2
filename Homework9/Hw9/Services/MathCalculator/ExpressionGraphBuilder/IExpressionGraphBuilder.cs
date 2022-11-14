using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator.ExpressionGraphBuilder;

public interface IExpressionGraphBuilder
{
    Dictionary<Expression, MyExpression> BuildGraph(Expression expression);
}
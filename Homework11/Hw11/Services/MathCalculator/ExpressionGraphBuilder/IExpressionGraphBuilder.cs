using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator.ExpressionGraphBuilder;

public interface IExpressionGraphBuilder
{
    Dictionary<Expression, MyExpression> BuildGraph(Expression expression);
}
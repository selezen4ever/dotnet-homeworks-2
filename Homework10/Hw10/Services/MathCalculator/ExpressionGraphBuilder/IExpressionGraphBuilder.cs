using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator.ExpressionGraphBuilder;

public interface IExpressionGraphBuilder
{
    Dictionary<Expression, MyExpression> BuildGraph(Expression expression);
}
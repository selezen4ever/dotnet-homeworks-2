using System.Linq.Expressions;

namespace WebServer.Services.MathCalculator.ExpressionGraphBuilder;

public interface IExpressionGraphBuilder
{
    Dictionary<Expression, MyExpression> BuildGraph(Expression expression);
}
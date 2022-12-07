using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator.ExpressionGraphBuilder;

public record MyExpression(Expression LeftExpression, Expression RightExpression);
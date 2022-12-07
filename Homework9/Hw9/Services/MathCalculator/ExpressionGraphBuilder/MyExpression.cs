using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator.ExpressionGraphBuilder;

public record MyExpression(Expression LeftExpression, Expression RightExpression);
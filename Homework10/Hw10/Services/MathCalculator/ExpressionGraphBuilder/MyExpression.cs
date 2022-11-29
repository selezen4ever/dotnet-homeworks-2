using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator.ExpressionGraphBuilder;

public record MyExpression(Expression LeftExpression, Expression RightExpression);
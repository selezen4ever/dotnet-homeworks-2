using System.Linq.Expressions;

namespace WebServer.Services.MathCalculator.ExpressionGraphBuilder;

public record MyExpression(Expression LeftExpression, Expression RightExpression);
using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator.ExpressionGraphBuilder;

public class ExpressionGraphBuilder : ExpressionVisitor, IExpressionGraphBuilder
{
    private readonly Dictionary<Expression, MyExpression> _dependencies = new();

    public Dictionary<Expression, MyExpression> 
        BuildGraph(Expression expression)
    {
        Visit(expression);
        return _dependencies;
    }
        
    protected override Expression VisitBinary(BinaryExpression node)
    {
        _dependencies.Add(node, new MyExpression(node.Left, node.Right));
        Visit(node.Left);
        Visit(node.Right);
        return node;
    }
}
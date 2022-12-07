using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator.ExpressionGraphBuilder;

public class ExpressionGraphBuilder : ExpressionVisitor, IExpressionGraphBuilder
{
    private readonly Dictionary<Expression, MyExpression> _dependencies = new();

    public Dictionary<Expression, MyExpression> 
        BuildGraph(Expression expression)
    {
        Visit((dynamic)expression);
        return _dependencies;
    }
    
    private void Visit(ConstantExpression node) { }
    
    private void Visit(BinaryExpression node)
    {
        _dependencies.Add(node, new MyExpression(node.Left, node.Right));
        Visit((dynamic)node.Left);
        Visit((dynamic)node.Right);
    }
}
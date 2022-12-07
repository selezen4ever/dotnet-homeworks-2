using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using Hw11.Services.MathCalculator.ExpressionBuilder.Extensions;

namespace Hw11.Services.MathCalculator.ExpressionBuilder;

public class ExpressionBuilder : IExpressionBuilder
{
    private readonly Dictionary<string, int> _operationPriority = new()
    {
        {"+", 1},
        {"-", 1},
        {"*", 2},
        {"/", 2},
        {"(", 0}
    };
    
    public Expression GetExpressionFromCharacters(IEnumerable<Character> characters)
    {
        var expressions = new Stack<Expression>();
        var operations = new Stack<Character>();
        var isNegativeNumber = false;
        Character? prevChar = null;
    
        foreach (var currentChar in characters)
        {
            switch (currentChar.Type)
            {
                case CharacterType.Number:
                    expressions.Push(Expression.Constant(
                        (isNegativeNumber ? -1 : 1) * double.Parse(currentChar.Value, 
                            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture), typeof(double)));
                    isNegativeNumber = false;
                    break;
                case CharacterType.Operator:
                    if (prevChar is not null && prevChar.Value.Value == "(" && currentChar.Value == "-")
                        isNegativeNumber = true;
                    else AddOperations(currentChar, expressions, operations);
                    break;
                case CharacterType.OpeningBracket:
                    operations.Push(currentChar);
                    break;
                case CharacterType.ClosingBracket:
                    AddOperationsBeforeOpenBracket(expressions, operations);
                    break;
            }

            prevChar = currentChar;
        }

        AddLastOperations(expressions, operations);

        return expressions.Pop();
    }
    
    private void MakeBinaryExpression(Stack<Expression> expressions, Character operation)
    {
        var rightNode = expressions.Pop();
        expressions.Push(Expression.MakeBinary(ExpressionBuilderExtension.TryParse(operation.Value), expressions.Pop(),
            rightNode));
    }
    
    private void AddOperations(Character operation, Stack<Expression> expressions, Stack<Character> operations)
    {
        while (operations.Count > 0 && _operationPriority[operations.Peek().Value] >= _operationPriority[operation.Value])
            MakeBinaryExpression(expressions, operations.Pop());

        operations.Push(operation);
    }

    private void AddOperationsBeforeOpenBracket(Stack<Expression> expressions, Stack<Character> operations)
    {
        var operation = operations.Pop();
        while (operations.Count > 0 && operation.Type is not CharacterType.OpeningBracket)
        {
            MakeBinaryExpression(expressions, operation);
            operation = operations.Pop();
        }
    }

    private void AddLastOperations(Stack<Expression> expressions, Stack<Character> operations)
    {
        while (operations.Count > 0)
            MakeBinaryExpression(expressions, operations.Pop());
    }
    
    
    [ExcludeFromCodeCoverage]
    private bool SetNegativity(Character currentChar, Character? prevChar)
    {
        return prevChar?.Type is CharacterType.OpeningBracket && currentChar.Value == "-";
    }
}
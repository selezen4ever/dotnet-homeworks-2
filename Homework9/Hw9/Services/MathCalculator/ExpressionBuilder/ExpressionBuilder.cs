using System.Globalization;
using System.Linq.Expressions;
using Hw9.Services.MathCalculator.ExpressionBuilder.Extensions;

namespace Hw9.Services.MathCalculator.ExpressionBuilder;

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
        var expressionsStack = new Stack<Expression>();
        var charactersStack = new Stack<Character>();
        var negativity = false;
        Character? prevCharacter = null;
    
        foreach (var ch in  characters)
        {
            switch (ch.Type)
            {
                case CharacterType.Number:
                    expressionsStack.Push(Expression.Constant(
                        (negativity ? -1 : 1) * double.Parse(ch.Value, 
                            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture), typeof(double)));
                    negativity = false;
                    break;
                case CharacterType.Operator:
                    if (prevCharacter is not null && prevCharacter.Value.Value == "(" && ch.Value == "-")
                        negativity = true;
                    else AddOperations(ch, expressionsStack, charactersStack);
                    break;
                case CharacterType.Bracket:
                    if (ch.Value == "(")
                        charactersStack.Push(ch);
                    else AddOperationsBeforeOpenBracket(expressionsStack, charactersStack);
                    break;
            }

            prevCharacter = ch;
        }

        AddLastOperations(expressionsStack, charactersStack);

        return expressionsStack.Pop();
    }
    
    private void MakeBinaryExpression(Stack<Expression> outputStack, Character operation)
    {
        var rightNode = outputStack.Pop();
        outputStack.Push(Expression.MakeBinary(ExpressionBuilderExtension.TryParse(operation.Value), outputStack.Pop(),
            rightNode));
    }
    
    private void AddOperations(Character operation, Stack<Expression> outputStack, Stack<Character> operatorStack)
    {
        while (operatorStack.Count > 0 && _operationPriority[operatorStack.Peek().Value] >= _operationPriority[operation.Value])
            MakeBinaryExpression(outputStack, operatorStack.Pop());

        operatorStack.Push(operation);
    }

    private void AddOperationsBeforeOpenBracket(Stack<Expression> outputStack, Stack<Character> operatorStack)
    {
        var operation = operatorStack.Pop();
        while (operatorStack.Count > 0 && operation.Value != "(")
        {
            MakeBinaryExpression(outputStack, operation);
            operation = operatorStack.Pop();
        }
    }

    private void AddLastOperations(Stack<Expression> outputStack, Stack<Character> operatorStack)
    {
        while (operatorStack.Count > 0)
            MakeBinaryExpression(outputStack, operatorStack.Pop());
    }
}
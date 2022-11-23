using System.Diagnostics.CodeAnalysis;
using Hw11.ErrorMessages;
using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator.Validator;

public class Validator : IValidator
{
    public void TryValidateExpression(List<Character> tokens)
    {
        if (!tokens.Any())
            throw new InvalidSyntaxException(MathErrorMessager.EmptyString);
        
        
        var soloOpenBracketsCount = 0;
        Character? lastToken = null;
        
        foreach (var currentToken in tokens)
        {
            switch (currentToken.Type)
            {
                case CharacterType.Number:
                    break;
                case CharacterType.Operator:
                    if (lastToken is null)
                        throw new InvalidSyntaxException(MathErrorMessager.StartingWithOperation);
                    if (lastToken is { Type: CharacterType.Operator })
                        throw new InvalidSyntaxException(MathErrorMessager.TwoOperationInRowMessage(lastToken.Value.Value, currentToken.Value));
                    if (lastToken is { Value: "(" } && currentToken.Value != "-")
                        throw new InvalidSyntaxException(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(currentToken.Value));
                    break;
                case CharacterType.OpeningBracket:
                    soloOpenBracketsCount++;
                    break;
                case CharacterType.ClosingBracket:
                    soloOpenBracketsCount--;
                    if (soloOpenBracketsCount < 0)
                        throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
                    CheckIfPrevTokenIsOperation(lastToken);
                    break;
            }
            lastToken = currentToken;
        }
        
        CheckForEndingWithOperation(lastToken);

        if (soloOpenBracketsCount != 0)
        {
            throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
        }
    }

    [ExcludeFromCodeCoverage]
    private static void CheckIfPrevTokenIsOperation(Character? lastToken)
    {
        if (lastToken?.Type == CharacterType.Operator)
            throw new InvalidSyntaxException(
                MathErrorMessager.OperationBeforeParenthesisMessage(lastToken.Value.Value));
    }

    [ExcludeFromCodeCoverage]
    private static void CheckForEndingWithOperation(Character? lastToken)
    {
        if (lastToken?.Type == CharacterType.Operator)
        {
            throw new InvalidSyntaxException(MathErrorMessager.EndingWithOperation);
        }
    }
}
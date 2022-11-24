using System.Diagnostics.CodeAnalysis;
using Hw11.ErrorMessages;
using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator.Validator;

public class Validator : IValidator
{
    public void TryValidateExpression(List<Character> characters)
    {
        if (!characters.Any())
            throw new InvalidSyntaxException(MathErrorMessager.EmptyString);
        
        
        var soloOpenBracketsCount = 0;
        Character? prevChar = null;
        
        foreach (var currentChar in characters)
        {
            switch (currentChar.Type)
            {
                case CharacterType.Number:
                    break;
                case CharacterType.Operator:
                    switch (prevChar)
                    {
                        case null:
                            throw new InvalidSyntaxException(MathErrorMessager.StartingWithOperation);
                        case { Type: CharacterType.Operator }:
                            throw new InvalidSyntaxException(MathErrorMessager.TwoOperationInRowMessage(prevChar.Value.Value, currentChar.Value));
                        case { Value: "(" } when currentChar.Value != "-":
                            throw new InvalidSyntaxException(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(currentChar.Value));
                    }

                    break;
                case CharacterType.OpeningBracket:
                    soloOpenBracketsCount++;
                    break;
                case CharacterType.ClosingBracket:
                    soloOpenBracketsCount--;
                    if (soloOpenBracketsCount < 0)
                        throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
                    CheckIfPrevTokenIsOperation(prevChar);
                    break;
            }
            prevChar = currentChar;
        }
        
        CheckForEndingWithOperation(prevChar);

        if (soloOpenBracketsCount != 0)
        {
            throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);
        }
    }
    [ExcludeFromCodeCoverage]
    private static void CheckIfPrevTokenIsOperation(Character? prevChar)
    {
        if (prevChar?.Type == CharacterType.Operator)
            throw new InvalidSyntaxException(
                MathErrorMessager.OperationBeforeParenthesisMessage(prevChar.Value.Value));
    }

    [ExcludeFromCodeCoverage]
    private static void CheckForEndingWithOperation(Character? prevChar)
    {
        if (prevChar?.Type == CharacterType.Operator)
        {
            throw new InvalidSyntaxException(MathErrorMessager.EndingWithOperation);
        }
    }
}
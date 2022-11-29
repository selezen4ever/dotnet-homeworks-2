using System.Diagnostics.CodeAnalysis;
using Hw10.ErrorMessages;

namespace Hw10.Services.MathCalculator.Validator;

public class Validator : IValidator
{
    public bool TryValidateExpression(List<Character> tokens, out string errorMessage)
    {
        if (!tokens.Any())
        {
            errorMessage = MathErrorMessager.EmptyString;
            return false;
        }
        
        var soloOpenBracketsCount = 0;
        errorMessage = "";
        Character? lastToken = null;
        
        foreach (var currentToken in tokens)
        {
            switch (currentToken.Type)
            {
                case CharacterType.Number:
                    break;
                case CharacterType.Operator:
                    if (lastToken is null)
                        errorMessage = MathErrorMessager.StartingWithOperation;
                    if (lastToken is { Type: CharacterType.Operator })
                        errorMessage = MathErrorMessager.TwoOperationInRowMessage(lastToken.Value.Value, currentToken.Value);
                    if (lastToken is { Value: "(" } && currentToken.Value != "-")
                        errorMessage = MathErrorMessager.InvalidOperatorAfterParenthesisMessage(currentToken.Value);
                    break;
                case CharacterType.OpeningBracket:
                    soloOpenBracketsCount++;
                    break;
                case CharacterType.ClosingBracket:
                    soloOpenBracketsCount--;
                    if (soloOpenBracketsCount < 0)
                        errorMessage = MathErrorMessager.IncorrectBracketsNumber;
                    errorMessage = CheckIfPrevTokenIsOperation(lastToken);
                    break;
            }

            if (!string.IsNullOrEmpty(errorMessage))
                return false;
            lastToken = currentToken;
        }

        errorMessage = CheckForEndingWithOperation(lastToken);
        if (errorMessage != "")
            return false;

        if (soloOpenBracketsCount == 0)
        {
            return true;
        }
        
        errorMessage = MathErrorMessager.IncorrectBracketsNumber;
        return false;
    }
    
    [ExcludeFromCodeCoverage]
    private static string CheckIfPrevTokenIsOperation(Character? lastToken) => 
        lastToken?.Type == CharacterType.Operator ? MathErrorMessager.OperationBeforeParenthesisMessage(lastToken.Value.Value) : "";
    
    [ExcludeFromCodeCoverage]
    private static string CheckForEndingWithOperation(Character? lastToken) => 
        lastToken?.Type == CharacterType.Operator ? MathErrorMessager.EndingWithOperation : "";
}
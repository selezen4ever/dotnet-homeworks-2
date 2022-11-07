using System.Diagnostics.CodeAnalysis;
using Hw9.ErrorMessages;

namespace Hw9.Services.MathCalculator.Validator;

public class Validator : IValidator
{
    public string ValidateExpression(List<Character> characters)
    {
        if (characters.Count == 0)
            return MathErrorMessager.EmptyString;
        
        var unclosedBrackets = 0;
        Character? prevCharacter = null;

        foreach (var ch in characters)
        {
            switch (ch.Type)
            {
                case CharacterType.Number:
                    break;
                case CharacterType.Operator:
                    switch (prevCharacter)
                    {
                        case null:
                            return MathErrorMessager.StartingWithOperation;
                        case { Type: CharacterType.Operator }:
                            return MathErrorMessager.TwoOperationInRowMessage(prevCharacter.Value.Value, ch.Value);
                        case { Value: "(" } when ch.Value != "-":
                            return MathErrorMessager.InvalidOperatorAfterParenthesisMessage(ch.Value);
                    }
                    break;
                case CharacterType.Bracket:
                    switch (ch.Value)
                    {
                        case "(":
                            unclosedBrackets++;
                            break;
                        case ")":
                        {
                            unclosedBrackets--;
                            if (unclosedBrackets < 0)
                            {
                                return MathErrorMessager.IncorrectBracketsNumber;
                            }

                            break;
                        }
                    }
                    if (prevCharacter?.Type is CharacterType.Operator && ch.Value == ")")
                        return MathErrorMessager.OperationBeforeParenthesisMessage(prevCharacter.Value.Value);
                    break;
            }
            prevCharacter = ch;
        }
        if (prevCharacter?.Type == CharacterType.Operator)
            return MathErrorMessager.EndingWithOperation;
        
        return unclosedBrackets > 0 ? MathErrorMessager.IncorrectBracketsNumber : string.Empty;
    }
}
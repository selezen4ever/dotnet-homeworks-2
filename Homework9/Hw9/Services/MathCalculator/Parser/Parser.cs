using System.Globalization;
using Hw9.ErrorMessages;

namespace Hw9.Services.MathCalculator.Parser;

public class Parser : IParser
{
    private readonly HashSet<char> _brackets = new() {'(', ')'};
    private readonly HashSet<char>  _operations = new() {'+', '-', '/', '*'};

    public ParsedExpression ParseExpressionToTokens(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            return new ParsedExpression(new List<Character>());
        
        var characters = new List<Character>();
        var number = "";
        
        foreach (var ch in expression.Replace(" ", ""))
        {
            if (_brackets.Contains(ch))
            {
                TryAddToken(ref number, characters, ch, CharacterType.Bracket);
            }
            else if (_operations.Contains(ch))
            {
                if (!TryAddToken(ref number, characters, ch, CharacterType.Operator))
                    return new ParsedExpression(MathErrorMessager.NotNumberMessage(number));
            }
            else if (char.IsDigit(ch) || ch == '.')
                number += ch;
            else
                return new ParsedExpression(MathErrorMessager.UnknownCharacterMessage(ch));
        }

        if (!string.IsNullOrEmpty(number))
        {
            if (!double.TryParse(number, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
                return new ParsedExpression(MathErrorMessager.NotNumberMessage(number));
            
            characters.Add(new Character(CharacterType.Number, number));
        }
        
        return new ParsedExpression(characters);
    }

    private bool TryAddToken(ref string num, ICollection<Character> characters, char characterValue, CharacterType characterType)
    {
        if (!string.IsNullOrEmpty(num))
        {
            if (!double.TryParse(num, NumberStyles.AllowDecimalPoint,  CultureInfo.InvariantCulture, out _))
                return false;
            characters.Add(new Character(CharacterType.Number, num));
            num = "";
        }

        characters.Add(new Character(characterType, characterValue.ToString()));
        return true;
    }
}
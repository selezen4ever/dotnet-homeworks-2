using System.Globalization;
using System.Text;
using Hw10.ErrorMessages;

namespace Hw10.Services.MathCalculator.Parser;

public class Parser : IParser
{
    private readonly List<char>  _operations = new() {'+', '-', '/', '*'};

    public ParsedExpression GetParsedExpression(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            return new ParsedExpression(new List<Character>());
        
        var characters = new List<Character>();
        var number = "";
        
        foreach (var c in expression.Replace(" ", ""))
        {
            switch (c)
            {
                case '(':
                    TryAddCharacter(ref number, characters, c, CharacterType.OpeningBracket);
                    break;
                case ')':
                    TryAddCharacter(ref number, characters, c, CharacterType.ClosingBracket);
                    break;
                default:
                {
                    if (_operations.Contains(c))
                    {
                        if (!TryAddCharacter(ref number, characters, c, CharacterType.Operator))
                            return new ParsedExpression(MathErrorMessager.NotNumberMessage(number));
                    }
                    else if (char.IsDigit(c) || c == '.')
                        number += c;
                    else
                        return new ParsedExpression(MathErrorMessager.UnknownCharacterMessage(c));

                    break;
                }
            }
        }

        if (!string.IsNullOrEmpty(number))
        {
            if (!double.TryParse(number, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
                return new ParsedExpression(MathErrorMessager.NotNumberMessage(number));
            
            characters.Add(new Character(CharacterType.Number, number));
        }
        
        return new ParsedExpression(characters);
    }

     private static bool TryAddCharacter(ref string number, ICollection<Character> characters, char c, CharacterType type)
    {
        if (!string.IsNullOrEmpty(number))
        {
            if (!double.TryParse(number, NumberStyles.AllowDecimalPoint,  CultureInfo.InvariantCulture, out _))
                return false;
            characters.Add(new Character(CharacterType.Number, number));
            number = "";
        }

        characters.Add(new Character(type, c.ToString()));
        return true;
    }
}
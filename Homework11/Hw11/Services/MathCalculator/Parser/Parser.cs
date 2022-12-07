using System.Globalization;
using Hw11.ErrorMessages;
using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator.Parser;

public class Parser : IParser
{
    private readonly List<char>  _operations = new() {'+', '-', '/', '*'};

    public List<Character> GetParsedExpression(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            return new List<Character>();
        
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
                            throw new InvalidNumberException(MathErrorMessager.NotNumberMessage(number));
                    }
                    else if (char.IsDigit(c) || c == '.')
                        number += c;
                    else
                        throw new InvalidSymbolException(MathErrorMessager.UnknownCharacterMessage(c));

                    break;
                }
            }
        }

        if (!string.IsNullOrEmpty(number))
        {
            if (!double.TryParse(number, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
                throw new InvalidNumberException(MathErrorMessager.NotNumberMessage(number));
            
            characters.Add(new Character(CharacterType.Number, number));
        }
        
        return characters;
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
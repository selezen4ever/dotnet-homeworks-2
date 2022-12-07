namespace Hw11.Services.MathCalculator;

public struct Character
{
    public readonly CharacterType Type;
    public readonly string Value;

    public Character(CharacterType type, string value)
    {
        Type = type;
        Value = value;
    }
}

public enum CharacterType
{
    OpeningBracket, 
    ClosingBracket,
    Number, 
    Operator
}
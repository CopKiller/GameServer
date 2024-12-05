namespace Core.Database.Consistency;

public static class CharactersLimitations
{
    public const int MaxNameCharacters = 20;
    public const int MinNameCharacters = 3;

    public const int MaxPasswordCharacters = 20;
    public const int MinPasswordCharacters = 6;

    public const int MaxEmailCharacters = 50;
    public const int MinEmailCharacters = 6;

    public const int BirthDateCharacters = 10;
}
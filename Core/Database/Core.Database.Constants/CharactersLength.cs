using System.Text.RegularExpressions;

namespace Core.Database.Constants;

public static class CharactersLength
{
    public const int MaxUsernameLength = 30;
    public const int MinUsernameLength = 3;
    
    public const int MaxPasswordLength = 30;
    public const int MinPasswordLength = 6;
    
    public const int MaxEncryptedPasswordLength = 60;
    
    public const int MaxNameLength = 20;
    public const int MinNameLength = 3;

    public const int MaxEmailLength = 255;
    public const int MinEmailLength = 6;
    
    public const int MinYear = 1900;
    public const int MaxYear = 2021;
}
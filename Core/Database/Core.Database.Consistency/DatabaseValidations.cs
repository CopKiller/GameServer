using System.Text.RegularExpressions;
using Core.Database.Interfaces.Enum;
using static Core.Database.Consistency.CharactersLimitations;
using static System.String;

namespace Core.Database.Consistency;

public static class DatabaseValidations
{
    private static readonly Regex MyRegex = new("^[a-zA-Z0-9_]+$");

    public static bool IsValidName(this string name)
    {
        if (IsNullOrEmpty(name) ||
            (name.Length > MaxNameCharacters) ||
            (name.Length < MinNameCharacters))
        {
            return false;
        }

        return MyRegex.IsMatch(name);
    }

    public static bool IsValidPassword(this string pass)
    {
        return !IsNullOrEmpty(pass) &&
               pass.Length is <= MaxPasswordCharacters and >= MinPasswordCharacters;
    }

    public static bool IsValidRetypePassword(this string retypePass, string originalPass)
    {
        return !IsNullOrEmpty(originalPass) && !IsNullOrEmpty(retypePass) && originalPass == retypePass &&
               originalPass.Length is <= MaxPasswordCharacters and >= MinPasswordCharacters;
    }

    public static bool IsValidEmail(this string email)
    {
        if (IsNullOrEmpty(email) ||
            (email.Length > MaxEmailCharacters) ||
            (email.Length < MinEmailCharacters))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);

            return addr.Address == email && email.Contains('@');
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidGender(this int gender)
    {
        return gender is > (int)Gender.Undefined and <= (int)Gender.Female;
    }

    public static bool IsValidVocation(this int index)
    {
        return index is > (int)Vocation.None and <= (int)Vocation.Assassin;
    }

    public static bool IsValidBirthDate(this string date)
    {
        if (IsNullOrEmpty(date) || date.Length != BirthDateCharacters)
        {
            return false;
        }

        return Regex.IsMatch(date, @"^(\d{2})\/(\d{2})\/(\d{4})$") && DateTime.TryParse(date, out _);
    }
}
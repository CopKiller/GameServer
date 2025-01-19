using System.Text.RegularExpressions;

namespace Core.Database.Consistency.Validator.SyntaxValidator;

public static class MyRegex
{
    public static readonly Regex NameRegexCompiled = 
        new("^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
    
    public static readonly Regex EmailRegexCompiled = 
        new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

}
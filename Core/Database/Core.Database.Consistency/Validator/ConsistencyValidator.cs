
using System.Text.RegularExpressions;
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interface;

namespace Core.Database.Consistency.Validator;

public class ConsistencyValidator
{
    public readonly IValidatorResult ValidatorResult = new ValidatorResult();

    public void AddError(string error)
    {
        ValidatorResult.IsValid = false;
        ValidatorResult.AddError(error);
    }

    public bool ValidateStringLength(string? value, string propertyName, int? minLength, int? maxLength)
    {
        if (minLength == null && maxLength == null)
            return true;
        
        if (value == null)
            return false;
        
        if (minLength != null)
            if (value.Length < minLength)
            {
                AddError($"{propertyName} is too short");
                return false;
            }
        
        if (maxLength != null)
            if (value.Length > maxLength)
            {
                AddError($"{propertyName} is too long");
                return false;
            }
        
        return true;
    }

    public bool ValidateRegex(string? value, string propertyName, Regex? regex)
    {
        if (regex == null)
            return true;
        
        if (value == null)
            return false;
        
        if (!regex.IsMatch(value))
        {
            AddError($"{propertyName} is invalid format");
            return false;
        }
        
        return true;
    }
    
    public bool ValidateString(string? value, string propertyName, int? minLength = null, int? maxLength = null, Regex? regex = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AddError($"{propertyName} is null or empty");
            return false;
        }
        
        if (ValidateStringLength(value, propertyName, minLength, maxLength) &&
            ValidateRegex(value, propertyName,regex))
            return true;

        return false;
    }
}

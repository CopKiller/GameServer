
using System.Text.RegularExpressions;
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interfaces;

namespace Core.Database.Consistency.Validator;

public abstract class ConsistencyValidator<T> : IValidator<T> where T : class, IEntity
{
    internal readonly IValidatorResult ValidatorResult = new ValidatorResult(true);

    public abstract Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false);

    protected void AddError(string error)
    {
        ValidatorResult.IsValid = false;
        ValidatorResult.AddError(error);
    }
    
    private bool ValidateStringLength(string? value, string propertyName, int? minLength, int? maxLength)
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
    
    private bool ValidateRegex(string? value, string propertyName, Regex? regex)
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
    
    protected bool ValidateString(string? value, string propertyName, int? minLength = null, int? maxLength = null, Regex? regex = null)
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

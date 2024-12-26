using Core.Database.Consistency.Interface.Validator;
using Core.Database.Constants;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Validator.SyntaxValidator.Entities;

public class AccountSyntaxValidator<T> : ConsistencyValidator<T> where T : class, IAccountModel
{
    public override Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            AddError("Account Entity is null");
            return Task.FromResult(ValidatorResult);
        }
        
        // Username
        ValidateUsername(entity.Username);
        
        // Password
        ValidatePassword(entity.Password, isUpdate);

        // Email
        ValidateEmail(entity.Email);
        
        // BirthDate
        ValidateBirthDate(entity.BirthDate);
        
        // CreatedAt
        ValidateCreatedAt(entity.CreatedAt);

        return Task.FromResult(ValidatorResult);
    }
    
    public IValidatorResult ValidateUsername(string username)
    {
        ValidateString(username, "Username", CharactersLength.MinUsernameLength, CharactersLength.MaxUsernameLength, MyRegex.NameRegexCompiled);
        return ValidatorResult;
    }
    
    public IValidatorResult ValidatePassword(string password, bool isUpdate = false)
    {
        if (isUpdate && password.Length == CharactersLength.MaxEncryptedPasswordLength)
            ValidateString(password, "Password", CharactersLength.MaxEncryptedPasswordLength, CharactersLength.MaxEncryptedPasswordLength);
        else
            ValidateString(password, "Password", CharactersLength.MinPasswordLength, CharactersLength.MaxPasswordLength);
        
        return ValidatorResult;
    }
    
    public IValidatorResult ValidateEmail(string email)
    {
        ValidateString(email, "Email", CharactersLength.MinEmailLength, CharactersLength.MaxEmailLength, MyRegex.EmailRegexCompiled);
        return ValidatorResult;
    }
    
    public IValidatorResult ValidateBirthDate(DateOnly? birthDate)
    {
        if (birthDate == null || birthDate.Value.Year is < CharactersLength.MinYear or > CharactersLength.MaxYear)
        {
            AddError($"BirthDate {birthDate} is invalid");
        }
        return ValidatorResult;
    }
    
    public IValidatorResult ValidateCreatedAt(DateTime? createdAt)
    {
        if (createdAt == null)
        {
            AddError($"CreatedAt {createdAt} is invalid");
        }
        return ValidatorResult;
    }
    
    public void ClearErrors()
    {
        ValidatorResult.Errors.Clear();
        ValidatorResult.IsValid = true;
    }
}
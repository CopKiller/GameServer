using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.SyntaxValidator;
using Core.Database.Constants;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Validator.SyntaxValidator.Entities;

public class AccountSyntaxValidator<T> : IAccountSyntaxValidator<T> where T : class, IAccountModel
{
    private readonly ConsistencyValidator _validator = new();
    
    public IValidatorResult Validate(T? entity, bool isUpdate = false)
    {
        ClearErrors();
        
        if (entity == null)
        {
            _validator.AddError("Account Entity is null");
            return _validator.ValidatorResult;
        }
        
        // Username
        ValidateUsername(entity.Username);
        
        // Password
        ValidatePassword(entity.Password, isUpdate);
        
        // Email
        ValidateEmail(entity.Email);
        
        // BirthDate
        ValidateBirthDate(entity.BirthDate);
        
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateUsername(string username)
    {
        _validator.ValidateString(username, "Username", CharactersLength.MinUsernameLength, CharactersLength.MaxUsernameLength, MyRegex.NameRegexCompiled);
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidatePassword(string password, bool isUpdate = false)
    {
        if (isUpdate && password.Length == CharactersLength.MaxEncryptedPasswordLength)
            _validator.ValidateString(password, "Password", CharactersLength.MaxEncryptedPasswordLength, CharactersLength.MaxEncryptedPasswordLength);
        else
            _validator.ValidateString(password, "Password", CharactersLength.MinPasswordLength, CharactersLength.MaxPasswordLength);
        
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateEmail(string email)
    {
        _validator.ValidateString(email, "Email", CharactersLength.MinEmailLength, CharactersLength.MaxEmailLength, MyRegex.EmailRegexCompiled);
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateBirthDate(DateOnly? birthDate)
    {
        if (birthDate == null || birthDate.Value.Year is < CharactersLength.MinYear or > CharactersLength.MaxYear)
        {
            _validator.AddError($"BirthDate {birthDate} is invalid");
        }
        return _validator.ValidatorResult;
    }
    
    public void ClearErrors()
    {
        _validator.ValidatorResult.Errors.Clear();
        _validator.ValidatorResult.IsValid = true;
    }
}
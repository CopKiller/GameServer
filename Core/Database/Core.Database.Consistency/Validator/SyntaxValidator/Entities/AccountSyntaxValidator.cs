using Core.Database.Consistency.Interface.Validator;
using Core.Database.Constants;
using Core.Database.Interfaces.Account;

namespace Core.Database.Consistency.Validator.SyntaxValidator.Entities;

internal class AccountSyntaxValidator<T> : ConsistencyValidator<T> where T : class, IAccountModel
{
    public override Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            AddError("Account Entity is null");
            return Task.FromResult(ValidatorResult);
        }
        
        // Username
        ValidateString(entity.Username, "Username", CharactersLength.MinUsernameLength, CharactersLength.MaxUsernameLength, MyRegex.NameRegexCompiled);

        // Password
        if (isUpdate && entity.Password.Length == CharactersLength.MaxEncryptedPasswordLength)
        {
            ValidateString(entity.Password, "Password", CharactersLength.MaxEncryptedPasswordLength, CharactersLength.MaxEncryptedPasswordLength);
        }
        else
        {
            ValidateString(entity.Password, "Password", CharactersLength.MinPasswordLength, CharactersLength.MaxPasswordLength, MyRegex.NameRegexCompiled);
        }

        // Email
        ValidateString(entity.Email, "Email", CharactersLength.MinEmailLength, CharactersLength.MaxEmailLength, MyRegex.EmailRegexCompiled);
        
        // BirthDate
        if (entity?.BirthDate == null || entity?.BirthDate.Year is < CharactersLength.MinYear or > CharactersLength.MaxYear)
        {
            AddError($"BirthDate {entity?.BirthDate} is invalid");
        }
        
        // CreatedAt
        if (entity?.CreatedAt == null)
        {
            AddError($"CreatedAt {entity?.CreatedAt} is invalid");
        }

        return Task.FromResult(ValidatorResult);
    }
}
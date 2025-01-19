using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Interface.Validator.SyntaxValidator;

public interface IAccountSyntaxValidator<in T> where T : class, IAccountModel
{
    IValidatorResult Validate(T? entity, bool isUpdate = false);
    IValidatorResult ValidateUsername(string username);
    IValidatorResult ValidatePassword(string password, bool isUpdate = false);
    IValidatorResult ValidateEmail(string email);
    IValidatorResult ValidateBirthDate(DateOnly? birthDate);
    void ClearErrors();
}
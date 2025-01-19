using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Interface.Validator.DataValidator;

public interface IAccountDataValidator<in T> where T : class, IAccountModel
{
    Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false);
    Task<IValidatorResult> ValidateUsername(string username);
    Task<IValidatorResult> ValidateEmail(string email);
}
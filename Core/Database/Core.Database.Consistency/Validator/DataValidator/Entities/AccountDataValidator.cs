using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interface;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Validator.DataValidator.Entities;

internal class AccountDataValidator<T>(IRepository<T> repository) : ConsistencyValidator<T> where T : class, IAccountModel
{
    public override async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            AddError("Account Entity is null");
            return ValidatorResult;
        }
        
        // Username
        if (await repository.ExistEntityAsync(x => x.Username.Equals(entity.Username, StringComparison.CurrentCultureIgnoreCase)))
        {
            if (!isUpdate)
                AddError($"Username {entity.Username} already exists");
        }
        else
        {
            if (isUpdate)
                AddError($"Username {entity.Username} not found for update this account");
        }
        
        // Email
        if (await repository.ExistEntityAsync(x => x.Email.Equals(entity.Email, StringComparison.CurrentCultureIgnoreCase)))
        {
            if (!isUpdate)
                AddError("Email already exists");
        }
        else
        {
            if (isUpdate)
                AddError($"Email {entity.Email} not found for update this account");
        }
        
        return ValidatorResult;
    }
}
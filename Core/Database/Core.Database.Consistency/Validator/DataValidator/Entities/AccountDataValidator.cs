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
        await ValidateUsername(entity.Username, isUpdate);
        
        // Email
        await ValidateEmail(entity.Email, isUpdate);
        
        return ValidatorResult;
    }
    
    public async Task<IValidatorResult> ValidateUsername(string username, bool isUpdate = false)
    {
        if (await repository.ExistEntityAsync(x => x.Username.ToLower() == username.ToLower()))
        {
            if (!isUpdate)
                AddError($"Username {username} already exists");
        }
        else
        {
            if (isUpdate)
                AddError($"Username {username} not found for update this account");
        }
        
        return ValidatorResult;
    }
    
    public async Task<IValidatorResult> ValidateEmail(string email, bool isUpdate = false)
    {
        if (await repository.ExistEntityAsync(x => x.Email.ToLower() == email.ToLower()))
        {
            if (!isUpdate)
                AddError($"Email {email} already exists");
        }
        else
        {
            if (isUpdate)
                AddError($"Email {email} not found for update this account");
        }
        
        return ValidatorResult;
    }
}
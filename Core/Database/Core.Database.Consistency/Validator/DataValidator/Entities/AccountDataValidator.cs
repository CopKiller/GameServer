using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Interface;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Validator.DataValidator.Entities;

public class AccountDataValidator<T>(IRepository<T> repository) : IAccountDataValidator<T> where T : class, IAccountModel
{
    private readonly ConsistencyValidator _validator = new();
    
    public async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            _validator.AddError("Account Entity is null");
            return _validator.ValidatorResult;
        }
        
        // Username
        await ValidateUsername(entity.Username, isUpdate);
        
        // Email
        await ValidateEmail(entity.Email, isUpdate);
        
        return _validator.ValidatorResult;
    }
    
    public async Task<IValidatorResult> ValidateUsername(string username, bool isUpdate = false)
    {
        if (await repository.ExistEntityAsync(x => x.Username.ToLower() == username.ToLower()))
        {
            if (!isUpdate)
                _validator.AddError($"Username {username} already exists");
        }
        else
        {
            if (isUpdate)
                _validator.AddError($"Username {username} not found for update this account");
        }
        
        return _validator.ValidatorResult;
    }
    
    public async Task<IValidatorResult> ValidateEmail(string email, bool isUpdate = false)
    {
        if (await repository.ExistEntityAsync(x => x.Email.ToLower() == email.ToLower()))
        {
            if (!isUpdate)
                _validator.AddError($"Email {email} already exists");
        }
        else
        {
            if (isUpdate)
                _validator.AddError($"Email {email} not found for update this account");
        }
        
        return _validator.ValidatorResult;
    }
}
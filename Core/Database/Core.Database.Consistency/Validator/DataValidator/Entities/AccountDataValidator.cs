using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Interface;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency.Validator.DataValidator.Entities;

public class AccountDataValidator<T>(IRepository<T> repository)
    : IAccountDataValidator<T> where T : class, IAccountModel
{
    private readonly ConsistencyValidator _validator = new();

    public async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            _validator.AddError("Account Entity is null");
            return _validator.ValidatorResult;
        }

        if (isUpdate)
        {
            var account = await repository.GetEntityAsync(x => x.Username.ToLower() == entity.Username.ToLower());

            if (account == null)
            {
                _validator.AddError($"Account not found");
                return _validator.ValidatorResult;
            }

            if (entity.Id != account.Id)
            {
                _validator.AddError("Account ID modification is not allowed.");
                return _validator.ValidatorResult;
            }
            
            // Email
            if (account.Email.ToLower() != entity.Email.ToLower())
            {
                await ValidateEmail(entity.Email);
            }
        }
        else
        {
            // Username
            await ValidateUsername(entity.Username);

            // Email
            await ValidateEmail(entity.Email);
        }

        return _validator.ValidatorResult;
    }

    public async Task<IValidatorResult> ValidateUsername(string username)
    {
        var account = await repository.GetEntityAsync(x => x.Username.ToLower() == username.ToLower());

        if (account == null) return _validator.ValidatorResult;
        
        _validator.AddError($"Username {username} already exists");
        return _validator.ValidatorResult;
    }

    public async Task<IValidatorResult> ValidateEmail(string email)
    {
        if (await repository.ExistEntityAsync(x => x.Email.ToLower() == email.ToLower()))
        {
            _validator.AddError($"Email {email} already exists");
        }

        return _validator.ValidatorResult;
    }
}
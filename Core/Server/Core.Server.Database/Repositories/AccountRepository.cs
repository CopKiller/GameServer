using Core.Cryptography.Interface;
using Core.Database.Consistency;
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator;
using Core.Database.Interface;
using Core.Database.Interface.Account;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories;

public class AccountRepository<T>(IRepository<T> context, ICrypto crypto) : IAccountRepository<T>
    where T : class, IAccountModel
{
    private readonly AccountValidator<T> _validator = new(context);

    public async Task<(IValidatorResult, T?)> AddAccountAsync(T account)
    {
        var validationResult = await _validator.ValidateAsync(account);
        
        if (!validationResult.IsValid)
            return (validationResult, null);

        account.Password = crypto.HashString(account.Password);

        var result = await context.AddAsync(account);

        var changes = await context.SaveChangesAsync() > 0;
        
        if (!changes)
            validationResult.AddError("Failed to add account");
        
        return (validationResult, result);
    }

    public async Task<(IValidatorResult, T?)> GetAccountAsync(string username, string password)
    {
        var accountResult = await context.GetEntityAsync(a => a.Username == username, model => model.Players);

        var validator = new ValidatorResult(true);

        if (accountResult == null)
        {
            validator.AddError("Account not found");
            return (validator, null);
        }

        if (!crypto.CompareHash(password, accountResult.Password))
        {
            validator.AddError("Password is incorrect");
            return (validator, null);
        }

        return (validator, accountResult);
    }

    public async Task<IValidatorResult> UpdateAccountAsync(T account)
    {
        var validationResult = await _validator.ValidateAsync(account, true);
        
        if (!validationResult.IsValid)
            return validationResult;
        
        context.Update(account);

        var result = await context.SaveChangesAsync() > 0;
        
        if (!result)
            validationResult.AddError("Failed to update account");
        
        return validationResult;
    }
}
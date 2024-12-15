using Core.Cryptography.Interface;
using Core.Database.Consistency;
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories;

public class AccountRepository<T>(IRepository<T> context, ICrypto crypto) : IAccountRepository<T>
    where T : class, IAccountModel
{
    private IRepository<T> Context => context;
    
    private readonly AccountValidator<T> _validator = new(context);

    public async Task<(IValidatorResult, T?)> AddAccountAsync(T account)
    {
        var validationResult = await _validator.ValidateAsync(account);
        
        if (!validationResult.IsValid)
            return (validationResult, null);

        account.Password = crypto.HashString(account.Password);

        var result = await Context.AddAsync(account);

        var changes = await Context.SaveChangesAsync() > 0;
        
        if (!changes)
            validationResult.AddError("Failed to add account");
        
        return (validationResult, result);
    }

    public async Task<(IValidatorResult, T?)> GetAccountAsync(string username, string password)
    {
        var accountResult = await Context.GetEntityAsync(a => a.Username == username, model => model.Players);

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
        
        Context.Update(account);

        var result = await Context.SaveChangesAsync() > 0;
        
        if (!result)
            validationResult.AddError("Failed to update account");
        
        return validationResult;
    }
}
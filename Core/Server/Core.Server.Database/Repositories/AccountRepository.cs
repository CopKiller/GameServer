using Core.Cryptography.Interface;
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Consistency.Interface.Validator.SyntaxValidator;
using Core.Database.Interface;
using Core.Database.Interface.Account;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories;

public class AccountRepository<T>(
    IRepository<T> context, 
    ICrypto crypto,
    IAccountSyntaxValidator<T> syntaxValidator,
    IAccountDataValidator<T> dataValidator) 
    : IAccountRepository<T> where T : class, IAccountModel
{
    public async Task<(IValidatorResult, T?)> AddAccountAsync(T account)
    {
        var syntaxValidationResult = syntaxValidator.Validate(account);
        
        if (!syntaxValidationResult.IsValid)
            return (syntaxValidationResult, null);
        
        var validationResult = await dataValidator.ValidateAsync(account);
        
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
        
        var validator = new ValidatorResult();

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
        var validatorResult = new ValidatorResult();

        // Validar sintaxe e dados
        var syntaxValidationResult = syntaxValidator.Validate(account);
        if (!syntaxValidationResult.IsValid)
            return syntaxValidationResult;
    
        var dataValidationResult = await dataValidator.ValidateAsync(account);
        if (!dataValidationResult.IsValid)
            return dataValidationResult;

        // Persistir no banco
        context.Update(account);
        var result = await context.SaveChangesAsync() > 0;
    
        if (!result)
            validatorResult.AddError("Failed to update account");

        return validatorResult;
    }

}
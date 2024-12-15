
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator;
using Core.Database.Consistency.Validator.DataValidator.Entities;
using Core.Database.Consistency.Validator.SyntaxValidator.Entities;
using Core.Database.Interface;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency;

public class AccountValidator<T>(IRepository<T> repository) where T : class, IAccountModel
{
    private readonly IValidator<T> _syntaxValidator = new AccountSyntaxValidator<T>();
    private readonly IValidator<T> _dataValidator = new AccountDataValidator<T>(repository);
    
    public async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        var syntaxValidationResult = await _syntaxValidator.ValidateAsync(entity, isUpdate);
        
        if (!syntaxValidationResult.IsValid)
        {
            return syntaxValidationResult;
        }
        
        var dataValidationResult = await _dataValidator.ValidateAsync(entity, isUpdate);
        
        return dataValidationResult;
    }
}
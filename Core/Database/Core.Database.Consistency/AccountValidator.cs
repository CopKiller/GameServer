
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator;
using Core.Database.Consistency.Validator.DataValidator.Entities;
using Core.Database.Consistency.Validator.SyntaxValidator.Entities;
using Core.Database.Interface;
using Core.Database.Interface.Account;

namespace Core.Database.Consistency;

public class AccountValidator<T>(IRepository<T> repository) where T : class, IAccountModel
{
    private readonly AccountSyntaxValidator<T> _syntaxValidator = new();
    private readonly AccountDataValidator<T> _dataValidator = new(repository);
    
    public async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        var syntaxValidationResult = _syntaxValidator.Validate(entity, isUpdate);
        
        if (!syntaxValidationResult.IsValid)
        {
            return syntaxValidationResult;
        }
        
        var dataValidationResult = await _dataValidator.ValidateAsync(entity, isUpdate);
        
        return dataValidationResult;
    }
}
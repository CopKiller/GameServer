using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator;
using Core.Database.Consistency.Validator.DataValidator.Entities;
using Core.Database.Consistency.Validator.SyntaxValidator.Entities;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;

namespace Core.Database.Consistency;

public class PlayerValidator<T>(IRepository<T> repository) where T : class, IPlayerModel
{
    private readonly IValidator<T> _syntaxValidator = new PlayerSyntaxValidator<T>();
    private readonly IValidator<T> _dataValidator = new PlayerDataValidator<T>(repository);
    
    public async Task<IValidatorResult> ValidateAsync(T? entity)
    {
        var syntaxValidationResult = await _syntaxValidator.ValidateAsync(entity);
        
        if (!syntaxValidationResult.IsValid)
        {
            return syntaxValidationResult;
        }
        
        var dataValidationResult = await _dataValidator.ValidateAsync(entity);
        
        return dataValidationResult;
    }
}
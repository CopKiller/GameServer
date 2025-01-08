using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Consistency.Interface.Validator.SyntaxValidator;
using Core.Database.Consistency.Validator;
using Core.Database.Consistency.Validator.DataValidator.Entities;
using Core.Database.Consistency.Validator.SyntaxValidator.Entities;
using Core.Database.Interface;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency;

public class PlayerValidator<T>(IRepository<T> repository) where T : class, IPlayerModel
{
    private readonly IPlayerSyntaxValidator<T> _syntaxValidator = new PlayerSyntaxValidator<T>();
    private readonly IPlayerDataValidator<T> _dataValidator = new PlayerDataValidator<T>(repository);
    
    public async Task<IValidatorResult> ValidateAsync(T? entity)
    {
        var syntaxValidationResult = _syntaxValidator.Validate(entity);
        
        if (!syntaxValidationResult.IsValid)
        {
            return syntaxValidationResult;
        }
        
        var dataValidationResult = await _dataValidator.ValidateAsync(entity);
        
        return dataValidationResult;
    }
}
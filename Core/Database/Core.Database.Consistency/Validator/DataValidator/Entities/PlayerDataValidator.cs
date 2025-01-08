using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Interface;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Validator.DataValidator.Entities;

public class PlayerDataValidator<T>(IRepository<T> repository) : IPlayerDataValidator<T> where T : class, IPlayerModel
{
    private readonly ConsistencyValidator _validator = new();
    
    public async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            _validator.AddError("Player is null");
            return _validator.ValidatorResult;
        }
        
        // Name
        await ValidateName(entity.Name, isUpdate);
        
        return _validator.ValidatorResult;
    }
    
    public async Task<IValidatorResult> ValidateName(string name, bool isUpdate = false)
    {
        if (await repository.ExistEntityAsync(x => x.Name.ToLower() == name.ToLower()))
        {
            if (!isUpdate)
                _validator.AddError($"Player name {name} already exists");
        }
        else
        {
            if (isUpdate)
                _validator.AddError($"Player name {name} not found for update this player");
        }
        
        return _validator.ValidatorResult;
    }
}
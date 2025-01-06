using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interface;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Validator.DataValidator.Entities;

internal class PlayerDataValidator<T>(IRepository<T> repository) : ConsistencyValidator<T> where T : class, IPlayerModel
{
    public override async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            AddError("Player is null");
            return ValidatorResult;
        }
        
        // Name
        await ValidateName(entity.Name, isUpdate);
        
        return ValidatorResult;
    }
    
    public async Task<IValidatorResult> ValidateName(string name, bool isUpdate = false)
    {
        if (await repository.ExistEntityAsync(x => x.Name.ToLower() == name.ToLower()))
        {
            if (!isUpdate)
                AddError($"Player name {name} already exists");
        }
        else
        {
            if (isUpdate)
                AddError($"Player name {name} not found for update this player");
        }
        
        return ValidatorResult;
    }
}
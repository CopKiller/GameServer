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
        
        if (await repository.ExistEntityAsync(x => x.Name.Equals(entity.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            if (!isUpdate)
                AddError($"Player name {entity.Name} already exists");
        }
        else
        {
            if (isUpdate)
                AddError($"Player name {entity.Name} not found for update this player");
        }
        
        return ValidatorResult;
    }
}
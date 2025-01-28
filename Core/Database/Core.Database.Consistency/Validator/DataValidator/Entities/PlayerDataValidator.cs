using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Interface;
using Core.Database.Interface.Account;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Validator.DataValidator.Entities;

public class PlayerDataValidator<T>(IRepository<T> repository) : IPlayerDataValidator<T> where T : class, IPlayerModel
{
    private readonly ConsistencyValidator _validator = new();
    
    public async Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        ClearErrors();
        
        if (entity == null)
        {
            _validator.AddError("Player is null");
            return _validator.ValidatorResult;
        }

        if (!isUpdate)
        {
            if (await repository.ExistEntityAsync(x => x.Name.ToLower() == entity.Name.ToLower()))
                _validator.AddError($"Player name {entity.Name} already exists");

            if (await repository.ExistEntityAsync(x => x.SlotNumber == entity.SlotNumber))
                _validator.AddError($"Player slot number {entity.SlotNumber} already exists");

            return _validator.ValidatorResult;
        }
        
        var existingPlayer = await repository.GetEntityAsync(x => x.Id == entity.Id);
        if (existingPlayer == null)
        {
            _validator.AddError($"Player with ID {entity.Id} does not exist");
            return _validator.ValidatorResult;
        }

        if (!existingPlayer.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase) &&
            await repository.ExistEntityAsync(x => x.Name.ToLower() == entity.Name.ToLower()))
        {
            _validator.AddError($"Player name {entity.Name} already exists");
        }

        if (existingPlayer.SlotNumber != entity.SlotNumber &&
            await repository.ExistEntityAsync(x => x.SlotNumber == entity.SlotNumber))
        {
            _validator.AddError($"Player slot number {entity.SlotNumber} already exists");
        }

        return _validator.ValidatorResult;
    }
    
    public void ClearErrors()
    {
        _validator.ValidatorResult.Errors.Clear();
        _validator.ValidatorResult.IsValid = true;
    }
}
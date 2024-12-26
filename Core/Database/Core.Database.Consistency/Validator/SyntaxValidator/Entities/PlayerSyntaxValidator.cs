using Core.Database.Consistency.Interface.Validator;
using Core.Database.Constants;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Validator.SyntaxValidator.Entities;

public class PlayerSyntaxValidator<T> : ConsistencyValidator<T> where T : class, IPlayerModel
{
    public override Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            AddError("Player is null");
            return Task.FromResult(ValidatorResult);
        }
        
        // Name
        ValidateName(entity.Name);

        // Created At
        ValidateCreatedAt(entity.CreatedAt);
        
        // Children
        ValidateStats(entity.Stats);
        ValidateVitals(entity.Vitals);
        ValidatePosition(entity.Position);

        return Task.FromResult(ValidatorResult);
    }
    
    public IValidatorResult ValidateName(string name)
    {
        ValidateString(name, "Name", CharactersLength.MinNameLength, CharactersLength.MaxNameLength, MyRegex.NameRegexCompiled);
        return ValidatorResult;
    }
    
    public IValidatorResult ValidateCreatedAt(DateOnly createdAt)
    {
        if (createdAt == default)
        {
            AddError("Created Date is null or empty");
        }
        return ValidatorResult;
    }
    
    public IValidatorResult ValidateStats(IStats? stats)
    {
        if (stats == null)
        {
            AddError("Stats is null");
        }
        return ValidatorResult;
    }
    
    public IValidatorResult ValidateVitals(IVitals? vitals)
    {
        if (vitals == null)
        {
            AddError("Vitals is null");
        }
        return ValidatorResult;
    }
    
    public IValidatorResult ValidatePosition(IPosition? position)
    {
        if (position == null)
        {
            AddError("Position is null");
        }
        return ValidatorResult;
    }
    
    public void ClearErrors()
    {
        ValidatorResult.Errors.Clear();
        ValidatorResult.IsValid = true;
    }
}
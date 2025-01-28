using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.SyntaxValidator;
using Core.Database.Constants;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Validator.SyntaxValidator.Entities;

public class PlayerSyntaxValidator<T> : IPlayerSyntaxValidator<T> where T : class, IPlayerModel
{
    private readonly ConsistencyValidator _validator = new();
    
    public IValidatorResult Validate(T? entity, bool isUpdate = false)
    {
        ClearErrors();
        
        if (entity == null)
        {
            _validator.AddError("Player is null");
            return _validator.ValidatorResult;
        }
        
        // Name
        ValidateName(entity.Name);
        
        // Children
        ValidateStats(entity.Stats);
        ValidateVitals(entity.Vitals);
        ValidatePosition(entity.Position);
        ValidateSlotNumber(entity.SlotNumber);

        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateName(string name)
    {
        _validator.ValidateString(name, "Name", CharactersLength.MinNameLength, CharactersLength.MaxNameLength, MyRegex.NameRegexCompiled);
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateStats(IStats? stats)
    {
        if (stats == null)
        {
            _validator.AddError("Stats is null");
        }
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateVitals(IVitals? vitals)
    {
        if (vitals == null)
        {
            _validator.AddError("Vitals is null");
        }
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidatePosition(IPosition? position)
    {
        if (position == null)
        {
            _validator.AddError("Position is null");
        }
        return _validator.ValidatorResult;
    }
    
    public IValidatorResult ValidateSlotNumber(byte slotNumber)
    {
        if (slotNumber >= DatabaseLimiter.MaxPlayersPerAccount)
        {
            _validator.AddError($"Slot number {slotNumber} is invalid");
        }
        return _validator.ValidatorResult;
    }
    
    public void ClearErrors()
    {
        _validator.ValidatorResult.Errors.Clear();
        _validator.ValidatorResult.IsValid = true;
    }
}
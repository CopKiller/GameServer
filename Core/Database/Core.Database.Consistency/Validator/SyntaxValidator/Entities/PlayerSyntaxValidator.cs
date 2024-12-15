using Core.Database.Consistency.Interface.Validator;
using Core.Database.Constants;
using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Validator.SyntaxValidator.Entities;

internal class PlayerSyntaxValidator<T> : ConsistencyValidator<T> where T : class, IPlayerModel
{
    public override Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false)
    {
        if (entity == null)
        {
            AddError("Player is null");
            return Task.FromResult(ValidatorResult);
        }
        
        // Name
        ValidateString(entity.Name, "Name", CharactersLength.MinNameLength, CharactersLength.MaxNameLength, MyRegex.NameRegexCompiled);

        // Created At
        if (entity.CreatedAt == default)
        {
            AddError("Created Date is null or empty");
        }
        
        // Children
        if (entity?.Stats == null)
        {
            AddError("Stats is null");
        }
        if (entity?.Vitals == null)
        {
            AddError("Vitals is null");
        }
        if (entity?.Position == null)
        {
            AddError("Position is null");
        }

        return Task.FromResult(ValidatorResult);
    }
}
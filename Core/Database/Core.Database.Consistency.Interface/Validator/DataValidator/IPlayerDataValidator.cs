using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Interface.Validator.DataValidator;

public interface IPlayerDataValidator<in T> where T : class, IPlayerModel
{
    Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false);
    Task<IValidatorResult> ValidateName(string name, bool isUpdate = false);
}
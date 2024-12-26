
namespace Core.Database.Consistency.Interface.Validator;

public interface IValidator<in T>
{
    Task<IValidatorResult> ValidateAsync(T? entity, bool isUpdate = false);
    
}
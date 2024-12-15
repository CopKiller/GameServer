using Core.Database.Consistency.Interface.Validator;

namespace Core.Database.Consistency.Validator;

public class ValidatorResult(bool isValid) : IValidatorResult
{
    public bool IsValid { get; set; } = isValid;
    public List<string> Errors { get; } = new();

    public void AddError(string error)
    {
        Errors.Add(error);
        IsValid = false;
    }
}
using Core.Database.Consistency.Interface.Validator;

namespace Core.Server.Database;

public class ValidatorResult : IValidatorResult
{
    public bool IsValid { get; set; } = true;
    public List<string> Errors { get; } = [];
    public void AddError(string error)
    {
        Errors.Add(error);
        IsValid = false;
    }
}
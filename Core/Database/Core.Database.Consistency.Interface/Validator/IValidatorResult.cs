namespace Core.Database.Consistency.Interface.Validator;

public interface IValidatorResult
{
    bool IsValid { get; set; }
    List<string> Errors { get; }
    void AddError(string error);
}
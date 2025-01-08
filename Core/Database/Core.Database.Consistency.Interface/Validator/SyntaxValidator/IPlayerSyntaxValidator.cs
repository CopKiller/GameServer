using Core.Database.Interface.Player;

namespace Core.Database.Consistency.Interface.Validator.SyntaxValidator;

public interface IPlayerSyntaxValidator<in T> where T : class, IPlayerModel
{
    IValidatorResult Validate(T? entity, bool isUpdate = false);
    IValidatorResult ValidateName(string name);
    IValidatorResult ValidateStats(IStats? stats);
    IValidatorResult ValidateVitals(IVitals? vitals);
    IValidatorResult ValidatePosition(IPosition? position);
    void ClearErrors();
}
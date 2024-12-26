using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator.SyntaxValidator.Entities;
using Core.Database.Interface.Account;
using Core.Database.Interface.Player;

namespace Core.Client.Validator;

public static class EntitiesValidator
{
    private static readonly PlayerSyntaxValidator<IPlayerModel> _playerSyntaxValidator;
    
    private static readonly AccountSyntaxValidator<IAccountModel> _accountSyntaxValidator;
    
    static EntitiesValidator()
    {
        _playerSyntaxValidator = new PlayerSyntaxValidator<IPlayerModel>();
        _accountSyntaxValidator = new AccountSyntaxValidator<IAccountModel>();
    }
    
    public static IValidatorResult ValidatePlayerName(string name)
    {
        _playerSyntaxValidator.ClearErrors();
        return _playerSyntaxValidator.ValidateName(name);
    }
    
    public static IValidatorResult ValidateAccountUsername(string username)
    {
        _accountSyntaxValidator.ClearErrors();
        return _accountSyntaxValidator.ValidateUsername(username);
    }
    
    public static IValidatorResult ValidateAccountPassword(string password, bool isUpdate = false)
    {
        _accountSyntaxValidator.ClearErrors();
        return _accountSyntaxValidator.ValidatePassword(password, isUpdate);
    }
    
    public static IValidatorResult ValidateAccountEmail(string email)
    {
        _accountSyntaxValidator.ClearErrors();
        return _accountSyntaxValidator.ValidateEmail(email);
    }
    
    public static IValidatorResult ValidateAccountBirthDate(DateOnly? birthDate)
    {
        _accountSyntaxValidator.ClearErrors();
        return _accountSyntaxValidator.ValidateBirthDate(birthDate);
    }
}
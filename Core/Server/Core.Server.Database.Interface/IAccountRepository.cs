using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interface;
using Core.Database.Interface.Account;
using Core.Database.Interface.Player;

namespace Core.Server.Database.Interface;

public interface IAccountRepository<T> where T : class, IAccountModel
{
    Task<(IValidatorResult, T?)> AddAccountAsync(T account);
    Task<(IValidatorResult, T?)> GetAccountAsync(string username, string password);
    Task<IValidatorResult> UpdateAccountAsync(T account);
}
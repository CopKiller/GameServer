using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;

namespace Core.Server.Database.Interface;

public interface IAccountRepository<T> where T : class, IAccountModel
{
    Task<(IValidatorResult, T?)> AddAccountAsync(T account);
    Task<(IValidatorResult, T?)> GetAccountAsync(string username, string password);
    Task<IValidatorResult> UpdateAccountAsync(T account);
}
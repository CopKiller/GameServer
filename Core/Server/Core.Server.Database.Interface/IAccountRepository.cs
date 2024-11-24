using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;

namespace Core.Server.Database.Interface;

public interface IAccountRepository<T> where T : class, IAccountModel
{
    Task<(IDatabaseException, T?)> AddAccountAsync(T account);
    Task<(IDatabaseException, T?)> GetAccountAsync(string username, string password);
    Task<IDatabaseException> UpdateAccountAsync(T account);
    Task<IDatabaseException> EmailExistsAsync(string email);
    Task<IDatabaseException> UsernameExistsAsync(string username);
}
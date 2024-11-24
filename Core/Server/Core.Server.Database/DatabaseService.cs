
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Logger.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Server.Database.Interface;

namespace Core.Server.Database;

public class DatabaseService(IAccountRepository<AccountModel> accountRepository, IPlayerRepository<PlayerModel> playerRepository, ILogger<IDatabaseService> log) : IDatabaseService
{
    public IAccountRepository<AccountModel> GetAccountRepository()
    {
        return accountRepository;
    }
    
    public IPlayerRepository<PlayerModel> GetPlayerRepository()
    {
        return playerRepository;
    }
}
using Core.Database.Interface.Account;
using Core.Database.Interface.Player;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Logger.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Server.Database.Interface;

namespace Core.Server.Database;

public class DatabaseRepositories(
    IAccountRepository<AccountModel> accountRepository,
    IPlayerRepository<PlayerModel> playerRepository) 
    : IDatabaseService
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
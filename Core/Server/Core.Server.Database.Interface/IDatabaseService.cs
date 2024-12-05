using Core.Database.Interfaces.Account;
using Core.Database.Models.Account;
using Core.Database.Models.Player;

namespace Core.Server.Database.Interface;

public interface IDatabaseService
{
    IAccountRepository<AccountModel> GetAccountRepository();

    IPlayerRepository<PlayerModel> GetPlayerRepository();
}
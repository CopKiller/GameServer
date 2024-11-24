using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;

namespace Core.Server.Database.Interface;

public interface IPlayerRepository<T> where T : class, IPlayerModel
{
    Task<(IDatabaseException, IEnumerable<T>)> GetPlayersAsync(int accountId);
    Task<IDatabaseException> NameExistsAsync(string username);
    Task<IDatabaseException> UpdatePlayerAsync(T player);
}
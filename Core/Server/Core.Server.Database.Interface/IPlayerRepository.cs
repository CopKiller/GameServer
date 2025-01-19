using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interface;
using Core.Database.Interface.Player;

namespace Core.Server.Database.Interface;

public interface IPlayerRepository<T> where T : class, IPlayerModel
{
    Task<(IValidatorResult, ICollection<T>?)> GetPlayersAsync(int accountId);
    Task<(IValidatorResult, T?)> AddPlayerAsync(T player, int accountId);
    Task<IValidatorResult> UpdatePlayerAsync(T player);
}
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;

namespace Core.Server.Database.Interface;

public interface IPlayerRepository<T> where T : class, IPlayerModel
{
    Task<(IValidatorResult, ICollection<T>?)> GetPlayersAsync(int accountId);
    Task<IValidatorResult> UpdatePlayerAsync(T player);
}
using Core.Database.Interfaces;
using Core.Database.Interfaces.Player;
using Core.Server.Database.Exceptions;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories
{
    public class PlayerRepository<T>(IRepository<T> context) : IPlayerRepository<T>
        where T : class, IPlayerModel
    {
        private IRepository<T> Context => context;
        
        public async Task<(IDatabaseException, IEnumerable<T>)> GetPlayersAsync(int accountId)
        {
            // Recupera os jogadores do banco de dados
            var players = await Context.GetEntitiesAsync(
                p => p.AccountModelId == accountId,
                p => p.Position,
                p => p.Vitals,
                p => p.Stats
            );

            var enumerable = players as T[] ?? players.ToArray();
            
            if (!enumerable.Any())
                return (new DatabaseException(true, "Players not found"), enumerable);
            
            return (new DatabaseException(false, $"Players found Count:{enumerable.Count()}"), enumerable);
        }

        public async Task<IDatabaseException> NameExistsAsync(string username)
        {
            var result = await Context.ExistEntityAsync(p => p.Name == username);
            
            if (result)
                return new DatabaseException(true, "Player already exists");
            else
                return new DatabaseException(false, "Player not found");
        }

        public async Task<IDatabaseException> UpdatePlayerAsync(T player)
        {
            Context.Update(player);

            var result = await Context.SaveChangesAsync() > 0;
            
            if (result)
                return new DatabaseException(false, "Player updated");
            else
                return new DatabaseException(true, "Failed to update player");
        }
    }
}

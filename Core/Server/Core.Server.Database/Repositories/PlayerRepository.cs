using Core.Database.Consistency;
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Validator;
using Core.Database.Interface;
using Core.Database.Interface.Player;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories;

public class PlayerRepository<T>(IRepository<T> context) : IPlayerRepository<T>
    where T : class, IPlayerModel
{
    private IRepository<T> Context => context;
    
    private readonly PlayerValidator<T> _validator = new(context);

    public async Task<(IValidatorResult, ICollection<T>?)> GetPlayersAsync(int accountId)
    {
        // Recupera os jogadores do banco de dados
        var players = await Context.GetEntitiesAsync(
            p => p.AccountModelId == accountId,
            p => p.Position,
            p => p.Vitals,
            p => p.Stats
        );

        var validatorResult = new ValidatorResult(true);

        if (players == null)
        {
            validatorResult.AddError("Players not found");
            return (validatorResult, null);
        }
        
        return (validatorResult, players);
    }

    public async Task<IValidatorResult> UpdatePlayerAsync(T player)
    {
        var validatorResult = await _validator.ValidateAsync(player);
        
        Context.Update(player);

        var result = await Context.SaveChangesAsync() > 0;
        
        if (!result)
            validatorResult.AddError("Player not updated");
        
        return validatorResult;
    }
}
using Core.Database.Consistency.Interface.Validator;
using Core.Database.Consistency.Interface.Validator.DataValidator;
using Core.Database.Consistency.Interface.Validator.SyntaxValidator;
using Core.Database.Interface;
using Core.Database.Interface.Account;
using Core.Database.Interface.Player;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories;

public class PlayerRepository<T>(
    IRepository<T> context, 
    IPlayerSyntaxValidator<T> syntaxValidator,
    IPlayerDataValidator<T> dataValidator) 
    : IPlayerRepository<T> where T : class, IPlayerModel
{
    private IRepository<T> Context => context;

    public async Task<(IValidatorResult, ICollection<T>?)> GetPlayersAsync(int accountId)
    {
        // Recupera os jogadores do banco de dados
        var players = await Context.GetEntitiesAsync(
            p => p.AccountModelId == accountId,
            p => p.Position,
            p => p.Vitals,
            p => p.Stats
        );

        var validatorResult = new ValidatorResult();

        if (players != null) return (validatorResult, players);
        
        validatorResult.AddError("Players not found");
        return (validatorResult, null);

    }
    
    public async Task<(IValidatorResult, T?)> AddPlayerAsync(T player, int accountId)
    {
        var validatorResult = new ValidatorResult();
    
        // Validar sintaxe
        var syntaxValidationResult = syntaxValidator.Validate(player);
        if (!syntaxValidationResult.IsValid)
            return (syntaxValidationResult, null);

        // Validar dados relacionados ao player
        var dataValidationResult = await dataValidator.ValidateAsync(player);
        if (!dataValidationResult.IsValid)
            return (dataValidationResult, null);

        // Garantir que a Account existe antes de adicionar o Player
        var accountExists = await Context.ExistEntityAsync(a => a.Id == accountId);
        if (!accountExists)
        {
            validatorResult.AddError("Account not found");
            return (validatorResult, null);
        }

        // Associar o Player à conta usando apenas o Id
        player.AccountModelId = accountId;

        // Adicionar o Player diretamente ao banco
        await Context.AddAsync(player);
    
        var changes = await Context.SaveChangesAsync() > 0;
        if (!changes)
            validatorResult.AddError("Failed to add player");
    
        return (validatorResult, player);
    }

    public async Task<IValidatorResult> UpdatePlayerAsync(T player)
    {
        var syntaxValidatorResult = syntaxValidator.Validate(player);
        
        if (!syntaxValidatorResult.IsValid)
            return syntaxValidatorResult;
        
        var dataValidatorResult = await dataValidator.ValidateAsync(player);
        
        if (!dataValidatorResult.IsValid)
            return dataValidatorResult;
        
        Context.Update(player);

        var result = await Context.SaveChangesAsync() > 0;
        
        if (!result)
            dataValidatorResult.AddError("Player not updated");
        
        return dataValidatorResult;
    }
}
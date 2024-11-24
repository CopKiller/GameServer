using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;
using Core.Server.Database.Exceptions;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Repositories;

public class AccountRepository<T>(IRepository<T> context) : IAccountRepository<T>
where T : class, IAccountModel
{
    private IRepository<T> Context => context;
    
    public async Task<(IDatabaseException, T?)> AddAccountAsync(T account)
    {
        if (await Context.ExistEntityAsync(p => p.Username == account.Username))
            return (new DatabaseException(true, "Account already exists"), null);
        if (await Context.ExistEntityAsync(p => p.Email == account.Email))
            return (new DatabaseException(true, "E-mail already exists"), null);
        
        var result = await Context.AddAsync(account);
        
        var countChanges = await Context.SaveChangesAsync();
        
        if (countChanges > 0)
            return (new DatabaseException(false, $"Account: {account.Id} {account.Username} created!"), result);
        else
            return (new DatabaseException(true, "Failed to add account"), null);
    }
    
    public async Task<(IDatabaseException, T?)> GetAccountAsync(string username, string password)
    {
        
        var accountResult = await Context.GetEntityAsync(a => a.Username == username && a.Password == password, model => model.Players );
        
        if (accountResult == null)
            return (new DatabaseException(true, "Account not found"), null);
        else
            return (new DatabaseException(false, "Account found"), accountResult);
    }

    public async Task<IDatabaseException> UpdateAccountAsync(T account)
    {
        Context.Update(account);
        
        var result = await Context.SaveChangesAsync() > 0;
        
        if (result)
            return new DatabaseException(false, "Account updated");
        else
            return new DatabaseException(true, "Failed to update account");
    }

    public async Task<IDatabaseException> EmailExistsAsync(string email)
    {
        var result = await Context.ExistEntityAsync(p => p.Email == email);
        
        if (result)
            return new DatabaseException(true, "E-mail already exists");
        else
            return new DatabaseException(false, "E-mail not found");
    }
    
    public async Task<IDatabaseException> UsernameExistsAsync(string username)
    {
        
        var result = await Context.ExistEntityAsync(p => p.Username == username);
        
        if (result)
            return new DatabaseException(true, "Username already exists");
        else
            return new DatabaseException(false, "Username not found");
    }
}

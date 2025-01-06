using System.Linq.Expressions;
using System.Reflection;
using Core.Database.Interface;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Microsoft.EntityFrameworkCore;

namespace Core.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDbContext
{
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<PlayerModel> Players { get; set; }
    
    // Cache da compiled query
    // Cache de consultas compiladas por tipo de entidade para chamadas assíncronas
    private static readonly Dictionary<Type, Delegate> CompiledQueriesAsync = new();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return Set<TEntity>().AsQueryable();
    }
    
    public async Task<bool> ExistEntityCompiledAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) 
        where TEntity : class
    {
        // Verifica se já existe uma compiled query em cache para o tipo
        if (!CompiledQueriesAsync.TryGetValue(typeof(TEntity), out var compiledQuery))
        {
            // Cria uma nova compiled query assíncrona
            compiledQuery = EF.CompileAsyncQuery((DatabaseContext context, Expression<Func<TEntity, bool>> pred) =>
                context.Set<TEntity>().Any(pred));

            CompiledQueriesAsync[typeof(TEntity)] = compiledQuery;
        }

        // Converte a query para o tipo correto e executa com await
        var query = (Func<DatabaseContext, Expression<Func<TEntity, bool>>, Task<bool>>)compiledQuery;
        return await query(this, predicate);
    }

    public async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = await Set<TEntity>().AddAsync(entity);
        return entry.Entity;
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        Set<TEntity>().Update(entity);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        Set<TEntity>().Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public bool AnyChanges()
    {
        return ChangeTracker.HasChanges();
    }
}
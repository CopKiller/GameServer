using System.Reflection;
using Core.Database.Interfaces;
using Core.Database.Models;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Core.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDbContext
{
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<PlayerModel> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return Set<TEntity>().AsQueryable();
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
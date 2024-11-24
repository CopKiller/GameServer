using Core.Database.Interfaces;
using Core.Database.Models;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Microsoft.EntityFrameworkCore;

namespace Core.Database;

public class DatabaseContext : DbContext, IDbContext
{
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<PlayerModel> Players { get; set; }

    public DatabaseContext() { }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountModel>()
            .HasIndex(a => a.Email)
            .IsUnique();

        modelBuilder.Entity<AccountModel>()
            .HasIndex(a => a.Username)
            .IsUnique();

        modelBuilder.Entity<PlayerModel>()
            .HasIndex(a => a.Name)
            .IsUnique();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatabaseSide2D.db");
        optionsBuilder.UseSqlite($"Filename={databasePath}");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.AddInterceptors(new DetachEntitiesInterceptor());
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

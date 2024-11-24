using System.Linq.Expressions;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Database.Repositorys;

public class Repository<T>(IDbContext context) : IRepository<T>
    where T : class, IEntity
{
    public async Task<T?> GetEntityAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Query<T>();

        // Aplica os includes, se houver
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }
    
    public async Task<IEnumerable<T>> GetEntitiesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Query<T>();

        // Aplica os includes, se houver
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.Where(predicate).ToListAsync();
    }

    public async Task<bool> ExistEntityAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Query<T>().AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        return await context.Query<T>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        return await context.AddAsync(entity);
    }

    public void Update(T entity)
    {
        context.Update(entity);
    }

    public void Delete(T entity)
    {
        context.Delete(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}

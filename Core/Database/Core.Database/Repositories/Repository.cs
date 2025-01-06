using System.Linq.Expressions;
using Core.Database.Interface;
using Core.Database.Interface.Responses;
using Core.Database.Responses;
using Microsoft.EntityFrameworkCore;

namespace Core.Database.Repositories;

public class Repository<T>(IDbContext context) : IRepository<T>
    where T : class, IEntity
{
    
    public async Task<T?> GetEntityAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Query<T>();

        // Aplica os includes, se houver
        foreach (var include in includes) query = query.Include(include);

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<ICollection<T>?> GetEntitiesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Query<T>();

        // Aplica os includes, se houver
        foreach (var include in includes) query = query.Include(include);

        var result = await query.Where(predicate).ToListAsync();
        
        return result.Count != 0 ? result : null;
    }

    public async Task<bool> ExistEntityAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.ExistEntityCompiledAsync(predicate);
    }

    public async Task<IPagedResult<T>> GetPagedAsync(int page = 1, int pageSize = 10)
    {
        var totalCount = await context.Query<T>().CountAsync();
        var results = await context.Query<T>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(x => x.Id)
            .ToListAsync();
        
        return new PagedResult<T>
        {
            Results = results,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
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
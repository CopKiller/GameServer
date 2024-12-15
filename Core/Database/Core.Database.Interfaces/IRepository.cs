using System.Linq.Expressions;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Responses;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Database.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetEntityAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes);

    Task<ICollection<T>?> GetEntitiesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes);

    Task<bool> ExistEntityAsync(Expression<Func<T, bool>> predicate);

    Task<IPagedResult<T>> GetPagedAsync(int page = 1, int pageSize = 10);

    Task<T> AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<int> SaveChangesAsync();
}
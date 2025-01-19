using System.Linq.Expressions;

namespace Core.Database.Interface;

public interface IDbContext
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class;
    void Update<TEntity>(TEntity entity) where TEntity : class;
    void Delete<TEntity>(TEntity entity) where TEntity : class;
    Task<bool> ExistEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    Task<int> SaveChangesAsync();
    bool AnyChanges();
}
namespace Code4Nothing.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAsQueryable();
    IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> condition);
    int Count(Expression<Func<T, bool>> condition);
    Task<int> CountAsync(Expression<Func<T, bool>> condition);
    bool Any(Expression<Func<T, bool>>? condition = null);
    ValueTask<T?> GetAsync(object key);
    Task<IReadOnlyList<T>> GetAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> condition);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
using System.Linq.Expressions;

namespace Code4Nothing.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    private readonly Code4NothingDbContext _dbContext;

    public RepositoryBase(Code4NothingDbContext dbContext) => _dbContext = dbContext;

    public IQueryable<T> GetAsQueryable() => _dbContext.Set<T>();
    public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> condition) => _dbContext.Set<T>().Where(condition);

    public int Count(Expression<Func<T, bool>> condition) => _dbContext.Set<T>().Count(condition);
    public async Task<int> CountAsync(Expression<Func<T, bool>> condition) => await _dbContext.Set<T>().CountAsync(condition);

    public bool Any(Expression<Func<T, bool>>? condition = null) => null != condition ? _dbContext.Set<T>().Any(condition) : _dbContext.Set<T>().Any();

    public async ValueTask<T?> GetAsync(object key)  => await _dbContext.Set<T>().FindAsync(key);
    public async Task<IReadOnlyList<T>> GetAsync() => await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> condition) => await _dbContext.Set<T>().AsNoTracking().Where(condition).ToListAsync();

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);
}
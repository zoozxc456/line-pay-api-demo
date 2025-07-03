using System.Linq.Expressions;
using LinePayDemo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(ShoppingMallContext context) where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    protected virtual IQueryable<T> ApplyQuery(IQueryable<T> query)
    {
        return query;
    }

    protected async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    protected async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    protected async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    protected async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        await context.SaveChangesAsync();
    }

    protected async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }

    protected async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await context.SaveChangesAsync();
    }

    protected Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return ApplyQuery(_dbSet.AsQueryable()).FirstOrDefaultAsync(predicate);
    }

    protected Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        return ApplyQuery(_dbSet.AsQueryable()).Where(predicate).ToListAsync();
    }

    protected Task<List<T>> FindAllAsync()
    {
        return ApplyQuery(_dbSet.AsQueryable()).ToListAsync();
    }
}
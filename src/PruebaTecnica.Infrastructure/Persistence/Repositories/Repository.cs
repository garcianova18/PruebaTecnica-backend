using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Contracts.Repositories;
using System.Linq.Expressions;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories;

public class Repository<T>: IRepository<T> where T : class
{
    private readonly ApplicationDbContext context;
    protected readonly DbSet<T> dbSet;

    public Repository(ApplicationDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        dbSet = this.context.Set<T>();
    }
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.ToListAsync(cancellationToken);
    }
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       return await dbSet.FindAsync([id], cancellationToken);
    }
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(predicate).ToListAsync(cancellationToken);
    }
    public IQueryable<T> Get() => dbSet.AsQueryable();

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var result = await dbSet.AddAsync(entity, cancellationToken);
        return result.Entity;
    }
    public Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        return Task.CompletedTask;
    }
    public Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }
        
}

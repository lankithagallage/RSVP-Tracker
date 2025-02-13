namespace Rsvp.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

using Rsvp.Domain.Contexts;

public class Repository<T>(RsvpContext context) : IRepository<T>
  where T : class, IIdentifiable
{
  private readonly DbSet<T> dbSet = context.Set<T>();

  public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return await this.dbSet.FindAsync([id], cancellationToken);
  }

  public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
  {
    return await this.dbSet.ToListAsync(cancellationToken);
  }

  public async Task AddAsync(T entity, CancellationToken cancellationToken)
  {
    await this.dbSet.AddAsync(entity, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);
  }

  public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
  {
    this.dbSet.Update(entity);
    await context.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    var entity = await this.dbSet.FindAsync([id], cancellationToken);
    if (entity != null)
    {
      this.dbSet.Remove(entity);
      await context.SaveChangesAsync(cancellationToken);
    }
  }

  public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
  {
    return this.dbSet.AnyAsync(e => e.Id == id, cancellationToken);
  }
}

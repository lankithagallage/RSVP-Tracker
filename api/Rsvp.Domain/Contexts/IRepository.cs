namespace Rsvp.Domain.Contexts;

public interface IRepository<T> where T : class
{
  Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
  Task AddAsync(T entity, CancellationToken cancellationToken);
  Task UpdateAsync(T entity, CancellationToken cancellationToken);
  Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}

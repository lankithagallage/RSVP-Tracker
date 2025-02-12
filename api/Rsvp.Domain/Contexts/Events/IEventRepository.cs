namespace Rsvp.Domain.Contexts.Events;

public interface IEventRepository : IRepository<Event>
{
  Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

  Task<IEnumerable<Event>> GetPaginatedEventsAsync(int page, int size,
    CancellationToken cancellationToken);
}

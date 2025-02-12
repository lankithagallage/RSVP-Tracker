namespace Rsvp.Domain.Contexts.Events;

public interface IEventRepository : IRepository<Event>
{
  Task<int> GetTotalCountForPaginationAsync(string? search, CancellationToken cancellationToken);

  Task<IEnumerable<Event>> GetPaginatedEventsAsync(
    int page, int size, string? search, string? sort, string? order, CancellationToken cancellationToken);
}

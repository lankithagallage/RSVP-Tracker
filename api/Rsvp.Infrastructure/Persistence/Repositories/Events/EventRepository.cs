namespace Rsvp.Infrastructure.Persistence.Repositories.Events;

using Microsoft.EntityFrameworkCore;

using Rsvp.Domain.Contexts.Events;

public class EventRepository(RsvpContext context) : Repository<Event>(context), IEventRepository
{
  public async Task<int> GetTotalCountForPaginationAsync(string? search, CancellationToken cancellationToken)
  {
    return await context.Events
      .Where(e => e.Title.ToLower().Contains((search ?? string.Empty).ToLower()))
      .CountAsync(cancellationToken);
  }

  public async Task<IEnumerable<Event>> GetPaginatedEventsAsync(int page, int size, string? search,
    CancellationToken cancellationToken)
  {
    return await context.Events
      .Where(e => e.Title.ToLower().Contains((search ?? string.Empty).ToLower()))
      .OrderBy(e => e.StartTime)
      .Skip((page - 1) * size)
      .Take(size)
      .ToListAsync(cancellationToken);
  }
}

namespace Rsvp.Infrastructure.Persistence.Repositories.Events;

using Microsoft.EntityFrameworkCore;

using Rsvp.Domain.Contexts.Events;

public class EventRepository(RsvpContext context) : Repository<Event>(context), IEventRepository
{
  public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
  {
    return await context.Events.CountAsync(cancellationToken);
  }

  public async Task<IEnumerable<Event>> GetPaginatedEventsAsync(int page, int size,
    CancellationToken cancellationToken)
  {
    return await context.Events
      .OrderBy(e => e.StartTime)
      .Skip((page - 1) * size)
      .Take(size)
      .ToListAsync(cancellationToken);
  }
}

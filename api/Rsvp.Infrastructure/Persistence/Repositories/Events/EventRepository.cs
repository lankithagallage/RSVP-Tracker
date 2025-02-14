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

  public async Task<IEnumerable<Event>> GetPaginatedEventsAsync(
    int page, int size, string? search, string? sort, string? order, CancellationToken cancellationToken)
  {
    var query = context.Events.AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
      query = query.Where(e => EF.Functions.Like(e.Title.ToLower(), $"%{search.ToLower()}%"));
    }

    query = ApplySorting(query, sort, order);

    return await query
      .Skip((page - 1) * size)
      .Take(size).Include(e => e.Organizer)
      .ToListAsync(cancellationToken);
  }

  public Task<Event?> GetByIdWithAttendeesAsync(Guid id, CancellationToken cancellationToken)
  {
    return context.Events
      .Include(e => e.Attendees).ThenInclude(attendee => attendee.User)
      .Include(e => e.Organizer)
      .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
  }

  private static IQueryable<Event> ApplySorting(IQueryable<Event> query, string? sort, string? order)
  {
    return sort?.ToLower() switch
    {
      "title" => order.ToLower() == "desc" ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title),
      "date" => order.ToLower() == "desc" ? query.OrderByDescending(e => e.StartTime) : query.OrderBy(e => e.StartTime),
      _ => query.OrderBy(e => e.StartTime),
    };
  }
}

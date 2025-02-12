namespace Rsvp.Infrastructure.Persistence.Repositories.Events;

using Rsvp.Domain.Contexts.Events;

public class EventRepository(RsvpContext context) : Repository<Event>(context), IEventRepository { }

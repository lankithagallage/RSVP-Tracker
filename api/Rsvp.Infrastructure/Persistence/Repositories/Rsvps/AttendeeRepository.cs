namespace Rsvp.Infrastructure.Persistence.Repositories.Rsvps;

using Rsvp.Domain.Contexts.Rsvps;

public class AttendeeRepository(RsvpContext context) : Repository<Attendee>(context), IAttendeeRepository { }

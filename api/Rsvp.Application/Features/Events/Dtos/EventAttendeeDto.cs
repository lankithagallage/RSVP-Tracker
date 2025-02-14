namespace Rsvp.Application.Features.Events.Dtos;

using Rsvp.Domain.Contexts.Rsvps;

public class EventAttendeeDto
{
  public Guid UserId { get; init; }
  public string AttendeeName { get; init; }
  public RsvpStatus Status { get; init; }
  public DateTimeOffset RsvpDate { get; init; }
}

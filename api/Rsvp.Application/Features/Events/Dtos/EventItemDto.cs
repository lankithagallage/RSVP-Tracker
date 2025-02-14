namespace Rsvp.Application.Features.Events.Dtos;

public class EventItemDto : EventDto
{
  public List<EventAttendeeDto> Attendees { get; init; }
  public OrganizerDto Orgnizer { get; init; }
}

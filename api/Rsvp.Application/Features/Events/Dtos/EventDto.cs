namespace Rsvp.Application.Features.Events.Dtos;

public class EventDto
{
  public Guid Id { get; init; }
  public string Title { get; init; }
  public string Description { get; init; }
  public DateTime StartTime { get; init; }
  public DateTime EndTime { get; init; }
}

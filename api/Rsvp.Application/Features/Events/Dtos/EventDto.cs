namespace Rsvp.Application.Features.Events.Dtos;

public class EventDto
{
  public EventDto(Guid id, string title, string description, string location, DateTime startTime, DateTime endTime)
  {
    this.Id = id;
    this.Title = title;
    this.Description = description;
    this.Location = location;
    this.StartTime = startTime;
    this.EndTime = endTime;
  }

  public Guid Id { get; init; }
  public string Title { get; init; }
  public string Description { get; init; }
  public string Location { get; init; }
  public DateTime StartTime { get; init; }
  public DateTime EndTime { get; init; }

  // TODO: Add IsExpired property
  // TODO: Add OrganizerName property
}

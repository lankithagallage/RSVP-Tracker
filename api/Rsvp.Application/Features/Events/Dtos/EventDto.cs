namespace Rsvp.Application.Features.Events.Dtos;

public class EventDto
{
  public EventDto() { }

  public EventDto(Guid id, string title, string description, string location, DateTime startTime, DateTime endTime,
    bool isExpired, string organizerName)
  {
    this.Id = id;
    this.Title = title;
    this.Description = description;
    this.Location = location;
    this.StartTime = startTime;
    this.EndTime = endTime;
    this.IsExpired = isExpired;
    this.OrganizerName = organizerName;
  }

  public Guid Id { get; init; }
  public string Title { get; init; }
  public string Description { get; init; }
  public string Location { get; init; }
  public DateTime StartTime { get; init; }
  public DateTime EndTime { get; init; }
  public bool IsExpired { get; init; }
  public string OrganizerName { get; init; }
}

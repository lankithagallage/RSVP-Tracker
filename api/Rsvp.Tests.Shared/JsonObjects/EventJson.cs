namespace Rsvp.Tests.Shared.JsonObjects;

public class EventJson
{
  public Guid Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public string Location { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
  public Guid OrganizerId { get; set; }
}

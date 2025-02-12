namespace Rsvp.Infrastructure.Persistence.SeedData.Json;

public class EventJson
{
  public Guid Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
}

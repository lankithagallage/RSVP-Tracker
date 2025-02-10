namespace Rsvp.Infrastructure.Persistence.SeedData.Json;

public class AttendeeJson
{
  public Guid Id { get; set; }
  public Guid EventId { get; set; }
  public Guid UserId { get; set; }
  public string Status { get; set; }
}

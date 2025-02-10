namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using System.Text.Json;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Infrastructure.Persistence.SeedData.Json;

public class EventSeeder(RsvpContext context, ILogger<EventSeeder> logger) : ISeeder
{
  public void Seed()
  {
    if (context.Events.Any())
    {
      logger.LogInformation("Events already exist. Skipping seeding.");
      return;
    }

    var json = JsonFileReader.LoadJsonFile("events.json");
    var events = JsonSerializer.Deserialize<List<EventJson>>(json);

    if (events != null)
    {
      foreach (var newEvent in events.Select(e =>
                 Event.CreateNew(e.Id, e.Title, e.Description, e.StartTime, e.EndTime)))
      {
        context.Events.Add(newEvent);
      }
    }

    context.SaveChanges();
    logger.LogInformation("Seeded events.");
  }
}

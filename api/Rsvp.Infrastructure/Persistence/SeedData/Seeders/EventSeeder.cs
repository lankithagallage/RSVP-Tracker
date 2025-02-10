namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.SeedData.Json;

public class EventSeeder(RsvpContext context, IJsonFileReader jsonReader, ILogger<EventSeeder> logger) : ISeeder
{
  public void Seed()
  {
    if (context.Events.Any())
    {
      logger.LogInformation("Events already exist. Skipping seeding.");
      return;
    }

    var events = jsonReader.LoadData<EventJson>("events.json");

    // @formatter:off
    foreach (var newEvent in events.Select(e =>
               Event.CreateNew(e.Id, e.Title, e.Description, e.StartTime, e.EndTime)))
    {
      context.Events.Add(newEvent);
    }
    // @formatter:on

    context.SaveChanges();
    logger.LogInformation("Seeded events.");
  }
}

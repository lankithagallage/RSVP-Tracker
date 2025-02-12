namespace Rsvp.Tests.Shared.Fixtures.Database.SeedData.Seeders;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence;
using Rsvp.Tests.Shared.JsonObjects;

public class EventSeeder(RsvpContext context, IJsonFileReader jsonReader, ILogger<EventSeeder> logger) : ISeeder
{
  public int Order => 2;

  public void Seed()
  {
    if (context.Events.Any())
    {
      logger.LogInformation("Events already exist. Skipping seeding.");
      return;
    }

    var events = jsonReader.LoadData<EventJson>("test-events.json");

    foreach (var e in events)
    {
      var organizer = context.Users.FirstOrDefault(u => u.Id == e.OrganizerId && u.Role == UserRole.Organizer);
      if (organizer == null)
      {
        continue;
      }

      var @event = Event.CreateNew(e.Id, e.Title, e.Description, e.Location, e.StartTime, e.EndTime, organizer);
      context.Events.Add(@event);
    }

    context.SaveChanges();
    logger.LogInformation("Seeded events.");
  }
}

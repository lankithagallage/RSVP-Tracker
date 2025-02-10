namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using System.Text.Json;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Infrastructure.Persistence.SeedData.Json;

public class AttendeeSeeder(RsvpContext context, ILogger<AttendeeSeeder> logger) : ISeeder
{
  public void Seed()
  {
    if (context.Attendees.Any())
    {
      logger.LogInformation("Attendees already exist. Skipping seeding.");
      return;
    }

    var json = JsonFileReader.LoadJsonFile("attendees.json");
    var attendees = JsonSerializer.Deserialize<List<AttendeeJson>>(json);

    if (attendees != null)
    {
      foreach (var a in attendees)
      {
        var @event = context.Events.FirstOrDefault(e => e.Id == a.EventId);
        var user = context.Users.FirstOrDefault(u => u.Id == a.UserId);

        if (@event == null || user == null)
        {
          continue;
        }

        var newAttendee = Attendee.CreateNew(a.Id, @event, user, a.CreatedAt, a.ModifiedAt);
        if (a.Status == "Confirmed")
        {
          newAttendee.Confirm();
        }

        context.Attendees.Add(newAttendee);
      }
    }

    context.SaveChanges();
    logger.LogInformation("Seeded attendees.");
  }
}

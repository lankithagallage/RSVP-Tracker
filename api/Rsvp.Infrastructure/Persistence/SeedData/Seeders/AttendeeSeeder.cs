namespace Rsvp.Infrastructure.Persistence.SeedData.Seeders;

using Microsoft.Extensions.Logging;

using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.SeedData.Json;

public class AttendeeSeeder(RsvpContext context, IJsonFileReader jsonReader, ILogger<AttendeeSeeder> logger) : ISeeder
{
  public int Order => 3;

  public void Seed()
  {
    if (context.Attendees.Any())
    {
      logger.LogInformation("Attendees already exist. Skipping seeding.");
      return;
    }

    var attendees = jsonReader.LoadData<AttendeeJson>("attendees.json");

    foreach (var a in attendees)
    {
      var @event = context.Events.FirstOrDefault(e => e.Id == a.EventId);
      var user = context.Users.FirstOrDefault(u => u.Id == a.UserId);

      if (@event == null || user == null)
      {
        continue;
      }

      var newAttendee = Attendee.CreateNew(a.Id, @event, user);
      if (a.Status == "Confirmed")
      {
        newAttendee.Confirm();
      }

      context.Attendees.Add(newAttendee);
    }

    context.SaveChanges();
    logger.LogInformation("Seeded attendees.");
  }
}

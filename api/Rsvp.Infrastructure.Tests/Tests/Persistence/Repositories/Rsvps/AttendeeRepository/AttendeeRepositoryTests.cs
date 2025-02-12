namespace Rsvp.Infrastructure.Tests.Tests.Persistence.Repositories.Rsvps.AttendeeRepository;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.Repositories.Rsvps;
using Rsvp.Infrastructure.Tests.Fixtures;
using Rsvp.Tests.Shared.JsonObjects;

[Collection("Database collection")]
public class AttendeeRepositoryTests : IClassFixture<DatabaseFixture>
{
  private static readonly IJsonFileReader JsonFileReader = new InfrastructureJsonFileReader(Path.Combine("Tests",
    "Persistence", "Repositories", "Rsvps", "AttendeeRepository"));

  private readonly AttendeeRepository attendeeRepository;

  public AttendeeRepositoryTests(DatabaseFixture fixture)
  {
    var context = fixture.Context;
    this.attendeeRepository = new AttendeeRepository(context);
  }

  public static IEnumerable<object[]> GetAttendeeTestData()
  {
    var attendees = JsonFileReader.LoadData<AttendeeJson>("valid_attendees.json");
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var users = JsonFileReader.LoadData<UserJson>("valid_users.json");
    return attendees.Select(a => new object[]
    {
      events.FirstOrDefault(e => e.Id == a.EventId),
      users.FirstOrDefault(u => u.Id == a.UserId),
    });
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public async Task AttendeeRepository_CanAddAttendee(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    await this.attendeeRepository.AddAsync(newAttendee, CancellationToken.None);
    var fetchedAttendee = await this.attendeeRepository.GetByIdAsync(newAttendee.Id, CancellationToken.None);

    Assert.NotNull(fetchedAttendee);
    Assert.Equal(newAttendee.User.Email, fetchedAttendee.User.Email);
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public async Task AttendeeRepository_CanUpdateAttendee(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    await this.attendeeRepository.AddAsync(newAttendee, CancellationToken.None);

    newAttendee.Confirm();
    await this.attendeeRepository.UpdateAsync(newAttendee, CancellationToken.None);

    var updatedAttendee = await this.attendeeRepository.GetByIdAsync(newAttendee.Id, CancellationToken.None);
    Assert.NotNull(updatedAttendee);
    Assert.Equal(RsvpStatus.Confirmed, updatedAttendee.Status);
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public async Task AttendeeRepository_CanDeleteAttendee(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    await this.attendeeRepository.AddAsync(newAttendee, CancellationToken.None);
    await this.attendeeRepository.DeleteAsync(newAttendee.Id, CancellationToken.None);

    var deletedAttendee = await this.attendeeRepository.GetByIdAsync(newAttendee.Id, CancellationToken.None);
    Assert.Null(deletedAttendee);
  }
}

namespace Rsvp.Domain.Tests.Contexts.Rsvps.Attendee;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Tests.Shared.JsonObjects;

public class AttendeeTests
{
  private static readonly IJsonFileReader JsonFileReader =
    new DomainJsonFileReader(Path.Combine("Contexts", "Rsvps", "Attendee"));

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
  public void Attendee_CreateNew_CanCreateNewAttendee(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    Assert.NotNull(newAttendee);
    Assert.Equal(newEvent.Id, newAttendee.Event.Id);
    Assert.Equal(newUser.Id, newAttendee.User.Id);
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_CreateNew_ShouldThrowIfEventIsNull(EventJson eventJson, UserJson userJson)
  {
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    Assert.Throws<ArgumentNullException>(() => Attendee.CreateNew(null, newUser));
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_CreateNew_ShouldThrowIfUserIsNull(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    Assert.Throws<ArgumentNullException>(() => Attendee.CreateNew(newEvent, null));
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_Confirm_ShouldSetStatusToConfirmed(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    newAttendee.Confirm();
    Assert.Equal(RsvpStatus.Confirmed, newAttendee.Status);
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_Cancel_ShouldSetStatusToCancelled(EventJson eventJson, UserJson userJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    newAttendee.Cancel();
    Assert.Equal(RsvpStatus.Cancelled, newAttendee.Status);
  }
}

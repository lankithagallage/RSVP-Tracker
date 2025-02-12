namespace Rsvp.Domain.Tests.Tests.Contexts.Rsvps.Attendee;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Tests.Shared.JsonObjects;

public class AttendeeTests
{
  private static readonly IJsonFileReader JsonFileReader =
    new DomainJsonFileReader(Path.Combine("Tests", "Contexts", "Rsvps", "Attendee"));

  public static IEnumerable<object[]> GetAttendeeTestData()
  {
    var attendees = JsonFileReader.LoadData<AttendeeJson>("valid_attendees.json");
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var organizers = JsonFileReader.LoadData<UserJson>("valid_organizers.json");
    var users = JsonFileReader.LoadData<UserJson>("valid_users.json");

    return attendees.Select(a =>
    {
      var eventItem = events.FirstOrDefault(e => e.Id == a.EventId);
      var organizer = organizers.FirstOrDefault(o => o.Id == eventItem?.OrganizerId);
      var user = users.FirstOrDefault(u => u.Id == a.UserId);

      return new object[]
      {
        eventItem,
        organizer,
        user,
      };
    });
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_CreateNew_CanCreateNewAttendee(EventJson eventJson, UserJson organizer, UserJson userJson)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    Assert.NotNull(newAttendee);
    Assert.Equal(newEvent.Id, newAttendee.Event.Id);
    Assert.Equal(newUser.Id, newAttendee.User.Id);
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_CreateNew_ShouldThrowIfEventIsNull(EventJson eventJson, UserJson organizer, UserJson userJson)
  {
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    Assert.Throws<ArgumentNullException>(() => Attendee.CreateNew(null, newUser));
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_CreateNew_ShouldThrowIfUserIsNull(EventJson eventJson, UserJson organizer, UserJson userJson)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    Assert.Throws<ArgumentNullException>(() => Attendee.CreateNew(newEvent, null));
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_Confirm_ShouldSetStatusToConfirmed(EventJson eventJson, UserJson organizer, UserJson userJson)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    newAttendee.Confirm();
    Assert.Equal(RsvpStatus.Confirmed, newAttendee.Status);
  }

  [Theory]
  [MemberData(nameof(GetAttendeeTestData))]
  public void Attendee_Cancel_ShouldSetStatusToCancelled(EventJson eventJson, UserJson organizer, UserJson userJson)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    var newUser = User.CreateNew(userJson.FirstName, userJson.LastName, userJson.Email,
      Enum.Parse<UserRole>(userJson.Role));
    var newAttendee = Attendee.CreateNew(newEvent, newUser);

    newAttendee.Cancel();
    Assert.Equal(RsvpStatus.Cancelled, newAttendee.Status);
  }
}

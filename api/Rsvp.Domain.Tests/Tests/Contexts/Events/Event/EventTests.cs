namespace Rsvp.Domain.Tests.Tests.Contexts.Events.Event;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Tests.Shared.JsonObjects;

public class EventTests
{
  private static readonly IJsonFileReader JsonFileReader =
    new DomainJsonFileReader(Path.Combine("Tests", "Contexts", "Events", "Event"));

  public static IEnumerable<object[]> GetEventTestData()
  {
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var organizers = JsonFileReader.LoadData<UserJson>("valid_organizers.json");
    return events.SelectMany(e =>
      organizers.Select(u => new object[] { e, organizers.FirstOrDefault(o => o.Id == e.OrganizerId) }));
  }

  public static IEnumerable<object[]> GetEventAndUserTestData()
  {
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var organizers = JsonFileReader.LoadData<UserJson>("valid_organizers.json");
    var users = JsonFileReader.LoadData<UserJson>("valid_users.json");

    return events.SelectMany(e => users.Select(u => new object[]
    {
      e,
      organizers.FirstOrDefault(o => o.Id == e.OrganizerId),
      u,
    }));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ValidatesStartAndEndTime(EventJson @event, UserJson organizer)
  {
    Assert.True(@event.StartTime < @event.EndTime, "Start time should be before end time");
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_CanCreateNewEvent(EventJson @event, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(@event.Title, @event.Description, @event.Location, @event.StartTime, @event.EndTime,
      newOrganizer);
    Assert.NotNull(newEvent);
    Assert.Equal(newEvent.Title, @event.Title);
    Assert.Equal(newEvent.Description, @event.Description);
    Assert.Equal(newEvent.StartTime, @event.StartTime);
    Assert.Equal(newEvent.EndTime, @event.EndTime);
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfTitleIsEmpty(EventJson @event, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    Assert.Throws<ArgumentException>(() =>
      Event.CreateNew("", @event.Description, @event.Location, @event.StartTime, @event.EndTime, newOrganizer));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfDescriptionIsEmpty(EventJson @event, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    Assert.Throws<ArgumentException>(() =>
      Event.CreateNew(@event.Title, "", @event.Location, @event.StartTime, @event.EndTime, newOrganizer));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfLocationIsEmpty(EventJson @event, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    Assert.Throws<ArgumentException>(() =>
      Event.CreateNew(@event.Title, @event.Description, "", @event.StartTime, @event.EndTime, newOrganizer));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfStartTimeIsPast(EventJson @event, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var pastStartTime = DateTime.UtcNow.AddDays(-1);
    Assert.Throws<ArgumentException>(() =>
      Event.CreateNew(@event.Title, @event.Description, @event.Location, pastStartTime, @event.EndTime, newOrganizer));
  }

  [Theory]
  [MemberData(nameof(GetEventAndUserTestData))]
  public void Event_AddAttendee_CanAddNewAttendee(EventJson @event, UserJson organizer, UserJson user)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newUser = User.CreateNew(user.FirstName, user.LastName, user.Email, Enum.Parse<UserRole>(user.Role));
    var newEvent = Event.CreateNew(@event.Title, @event.Description, @event.Location, @event.StartTime, @event.EndTime,
      newOrganizer);
    var attendee = Attendee.CreateNew(newEvent, newUser);
    newEvent.AddAttendee(attendee);

    Assert.Contains(attendee, newEvent.Attendees);
  }

  [Theory]
  [MemberData(nameof(GetEventAndUserTestData))]
  public void Event_AddAttendee_DoesNotAllowDuplicateEmails(EventJson @event, UserJson organizer, UserJson user)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(@event.Title, @event.Description, @event.Location, @event.StartTime, @event.EndTime,
      newOrganizer);
    var newUser = User.CreateNew(user.FirstName, user.LastName, user.Email, Enum.Parse<UserRole>(user.Role));
    var attendee1 = Attendee.CreateNew(newEvent, newUser);
    newEvent.AddAttendee(attendee1);

    var attendee2 = Attendee.CreateNew(newEvent, newUser);

    Assert.Throws<InvalidOperationException>(() => newEvent.AddAttendee(attendee2));
  }

  [Fact]
  public void Event_AddAttendee_ShouldThrowIfAttendeeIsNull()
  {
    var newOrganizer = User.CreateNew("Organizer Fist Name", "Organizer Last Name", "organizer@rsvp.com",
      UserRole.Organizer);

    var newEvent = Event.CreateNew("Test Event", "Test Description", "Test Location", DateTime.UtcNow.AddHours(1),
      DateTime.UtcNow.AddHours(2), newOrganizer);
    Assert.Throws<ArgumentNullException>(() => newEvent.AddAttendee(null));
  }

  [Fact]
  public void Event_AddAttendee_ShouldThrowIfOrganizerIsNull()
  {
    Assert.Throws<ArgumentNullException>(() => Event.CreateNew("Test Event", "Test Description", "Test Location",
      DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2), null));
  }
}

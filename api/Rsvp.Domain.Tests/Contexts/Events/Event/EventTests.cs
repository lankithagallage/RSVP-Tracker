namespace Rsvp.Domain.Tests.Contexts.Events.Event;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Tests.Shared.JsonObjects;

public class EventTests
{
  private static readonly IJsonFileReader JsonFileReader =
    new DomainJsonFileReader(Path.Combine("Contexts", "Events", "Event"));

  public static IEnumerable<object[]> GetEventTestData()
  {
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    return events.Select(e => new object[] { e });
  }

  public static IEnumerable<object[]> GetEventAndUserTestData()
  {
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var users = JsonFileReader.LoadData<UserJson>("valid_users.json");
    return events.SelectMany(e => users.Select(u => new object[] { e, u }));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ValidatesStartAndEndTime(EventJson @event)
  {
    Assert.True(@event.StartTime < @event.EndTime, "Start time should be before end time");
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_CanCreateNewEvent(EventJson @event)
  {
    var newEvent = Event.CreateNew(@event.Title, @event.Description, @event.StartTime, @event.EndTime);
    Assert.NotNull(newEvent);
    Assert.Equal(newEvent.Title, @event.Title);
    Assert.Equal(newEvent.Description, @event.Description);
    Assert.Equal(newEvent.StartTime, @event.StartTime);
    Assert.Equal(newEvent.EndTime, @event.EndTime);
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfTitleIsEmpty(EventJson @event)
  {
    Assert.Throws<ArgumentException>(() => Event.CreateNew("", @event.Description, @event.StartTime, @event.EndTime));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfDescriptionIsEmpty(EventJson @event)
  {
    Assert.Throws<ArgumentException>(() => Event.CreateNew(@event.Title, "", @event.StartTime, @event.EndTime));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public void Event_CreateNew_ShouldThrowIfStartTimeIsPast(EventJson @event)
  {
    var pastStartTime = DateTime.UtcNow.AddDays(-1);
    Assert.Throws<ArgumentException>(() => Event.CreateNew(@event.Title, @event.Description, pastStartTime, @event.EndTime));
  }

  [Theory]
  [MemberData(nameof(GetEventAndUserTestData))]
  public void Event_AddAttendee_CanAddNewAttendee(EventJson @event, UserJson user)
  {
    var newEvent = Event.CreateNew(@event.Title, @event.Description, @event.StartTime, @event.EndTime);
    var newUser = User.CreateNew(user.FirstName, user.LastName, user.Email, UserRole.Attendee);
    var attendee = Attendee.CreateNew(newEvent, newUser);
    newEvent.AddAttendee(attendee);

    Assert.Contains(attendee, newEvent.Attendees);
  }

  [Theory]
  [MemberData(nameof(GetEventAndUserTestData))]
  public void Event_AddAttendee_DoesNotAllowDuplicateEmails(EventJson @event, UserJson user)
  {
    var newEvent = Event.CreateNew(@event.Title, @event.Description, @event.StartTime, @event.EndTime);
    var newUser = User.CreateNew(user.FirstName, user.LastName, user.Email, UserRole.Attendee);
    var attendee1 = Attendee.CreateNew(newEvent, newUser);
    newEvent.AddAttendee(attendee1);

    var attendee2 = Attendee.CreateNew(newEvent, newUser);

    Assert.Throws<InvalidOperationException>(() => newEvent.AddAttendee(attendee2));
  }

  [Fact]
  public void Event_AddAttendee_ShouldThrowIfAttendeeIsNull()
  {
    var newEvent = Event.CreateNew("Test Event", "Test Description", DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2));
    Assert.Throws<ArgumentNullException>(() => newEvent.AddAttendee(null));
  }
}

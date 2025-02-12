namespace Rsvp.Infrastructure.Tests.Tests.Persistence.Repositories.Events.EventRepository;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.Repositories.Events;
using Rsvp.Infrastructure.Tests.Fixtures;
using Rsvp.Tests.Shared.JsonObjects;

[Collection("Database collection")]
public class EventRepositoryTests : IClassFixture<DatabaseFixture>
{
  private static readonly IJsonFileReader JsonFileReader = new InfrastructureJsonFileReader(Path.Combine("Tests",
    "Persistence", "Repositories", "Events", "EventRepository"));

  private readonly EventRepository eventRepository;

  public EventRepositoryTests(DatabaseFixture fixture)
  {
    var context = fixture.Context;
    this.eventRepository = new EventRepository(context);
  }

  public static IEnumerable<object[]> GetEventTestData()
  {
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var organizers = JsonFileReader.LoadData<UserJson>("valid_organizers.json");
    return events.SelectMany(e => organizers.Select(u => new object[] { e, u }));
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public async Task EventRepository_CanAddEvent(EventJson eventJson, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    await this.eventRepository.AddAsync(newEvent, CancellationToken.None);

    var fetchedEvent = await this.eventRepository.GetByIdAsync(newEvent.Id, CancellationToken.None);
    Assert.NotNull(fetchedEvent);
    Assert.Equal(newEvent.Title, fetchedEvent.Title);
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public async Task EventRepository_CanUpdateTitleEvent(EventJson eventJson, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    await this.eventRepository.AddAsync(newEvent, CancellationToken.None);

    newEvent.UpdateTitle("Updated Title");
    await this.eventRepository.UpdateAsync(newEvent, CancellationToken.None);

    var updatedEvent = await this.eventRepository.GetByIdAsync(newEvent.Id, CancellationToken.None);
    Assert.NotNull(updatedEvent);
    Assert.Equal("Updated Title", updatedEvent.Title);
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public async Task EventRepository_CanDeleteEvent(EventJson eventJson, UserJson organizer)
  {
    var newOrganizer = User.CreateNew(organizer.FirstName, organizer.LastName, organizer.Email,
      Enum.Parse<UserRole>(organizer.Role));
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.Location, eventJson.StartTime,
      eventJson.EndTime, newOrganizer);
    await this.eventRepository.AddAsync(newEvent, CancellationToken.None);

    await this.eventRepository.DeleteAsync(newEvent.Id, CancellationToken.None);

    var deletedEvent = await this.eventRepository.GetByIdAsync(newEvent.Id, CancellationToken.None);
    Assert.Null(deletedEvent);
  }
}

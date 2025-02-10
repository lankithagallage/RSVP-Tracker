namespace Rsvp.Infrastructure.Tests.Tests.Persistence.Repositories.Events.EventRepository;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Interfaces;
using Rsvp.Infrastructure.Persistence.Repositories.Events;
using Rsvp.Infrastructure.Persistence.SeedData.Json;
using Rsvp.Infrastructure.Tests.Fixtures;

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
    return events.Select(e => new object[] { e });
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public async Task EventRepository_CanAddEvent(EventJson eventJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    await this.eventRepository.AddAsync(newEvent, CancellationToken.None);

    var fetchedEvent = await this.eventRepository.GetByIdAsync(newEvent.Id, CancellationToken.None);
    Assert.NotNull(fetchedEvent);
    Assert.Equal(newEvent.Title, fetchedEvent.Title);
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public async Task EventRepository_CanUpdateTitleEvent(EventJson eventJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    await this.eventRepository.AddAsync(newEvent, CancellationToken.None);

    newEvent.UpdateTitle("Updated Title");
    await this.eventRepository.UpdateAsync(newEvent, CancellationToken.None);

    var updatedEvent = await this.eventRepository.GetByIdAsync(newEvent.Id, CancellationToken.None);
    Assert.NotNull(updatedEvent);
    Assert.Equal("Updated Title", updatedEvent.Title);
  }

  [Theory]
  [MemberData(nameof(GetEventTestData))]
  public async Task EventRepository_CanDeleteEvent(EventJson eventJson)
  {
    var newEvent = Event.CreateNew(eventJson.Title, eventJson.Description, eventJson.StartTime, eventJson.EndTime);
    await this.eventRepository.AddAsync(newEvent, CancellationToken.None);

    await this.eventRepository.DeleteAsync(newEvent.Id, CancellationToken.None);

    var deletedEvent = await this.eventRepository.GetByIdAsync(newEvent.Id, CancellationToken.None);
    Assert.Null(deletedEvent);
  }
}

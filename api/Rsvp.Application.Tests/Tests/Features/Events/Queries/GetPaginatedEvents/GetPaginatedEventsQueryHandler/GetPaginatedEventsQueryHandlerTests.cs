namespace Rsvp.Application.Tests.Tests.Features.Events.Queries.GetPaginatedEvents.GetPaginatedEventsQueryHandler;

using AutoMapper;

using Moq;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;
using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Users;
using Rsvp.Domain.Interfaces;
using Rsvp.Tests.Shared.JsonObjects;

public class GetPaginatedEventsQueryHandlerTests
{
  private static readonly IJsonFileReader JsonFileReader = new ApplicationJsonFileReader(
    Path.Combine("Tests", "Features", "Events", "Queries", "GetPaginatedEvents", "GetPaginatedEventsQueryHandler"));

  private readonly GetPaginatedEventsQueryHandler handler;
  private readonly Mock<IEventRepository> mockEventRepository;
  private readonly Mock<IMapper> mockMapper;

  public GetPaginatedEventsQueryHandlerTests()
  {
    this.mockEventRepository = new Mock<IEventRepository>();
    this.mockMapper = new Mock<IMapper>();
    this.handler = new GetPaginatedEventsQueryHandler(this.mockEventRepository.Object, this.mockMapper.Object);
  }

  public static IEnumerable<object[]> GetEventDtoQueryTestData()
  {
    var events = JsonFileReader.LoadData<EventJson>("valid_events.json");
    var eventDtos = JsonFileReader.LoadData<EventDto>("valid_events_dto.json");
    var organizers = JsonFileReader.LoadData<UserJson>("valid_organizers.json");
    var userLookup = organizers.ToDictionary(json => json.Id, json =>
      User.CreateNew(json.Id, json.FirstName, json.LastName, json.Email, Enum.Parse<UserRole>(json.Role)));
    var eventList = events.Select(json => Event.CreateNew(json.Id, json.Title, json.Description, json.Location,
      json.StartTime, json.EndTime, userLookup[json.OrganizerId])).ToList();

    return new List<object[]> { new object[] { eventList, eventDtos } };
  }

  [Theory]
  [MemberData(nameof(GetEventDtoQueryTestData))]
  public async Task Handle_ReturnsPaginatedResult(List<Event> events, List<EventDto> eventDtos)
  {
    var request = new GetPaginatedEventsQuery(1, 10);
    var pagedEvents = events.Take(10).ToList();
    var pagedEventsDtos = eventDtos.Take(10).ToList();

    this.mockEventRepository.Setup(repo => repo.GetTotalCountForPaginationAsync(null, It.IsAny<CancellationToken>()))
      .ReturnsAsync(eventDtos.Count);
    this.mockEventRepository.Setup(repo =>
        repo.GetPaginatedEventsAsync(1, 10, null, "date", "asc", It.IsAny<CancellationToken>()))
      .ReturnsAsync(events.Take(10));
    this.mockMapper.Setup(mapper => mapper.Map<List<EventDto>>(pagedEvents)).Returns(pagedEventsDtos);

    var result = await this.handler.Handle(request, CancellationToken.None);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.Equal(10, result.Value.Value.Count);
  }

  [Theory]
  [MemberData(nameof(GetEventDtoQueryTestData))]
  public async Task Handle_ReturnsInvalidResult_WhenPageNumberIsTooHigh(List<Event> events, List<EventDto> eventDtos)
  {
    var request = new GetPaginatedEventsQuery(100, 10);

    this.mockEventRepository.Setup(repo => repo.GetTotalCountForPaginationAsync(null, It.IsAny<CancellationToken>()))
      .ReturnsAsync(eventDtos.Count);

    var result = await this.handler.Handle(request, CancellationToken.None);

    Assert.NotNull(result);
    Assert.False(result.IsSuccess);
    Assert.Contains("Page number is too high.", result.ValidationErrors.First().ErrorMessage);
  }

  [Theory]
  [InlineData("Test Event 1")]
  [InlineData("Test Event 5")]
  [InlineData("NonExistingEvent")]
  public async Task Handle_FiltersBySearchTerm(string searchTerm)
  {
    var items = GetEventDtoQueryTestData();
    var events = items.First()[0] as List<Event>;
    var eventDtos = items.First()[1] as List<EventDto>;

    var request = new GetPaginatedEventsQuery(1, 10, searchTerm, "title");
    var filteredEvents =
      events.Where(e => e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    var filteredEventDtos =
      eventDtos.Where(e => e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

    this.mockEventRepository
      .Setup(repo => repo.GetTotalCountForPaginationAsync(searchTerm, It.IsAny<CancellationToken>()))
      .ReturnsAsync(filteredEvents.Count);
    this.mockEventRepository.Setup(repo =>
        repo.GetPaginatedEventsAsync(1, 10, searchTerm, "title", "asc", It.IsAny<CancellationToken>()))
      .ReturnsAsync(filteredEvents);
    this.mockMapper.Setup(mapper => mapper.Map<List<EventDto>>(filteredEvents)).Returns(filteredEventDtos);

    var result = await this.handler.Handle(request, CancellationToken.None);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    if (filteredEvents.Count > 0)
    {
      Assert.All(result.Value.Value, e => Assert.Contains(searchTerm, e.Title, StringComparison.OrdinalIgnoreCase));
    }
    else
    {
      Assert.Empty(result.Value.Value);
    }
  }
}

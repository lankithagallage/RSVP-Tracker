namespace Rsvp.Api.Tests.Tests.Controllers.v1.EventsController.SearchEvents;

using System.Globalization;

using Ardalis.Result;

using Moq;

using Rsvp.Api.Controllers.v1;
using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;
using Rsvp.Application.Services;
using Rsvp.Domain.Interfaces;

public class SearchEventsTests
{
  private static readonly IJsonFileReader JsonFileReader =
    new ApiJsonFileReader(Path.Combine("Tests", "Controllers", "v1", "EventsController", "SearchEvents"));

  private readonly EventsController controller;
  private readonly Mock<IEventsControllerService> mockService;

  public SearchEventsTests()
  {
    this.mockService = new Mock<IEventsControllerService>();
    this.controller = new EventsController(this.mockService.Object);
  }

  public static IEnumerable<object[]> GetEventDtoTestData()
  {
    var events = JsonFileReader.LoadData<EventDto>("valid_events_dto.json");
    return new List<object[]> { new object[] { events } };
  }

  [Theory]
  [MemberData(nameof(GetEventDtoTestData))]
  public async Task SearchEvents_ReturnsOkWithData(List<EventDto> eventDtos)
  {
    var pagedInfo = new PagedInfo(1, eventDtos.Count, 1, eventDtos.Count);

    var pagedResult = new PagedResult<List<EventDto>>(pagedInfo, eventDtos);
    var expectedResult = Result.Success(pagedResult);

    this.mockService.Setup(s =>
        s.GetPaginatedEventsQueryAsync(It.IsAny<GetPaginatedEventsQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(expectedResult);

    var result = await this.controller.SearchEvents(1, 10, null, "date", "asc", CancellationToken.None);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.Equal(eventDtos.Count, result.Value.Value.Count);
  }

  [Theory]
  [InlineData("title", "asc")]
  [InlineData("title", "desc")]
  [InlineData("date", "asc")]
  [InlineData("date", "desc")]
  public async Task SearchEvents_SortsCorrectly(string sort, string order)
  {
    var eventDtos = JsonFileReader.LoadData<EventDto>("valid_events_large_dto.json");
    var pagedInfo = new PagedInfo(1, eventDtos.Count, 1, eventDtos.Count);
    var sortedEvents = order == "asc"
      ? eventDtos.OrderBy(e => sort == "title" ? e.Title : e.StartTime.ToString(CultureInfo.InvariantCulture)).ToList()
      : eventDtos.OrderByDescending(e => sort == "title" ? e.Title : e.StartTime.ToString(CultureInfo.InvariantCulture))
        .ToList();

    var pagedResult = new PagedResult<List<EventDto>>(pagedInfo, sortedEvents);
    var expectedResult = Result.Success(pagedResult);

    this.mockService.Setup(s =>
        s.GetPaginatedEventsQueryAsync(It.IsAny<GetPaginatedEventsQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(expectedResult);

    var result = await this.controller.SearchEvents(1, 10, null, sort, order, CancellationToken.None);

    Assert.NotNull(result);
    Assert.True(result.IsSuccess);
    Assert.Equal(sortedEvents.First().Title, result.Value.Value.First().Title);
  }

  [Theory]
  [InlineData("Test Event 1")]
  [InlineData("Test Event 5")]
  [InlineData("NonExistingEvent")]
  public async Task SearchEvents_FiltersBySearchTerm(string searchTerm)
  {
    var eventDtos = JsonFileReader.LoadData<EventDto>("valid_events_large_dto.json");
    var filteredEvents =
      eventDtos.Where(e => e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

    var pagedInfo = new PagedInfo(1, filteredEvents.Count, 1, filteredEvents.Count);
    var pagedResult = new PagedResult<List<EventDto>>(pagedInfo, filteredEvents);
    var expectedResult = Result.Success(pagedResult);

    this.mockService.Setup(s =>
        s.GetPaginatedEventsQueryAsync(It.IsAny<GetPaginatedEventsQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(expectedResult);

    var result = await this.controller.SearchEvents(1, 10, searchTerm, "title", "asc", CancellationToken.None);

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

  [Fact]
  public async Task SearchEvents_ReturnsBadRequestOnFailure()
  {
    var expectedResult = Result.Invalid(new ValidationError("Error fetching events"));

    this.mockService.Setup(s =>
        s.GetPaginatedEventsQueryAsync(It.IsAny<GetPaginatedEventsQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(expectedResult);

    var result = await this.controller.SearchEvents(1, 10, null, "date", "asc", CancellationToken.None);

    Assert.NotNull(result);
    Assert.False(result.IsSuccess);
    Assert.Equal("Error fetching events", result.ValidationErrors.First().ErrorMessage);
  }
}

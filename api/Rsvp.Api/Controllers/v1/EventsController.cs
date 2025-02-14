namespace Rsvp.Api.Controllers.v1;

using System.Net.Mime;

using Ardalis.Result;
using Ardalis.Result.AspNetCore;

using Asp.Versioning;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Application.Features.Events.Queries.GetEventById;
using Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;
using Rsvp.Application.Services;

/// <summary>
/// Manages event-related operations, including searching and retrieving events.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/events")]
[TranslateResultToActionResult]
public class EventsController : ControllerBase
{
  private readonly IEventsControllerService controllerService;

  /// <summary>
  /// Initializes a new instance of the <see cref="EventsController" /> class.
  /// </summary>
  /// <param name="controllerService">The service handling event-related business logic.</param>
  public EventsController(IEventsControllerService controllerService)
  {
    this.controllerService = controllerService;
  }

  /// <summary>
  /// Retrieves the details of an event by its unique identifier.
  /// </summary>
  /// <param name="eventId">The unique identifier of the event.</param>
  /// <param name="cancellationToken">A token to cancel the request if needed.</param>
  /// <returns>The event details including attendees and organizer information.</returns>
  /// <response code="200">Returns the event details.</response>
  /// <response code="404">If no event is found with the provided identifier.</response>
  [HttpGet("{eventId:guid}")]
  [ProducesResponseType(typeof(EventItemDto), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(NotFound), StatusCodes.Status404NotFound)]
  [Produces(MediaTypeNames.Application.Json)]
  public async Task<Result<EventItemDto>> GetEventById(Guid eventId, CancellationToken cancellationToken)
  {
    var query = new GetEventByIdQuery(eventId);
    return await this.controllerService.GetEventByIdAsync(query, cancellationToken);
  }

  /// <summary>
  /// Searches for events and retrieves a paginated list based on the provided filters.
  /// </summary>
  /// <param name="page">The page number to retrieve (1-based index).</param>
  /// <param name="size">The number of events to retrieve per page.</param>
  /// <param name="search">Optional search term to filter events by title or description.</param>
  /// <param name="sort">Field to sort the events by (e.g., "title" or "date"). Default is "date".</param>
  /// <param name="order">Sorting order, either "asc" (ascending) or "desc" (descending). Default is "asc".</param>
  /// <param name="cancellationToken">A token to cancel the request if needed.</param>
  /// <returns>A paginated list of events matching the search criteria.</returns>
  /// <response code="200">Returns the paginated list of events.</response>
  /// <response code="400">If any of the query parameters are invalid.</response>
  [HttpGet("search")]
  [ProducesResponseType(typeof(PagedResult<List<EventDto>>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
  [Produces(MediaTypeNames.Application.Json)]
  public async Task<Result<PagedResult<List<EventDto>>>> SearchEvents(
    [FromQuery] int page,
    [FromQuery] int size,
    [FromQuery] string? search = null,
    [FromQuery] string? sort = "date",
    [FromQuery] string? order = "asc",
    CancellationToken cancellationToken = default)
  {
    var query = new GetPaginatedEventsQuery(page, size, search, sort, order);
    return await this.controllerService.GetPaginatedEventsQueryAsync(query, cancellationToken);
  }
}

namespace Rsvp.Api.Controllers.v1;

using System.Net.Mime;

using Ardalis.Result;
using Ardalis.Result.AspNetCore;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;
using Rsvp.Application.Services;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/events")]
[TranslateResultToActionResult]
public class EventsController : ControllerBase
{
  private readonly IEventsControllerService controllerService;

  public EventsController(IEventsControllerService controllerService)
  {
    this.controllerService = controllerService;
  }

  /// <summary>
  /// Search and retrieve paginated events.
  /// </summary>
  /// <param name="page">Page number to retrieve.</param>
  /// <param name="size">Number of events per page.</param>
  /// <param name="search">Search term for filtering event titles/descriptions.</param>
  /// <param name="sort">Sorting field ("title" or "date").</param>
  /// <param name="order">Sorting order ("asc" or "desc").</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Paginated result of events.</returns>
  [HttpGet("search")]
  [ProducesResponseType(typeof(PagedResult<List<EventDto>>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Program), StatusCodes.Status400BadRequest)]
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

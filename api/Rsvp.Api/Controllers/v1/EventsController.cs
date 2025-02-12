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

  [HttpGet("search")]
  [Produces(MediaTypeNames.Application.Json)]
  [ExpectedFailures(ResultStatus.Error)]
  public async Task<Result<PagedResult<List<EventDto>>>> SearchEvents(
    [FromQuery] int page,
    [FromQuery] int size,
    [FromQuery] string? search = null,
    [FromQuery] string? sort = "date",
    [FromQuery] string? order = "asc",
    CancellationToken cancellationToken = default)
  {
    var query = new GetPaginatedEventsQuery(page, size, search, sort, order);
    return await controllerService.GetPaginatedEventsQueryAsync(query, cancellationToken);
  }

}

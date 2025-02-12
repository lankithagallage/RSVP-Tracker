namespace Rsvp.Application.Services;

using Ardalis.Result;

using MediatR;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;

public class EventsControllerService(ISender mediator) : IEventsControllerService
{
  public async Task<Result<PagedResult<List<EventDto>>>> GetPaginatedEventsQueryAsync(GetPaginatedEventsQuery query,
    CancellationToken cancellationToken)
  {
    return await mediator.Send(query, cancellationToken);
  }
}

public interface IEventsControllerService
{
  public Task<Result<PagedResult<List<EventDto>>>> GetPaginatedEventsQueryAsync(GetPaginatedEventsQuery query,
    CancellationToken cancellationToken);
}

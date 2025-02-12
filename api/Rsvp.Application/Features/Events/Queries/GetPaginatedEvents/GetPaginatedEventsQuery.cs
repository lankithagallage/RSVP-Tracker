namespace Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;

using Ardalis.Result;

using MediatR;

using Rsvp.Application.Features.Events.Dtos;

public class GetPaginatedEventsQuery(int page, int size) : IRequest<Result<PagedResult<List<EventDto>>>>
{
  public int Page { get; set; } = page;
  public int Size { get; set; } = size;
}

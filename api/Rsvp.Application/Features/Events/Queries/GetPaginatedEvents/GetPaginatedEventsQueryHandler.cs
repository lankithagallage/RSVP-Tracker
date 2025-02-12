namespace Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;

using Ardalis.Result;

using AutoMapper;

using MediatR;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Domain.Contexts.Events;

public class GetPaginatedEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
  : IRequestHandler<GetPaginatedEventsQuery, Result<PagedResult<List<EventDto>>>>
{
  public async Task<Result<PagedResult<List<EventDto>>>> Handle(GetPaginatedEventsQuery request,
    CancellationToken cancellationToken)
  {
    var totalCount = await eventRepository.GetTotalCountAsync(cancellationToken);
    var paginatedEvents =
      await eventRepository.GetPaginatedEventsAsync(request.Page, request.Size, cancellationToken);
    var eventDtos = mapper.Map<List<EventDto>>(paginatedEvents);

    var totalPages = request.Size >= totalCount ? 1 : totalCount / request.Size;

    if (request.Page > totalPages)
    {
      return Result<PagedResult<List<EventDto>>>.Invalid(new ValidationError("Page number is too high."));
    }

    var pagedInfo = new PagedInfo(request.Page,
      request.Size,
      totalPages,
      totalCount);

    return Result<PagedResult<List<EventDto>>>.Success(new PagedResult<List<EventDto>>(pagedInfo, eventDtos));
  }
}

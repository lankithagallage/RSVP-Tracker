namespace Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;

using System.Text.RegularExpressions;

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
    var totalCount = await eventRepository.GetTotalCountForPaginationAsync(request.Search, cancellationToken);
    var paginatedEvents =
      await eventRepository.GetPaginatedEventsAsync(request.Page, request.Size, request.Search, cancellationToken);
    var eventDtos = mapper.Map<List<EventDto>>(paginatedEvents);

    var totalPages = request.Size >= totalCount ? 1 : totalCount / request.Size;

    if (request.Page > totalPages)
    {
      return Result<PagedResult<List<EventDto>>>.Invalid(new ValidationError("Page number is too high."));
    }

    if (!string.IsNullOrEmpty(request.Search))
    {
      eventDtos = eventDtos.Select(e => HighlightSearchTerm(e, request.Search)).ToList();
    }

    var pagedInfo = new PagedInfo(request.Page,
      request.Size,
      totalPages,
      totalCount);

    return Result<PagedResult<List<EventDto>>>.Success(new PagedResult<List<EventDto>>(pagedInfo, eventDtos));
  }

  private static EventDto HighlightSearchTerm(EventDto eventDto, string search)
  {
    var searchPattern = Regex.Escape(search);
    const string replacement = "<pre>$0</pre>";

    return new EventDto(
      eventDto.Id,
      Regex.Replace(eventDto.Title, searchPattern, replacement, RegexOptions.IgnoreCase),
      eventDto.Description,
      eventDto.Location,
      eventDto.StartTime,
      eventDto.EndTime);
  }
}

namespace Rsvp.Application.Features.Events.Queries.GetEventById;

using Ardalis.Result;

using AutoMapper;

using MediatR;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Domain.Contexts.Events;

public class GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
  : IRequestHandler<GetEventByIdQuery, Result<EventItemDto>>
{
  public async Task<Result<EventItemDto>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
  {
    var @event = await eventRepository.GetByIdWithAttendeesAsync(request.EventId, cancellationToken);
    if (@event == null)
    {
      return Result<EventItemDto>.NotFound();
    }

    var eventDto = mapper.Map<EventItemDto>(@event);

    return Result<EventItemDto>.Success(eventDto);
  }
}

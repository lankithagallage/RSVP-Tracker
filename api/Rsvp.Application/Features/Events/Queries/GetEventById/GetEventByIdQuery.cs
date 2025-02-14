namespace Rsvp.Application.Features.Events.Queries.GetEventById;

using Ardalis.Result;

using MediatR;

using Rsvp.Application.Features.Events.Dtos;

public class GetEventByIdQuery(Guid eventId) : IRequest<Result<EventItemDto>>
{
  public Guid EventId { get; } = eventId;
}

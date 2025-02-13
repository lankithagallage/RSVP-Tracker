namespace Rsvp.Application.Features.Rsvps.Commands.SaveRsvp;

using Ardalis.Result;

using MediatR;

public class SaveRsvpCommand(Guid eventId, string firstName, string lastName, string email) : IRequest<Result<Guid>>
{
  public Guid EventId { get; set; } = eventId;
  public string FirstName { get; set; } = firstName;
  public string LastName { get; set; } = lastName;
  public string Email { get; set; } = email;
}

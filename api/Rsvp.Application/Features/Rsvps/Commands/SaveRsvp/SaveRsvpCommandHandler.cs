namespace Rsvp.Application.Features.Rsvps.Commands.SaveRsvp;

using Ardalis.Result;

using MediatR;

using Rsvp.Domain.Contexts.Events;
using Rsvp.Domain.Contexts.Rsvps;
using Rsvp.Domain.Contexts.Users;

public class SaveRsvpCommandHandler(
  IEventRepository eventRepository,
  IAttendeeRepository attendeeRepository,
  IUserRepository userRepository)
  : IRequestHandler<SaveRsvpCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(SaveRsvpCommand command, CancellationToken cancellationToken)
  {
    var eventExists = await eventRepository.ExistsAsync(command.EventId, cancellationToken);
    if (!eventExists)
    {
      return Result<Guid>.Invalid(new ValidationError("Event", "Event not found."));
    }

    var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

    if (user == null)
    {
      user = User.CreateNew(command.FirstName, command.LastName, command.Email, UserRole.Attendee);
      await userRepository.AddAsync(user, cancellationToken);
    }

    var @event = await eventRepository.GetByIdWithAttendeesAsync(command.EventId, cancellationToken);
    var attendee = Attendee.CreateNew(@event!, user);
    try
    {
      @event!.AddAttendee(attendee);
      attendee.Confirm();
      await attendeeRepository.AddAsync(attendee, cancellationToken);
    }
    catch (InvalidOperationException ex)
    {
      return Result<Guid>.Conflict(ex.Message);
    }

    return Result<Guid>.Success(attendee.Id);
  }
}

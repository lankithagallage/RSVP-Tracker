namespace Rsvp.Application.Services;

using Ardalis.Result;

using MediatR;

using Rsvp.Application.Features.Rsvps.Commands.SaveRsvp;

public class RsvpControllerService(ISender mediator) : IRsvpControllerService
{
  public async Task<Result<Guid>> SaveRsvpCommandAsync(SaveRsvpCommand command, CancellationToken cancellationToken)
  {
    return await mediator.Send(command, cancellationToken);
  }
}

public interface IRsvpControllerService
{
  Task<Result<Guid>> SaveRsvpCommandAsync(SaveRsvpCommand command, CancellationToken cancellationToken);
}

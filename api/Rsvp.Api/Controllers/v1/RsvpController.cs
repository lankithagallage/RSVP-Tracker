namespace Rsvp.Api.Controllers.v1;

using System.Net.Mime;

using Ardalis.Result;
using Ardalis.Result.AspNetCore;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

using Rsvp.Application.Features.Rsvps.Commands.SaveRsvp;
using Rsvp.Application.Features.Rsvps.Requests;
using Rsvp.Application.Services;

/// <summary>
/// Handles RSVP-related operations such as registering an RSVP for an event.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/rsvps")]
[TranslateResultToActionResult]
public class RsvpController : ControllerBase
{
  private readonly IRsvpControllerService controllerService;

  /// <summary>
  /// Initializes a new instance of the <see cref="RsvpController"/> class.
  /// </summary>
  /// <param name="controllerService">The service handling RSVP business logic.</param>
  public RsvpController(IRsvpControllerService controllerService)
  {
    this.controllerService = controllerService;
  }

  /// <summary>
  /// Registers an RSVP for a specific event.
  /// </summary>
  /// <param name="eventId">The unique identifier of the event.</param>
  /// <param name="request">The RSVP request containing attendee details.</param>
  /// <param name="cancellationToken">A token to cancel the request if needed.</param>
  /// <returns>A result containing the RSVP ID if successful.</returns>
  /// <response code="201">Returns the RSVP ID if successfully created.</response>
  /// <response code="400">If the request is invalid (e.g., missing required fields).</response>
  /// <response code="409">If the RSVP already exists for the given email and event.</response>
  [HttpPost("{eventId:guid}")]
  [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
  public async Task<Result<Guid>> SaveRsvp(Guid eventId, [FromBody] SaveRsvpRequest request,
    CancellationToken cancellationToken)
  {
    var command = new SaveRsvpCommand(eventId, request.FirstName, request.LastName, request.Email);
    return await this.controllerService.SaveRsvpCommandAsync(command, cancellationToken);
  }
}


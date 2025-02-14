namespace Rsvp.Application.Features.Events.Queries.GetEventById;

using FluentValidation;

public class GetEventByIdQueryValidator : Validator<GetEventByIdQuery>
{
  public GetEventByIdQueryValidator()
  {
    this.RuleFor(x => x.EventId)
      .NotEmpty()
      .WithMessage("Event id must not be empty");
  }
}

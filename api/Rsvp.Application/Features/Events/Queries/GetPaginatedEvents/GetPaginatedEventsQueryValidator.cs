namespace Rsvp.Application.Features.Events.Queries.GetPaginatedEvents;

using FluentValidation;

public class GetPaginatedEventsQueryValidator : Validator<GetPaginatedEventsQuery>
{
  public GetPaginatedEventsQueryValidator()
  {
    this.RuleFor(x => x.Page)
      .GreaterThan(0).WithMessage("Page number must be greater than zero.");

    this.RuleFor(x => x.Size)
      .InclusiveBetween(1, 100).WithMessage("Size must be between 1 and 100.");
  }
}

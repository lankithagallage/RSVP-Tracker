namespace Rsvp.Application.Features.Rsvps.Commands.SaveRsvp;

using System.Text.RegularExpressions;

using FluentValidation;

public partial class SaveRsvpCommandValidator : Validator<SaveRsvpCommand>
{
  public SaveRsvpCommandValidator()
  {
    this.RuleFor(x => x.FirstName)
      .NotEmpty().WithMessage("First name cannot be empty.")
      .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

    this.RuleFor(x => x.LastName)
      .NotEmpty().WithMessage("Last name cannot be empty.")
      .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

    this.RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email cannot be empty.")
      .Matches(EmailRegex()).WithMessage("Invalid email format.");
  }

  [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
  private static partial Regex EmailRegex();
}

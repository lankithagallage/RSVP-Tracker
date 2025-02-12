namespace Rsvp.Application.Features;

using FluentValidation;

public abstract class Validator<T> : AbstractValidator<T> where T : class { }

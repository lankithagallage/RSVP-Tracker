namespace Rsvp.Application;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Rsvp.Application.Behaviors;
using Rsvp.Application.Configurations.Mapper;
using Rsvp.Application.Features;
using Rsvp.Application.Services;

public static class DependencyInjection
{
  public static void AddApplicationServices(this IServiceCollection services)
  {
    // Controller Services
    services.AddScoped<IEventsControllerService, EventsControllerService>();
    services.AddScoped<IRsvpControllerService, RsvpControllerService>();

    // AutoMapper
    services.AddAutoMapper(typeof(MappingProfile));

    // MediatR
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(FeatureAssemblyMarker).Assembly));

    // CQRS Validation Pipeline
    services.AddValidatorsFromAssembly(typeof(FeatureAssemblyMarker).Assembly);
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    // CQRS Logging Pipeline
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
  }
}

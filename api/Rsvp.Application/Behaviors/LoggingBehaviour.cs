﻿namespace Rsvp.Application.Behaviors;

using System.Diagnostics;

using MediatR;

using Microsoft.Extensions.Logging;

// Logs CQRS commands and queries
public class LoggingBehavior<TRequest, TResponse>(
  ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    var requestName = typeof(TRequest).Name;
    var stopwatch = Stopwatch.StartNew();

    try
    {
      logger.LogInformation("Handling {RequestName}: {@Request}", requestName, request);

      var response = await next();

      stopwatch.Stop();
      logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms: {@Response}",
        requestName, stopwatch.ElapsedMilliseconds, response);

      return response;
    }
    catch (Exception ex)
    {
      stopwatch.Stop();
      logger.LogError(ex, "Error handling {RequestName} after {ElapsedMilliseconds}ms",
        requestName, stopwatch.ElapsedMilliseconds);
      throw;
    }
  }
}

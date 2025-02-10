namespace Rsvp.Api.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
  public async Task Invoke(HttpContext context)
  {
    this.LogRequest(context);
    await next(context);
  }

  private void LogRequest(HttpContext context)
  {
    var request = context.Request;

    logger.LogInformation("""
                          Incoming Request: HTTP {@method}
                          Host: {@host}
                          Ip: {@Ip}
                          Content-Type: {@contentType}
                          Content-Length: {@contentLength}
                          """,
      $"{request.Method} {request.Path}",
      request.Host.Value,
      context.Connection.RemoteIpAddress?.ToString(),
      request.ContentType,
      request.ContentLength);
  }
}

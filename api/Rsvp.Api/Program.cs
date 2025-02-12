using System.Net;

using Ardalis.Result;
using Ardalis.Result.AspNetCore;

using Rsvp.Api;
using Rsvp.Api.Configuration;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Configurations
    builder.Configuration
      .AddJsonFile("appsettings.Local.json", true, true)
      .AddEnvironmentVariables();

    builder.AddLoggingServices();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers(mvcOptions =>
      {
        mvcOptions.AddResultConvention(resultStatusMap =>
          resultStatusMap.AddDefaultMap()
            .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
              .For("POST", HttpStatusCode.Created)
              .For("DELETE", HttpStatusCode.NoContent))
            .For(ResultStatus.Invalid, HttpStatusCode.BadRequest)
            .For(ResultStatus.Error, HttpStatusCode.InternalServerError)
            .For(ResultStatus.NotFound, HttpStatusCode.NotFound)
        );
      });

    builder.Services.AddServices(builder.Configuration, builder.Environment.IsDevelopment());

    var app = builder.Build().Configure();

    app.Run();
  }
}

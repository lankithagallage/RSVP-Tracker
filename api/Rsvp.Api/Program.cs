using Rsvp.Api;
using Rsvp.Api.Configuration;

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
builder.Services.AddControllers();

builder.Services.AddServices(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build().Configure();

app.Run();

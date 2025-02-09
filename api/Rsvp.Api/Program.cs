using Rsvp.Api;
using Rsvp.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build().Configure();



app.Run();

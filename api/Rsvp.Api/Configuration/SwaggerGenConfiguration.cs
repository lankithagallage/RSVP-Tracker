namespace Rsvp.Api.Configuration;

using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerGenConfiguration(IApiVersionDescriptionProvider provider)
  : IConfigureNamedOptions<SwaggerGenOptions>
{
  /// Configures Swagger to generate API documentation for each discovered API version.
  public void Configure(SwaggerGenOptions options)
  {
    // Iterate through all API versions discovered by the provider
    foreach (var description in provider.ApiVersionDescriptions)
    {
      // Add a Swagger document for each API version
      options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
    }
  }

  /// Configures Swagger options for a specific named configuration.
  /// Calls the main Configure method to apply general settings.
  public void Configure(string? name, SwaggerGenOptions options)
  {
    this.Configure(options); // Call the primary Configure method
  }

  /// Creates OpenAPI information for a given API version.
  private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
  {
    var info = new OpenApiInfo
    {
      Title = "Event RSVP API", // API title displayed in Swagger UI
      Version = description.ApiVersion.ToString(), // API version string
    };

    // Append a deprecation warning if the API version is marked as deprecated
    if (description.IsDeprecated)
    {
      info.Description += " This API version has been deprecated.";
    }

    return info;
  }
}

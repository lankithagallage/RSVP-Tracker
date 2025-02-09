namespace Rsvp.Api.Extensions;

using Asp.Versioning;

using Microsoft.OpenApi.Models;

using Rsvp.Api.Configuration;

/// <summary>
/// Provides extension methods for configuring Swagger services in the application.
/// </summary>
public static class SwaggerExtensions
{
  public static void AddSwaggerServices(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    var version = configuration.GetSection("Version").Get<string>() ??
                  throw new ArgumentException(
                    "Version is required. Please provide a version in the appsettings.json file.");

    // Parse major and minor versions from the version string
    var majorVersion = int.Parse(version.Split('.')[0]);
    var minorVersion = int.Parse(version.Split('.')[1]);

    // Add API versioning services
    serviceCollection.AddVersioningServices(new ApiVersion(majorVersion, minorVersion));

    // Configure Swagger generation options
    serviceCollection.AddSwaggerGen(options =>
    {
      // Enable detection of non-nullable reference types to set the Nullable flag on schema properties
      options.SupportNonNullableReferenceTypes();

      // Enables support for nullable object properties in Swagger documentation
      options.UseAllOfToExtendReferenceSchemas();

      // Use OpenAPI "allOf" for inheritance representation
      options.UseAllOfForInheritance();

      // Use OpenAPI "oneOf" for polymorphic types
      options.UseOneOfForPolymorphism();

      // Map GUIDs to be represented as strings with "uuid" format in Swagger
      options.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "uuid" });

      // Dynamically select subtypes for a given base type (used for polymorphism handling)
      options.SelectSubTypesUsing(baseType =>
      {
        if (baseType == typeof(ValueType))
        {
          return Enumerable.Empty<Type>(); // Avoid selecting primitive types
        }

        // Find all types that inherit from the base type within the same assembly
        var types = baseType.Assembly
          .GetTypes()
          .Where(t => t.IsSubclassOf(baseType))
          .Where(
            t => !t.IsGenericType || t.GenericTypeArguments.ElementAtOrDefault(0) != default
          );

        return types;
      });
    });

    // Configure additional Swagger options through a separate configuration class
    serviceCollection.ConfigureOptions<SwaggerGenConfiguration>();
  }

  private static void AddVersioningServices(this IServiceCollection serviceCollection, ApiVersion version)
  {
    serviceCollection
      .AddApiVersioning(options =>
      {
        // Set the default API version
        options.DefaultApiVersion = version;

        // Assume default version if no version is specified in the request
        options.AssumeDefaultVersionWhenUnspecified = true;

        // Enable reporting supported API versions in the response headers
        options.ReportApiVersions = true;
      })
      .AddApiExplorer(setup =>
      {
        // Define the API version format (e.g., "v1", "v1.0")
        setup.GroupNameFormat = "'v'VVV";

        // Enable substitution of API versions in route URLs
        setup.SubstituteApiVersionInUrl = true;
      });
  }
}

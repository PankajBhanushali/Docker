/**
 * Day 2: Docker Images - Building Optimized Images
 * 
 * Learning Objectives:
 * - Build efficient multi-stage Docker images
 * - Understand image layers and caching
 * - Optimize image size (Alpine base images)
 * - Image versioning and tagging
 * - Push/pull images from registry
 * - Security best practices
 * 
 * Lab Exercises:
 * 1. Compare image sizes with different base images
 * 2. Understand layer caching and build optimization
 * 3. Build with Alpine Linux (minimal size)
 * 4. Tag and version images properly
 * 5. Create a private registry and push images
 */

using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Docker Training Day 2",
        Version = "1.0.0",
        Description = "Building and optimizing Docker images"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Image metadata endpoint
app.MapGet("/api/image-info", () => new 
{
    buildTime = GetBuildTime(),
    version = GetVersion(),
    framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
    os = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
    hostname = System.Net.Dns.GetHostName(),
    processorCount = Environment.ProcessorCount
})
    .WithName("ImageInfo")
    .WithOpenApi()
    .Produces<ImageInfoResponse>();

// Layer information endpoint
app.MapGet("/api/layers", () => new 
{
    layers = new[] 
    {
        new { stage = "build", purpose = "Compile application", cleaned = true },
        new { stage = "publish", purpose = "Publish artifacts", cleaned = true },
        new { stage = "runtime", purpose = "Run application", cleaned = false }
    },
    totalLayers = 3,
    explanation = "Multi-stage build reduces final image size by discarding build tools"
})
    .WithName("LayerInfo")
    .WithOpenApi();

// Image size estimation endpoint
app.MapGet("/api/size-comparison", () => new 
{
    comparison = new[]
    {
        new { baseImage = "aspnet:8.0", sizeGb = 0.6, layers = 15, useCases = "Full framework features" },
        new { baseImage = "aspnet:8.0-alpine", sizeGb = 0.15, layers = 8, useCases = "Minimal setup, smaller image" },
        new { baseImage = "dotnet:8.0-runtime", sizeGb = 0.42, layers = 12, useCases = "No ASP.NET, lightweight API" },
        new { baseImage = "dotnet:8.0-runtime-alpine", sizeGb = 0.1, layers = 6, useCases = "Minimal runtime, smallest size" }
    },
    recommendation = "Use Alpine for production deployments to reduce attack surface and deployment time"
})
    .WithName("SizeComparison")
    .WithOpenApi();

// Docker.json endpoint for introspection
app.MapGet("/api/docker-metadata", () => new 
{
    createdAt = DateTime.UtcNow.AddDays(-1),
    environment = new Dictionary<string, string>
    {
        { "ASPNETCORE_ENVIRONMENT", app.Environment.EnvironmentName },
        { "DOTNET_RUNNING_IN_CONTAINER", "true" }
    },
    labels = new Dictionary<string, string>
    {
        { "maintainer", "Docker Training Team" },
        { "version", "2.0.0" },
        { "training", "day-2" }
    }
})
    .WithName("DockerMetadata")
    .WithOpenApi();

app.Run();

static string GetBuildTime()
{
    var assembly = Assembly.GetExecutingAssembly();
    var resourceName = "Day2_Docker_Images.BuildTime.txt";
    
    using (var stream = assembly.GetManifestResourceStream(resourceName))
    {
        if (stream == null)
            return DateTime.UtcNow.ToString("o");
        
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}

static string GetVersion()
{
    var assembly = Assembly.GetExecutingAssembly();
    var version = assembly.GetName().Version;
    return version?.ToString() ?? "1.0.0.0";
}

public record ImageInfoResponse(
    string BuildTime,
    string Version,
    string Framework,
    string OS,
    string Hostname,
    int ProcessorCount
);

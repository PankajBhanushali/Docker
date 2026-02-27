/**
 * Day 1: Docker Basics - Simple ASP.NET Core API
 * 
 * Learning Objectives:
 * - Understand Docker concepts (images, containers, registry)
 * - Build your first Docker image
 * - Run and manage containers
 * - Explore container lifecycle
 * 
 * Lab Exercises:
 * 1. Build this application
 * 2. Create a Dockerfile for this application
 * 3. Build the Docker image: docker build -t day1-app:latest .
 * 4. Run the container: docker run -p 5000:8080 day1-app:latest
 * 5. Access the API at http://localhost:5000/api/health
 */

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.MapGet("/api/health", () => new { status = "healthy", timestamp = DateTime.UtcNow })
    .WithName("HealthCheck")
    .WithOpenApi()
    .Produces<HealthCheckResponse>();

app.MapGet("/api/info", () => new { 
    application = "Docker Training Day 1",
    environment = app.Environment.EnvironmentName,
    version = "1.0.0",
    hostname = System.Net.Dns.GetHostName()
})
    .WithName("AppInfo")
    .WithOpenApi();

app.Run();

public record HealthCheckResponse(string status, DateTime timestamp);

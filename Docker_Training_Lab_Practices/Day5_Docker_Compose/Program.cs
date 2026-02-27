/**
 * Day 5: Docker Compose - Multi-Container Orchestration
 * 
 * Learning Objectives:
 * - Define multi-container applications with Docker Compose
 * - Service dependencies and health checks
 * - Environment variables and secrets
 * - Networking between services
 * - Volume management in Compose
 * - Development vs Production configurations
 * - Scaling services
 * 
 * Lab Exercises:
 * 1. Build and run Docker Compose stack
 * 2. Inter-service communication
 * 3. Database initialization and migrations
 * 4. Cache integration
 * 5. Logging and monitoring
 */

using System.Data;
using Npgsql;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var connectionString = builder.Configuration["DATABASE_CONNECTION_STRING"] 
    ?? "Server=postgres;Port=5432;Database=training;User Id=postgres;Password=Training@123;";
var redisConnection = builder.Configuration["REDIS_CONNECTION_STRING"] 
    ?? "redis:6379";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Docker Compose - Day 5",
        Version = "1.0.0",
        Description = "Multi-container application orchestration"
    });
});

// Register Redis
try
{
    var redis = ConnectionMultiplexer.Connect(redisConnection);
    builder.Services.AddSingleton(redis);
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: Could not connect to Redis: {ex.Message}");
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Health endpoint
app.MapGet("/health", async (IConnectionMultiplexer? redis) =>
{
    var checks = new Dictionary<string, bool>();

    // Check database
    try
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            await conn.OpenAsync();
            checks["database"] = conn.State == ConnectionState.Open;
        }
    }
    catch (Exception ex)
    {
        checks["database"] = false;
        Console.WriteLine($"Database health check failed: {ex.Message}");
    }

    // Check Redis
    if (redis != null)
    {
        try
        {
            var server = redis.GetServer(redis.GetEndPoints().First());
            checks["redis"] = server.Ping().IsTrue;
        }
        catch (Exception ex)
        {
            checks["redis"] = false;
            Console.WriteLine($"Redis health check failed: {ex.Message}");
        }
    }
    else
    {
        checks["redis"] = false;
    }

    var allHealthy = checks.Values.All(v => v);
    return Results.Ok(new { 
        status = allHealthy ? "healthy" : "degraded",
        checks,
        timestamp = DateTime.UtcNow
    });
})
    .WithName("HealthCheck")
    .WithOpenApi();

// Service status endpoint
app.MapGet("/api/services", (IConnectionMultiplexer? redis) =>
{
    return Results.Ok(new 
    {
        services = new[]
        {
            new { name = "web-api", url = "http://web-api:8080", status = "running" },
            new { name = "postgres", url = "postgres:5432", status = "running" },
            new { name = "redis", url = "redis:6379", status = redis != null ? "running" : "unavailable" },
            new { name = "nginx", url = "http://nginx:80", status = "running" }
        },
        timestamp = DateTime.UtcNow
    });
})
    .WithName("Services")
    .WithOpenApi();

// Database info endpoint
app.MapGet("/api/database-info", async () =>
{
    try
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using (var cmd = new NpgsqlCommand("SELECT version();", conn))
            {
                var version = await cmd.ExecuteScalarAsync();
                return Results.Ok(new 
                {
                    connected = true,
                    version = version?.ToString() ?? "unknown",
                    connectionString = "***hidden***"
                });
            }
        }
    }
    catch (Exception ex)
    {
        return Results.Ok(new { connected = false, error = ex.Message });
    }
})
    .WithName("DatabaseInfo")
    .WithOpenApi();

// Redis cache endpoint
app.MapPost("/api/cache/{key}", async (string key, Dictionary<string, string> data, IConnectionMultiplexer? redis) =>
{
    if (redis == null)
        return Results.BadRequest("Redis not available");

    try
    {
        var db = redis.GetDatabase();
        var value = System.Text.Json.JsonSerializer.Serialize(data);
        await db.StringSetAsync(key, value, TimeSpan.FromHours(1));
        return Results.Created($"/api/cache/{key}", new { key, stored = true });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("SetCacheValue")
    .WithOpenApi();

app.MapGet("/api/cache/{key}", async (string key, IConnectionMultiplexer? redis) =>
{
    if (redis == null)
        return Results.BadRequest("Redis not available");

    try
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        
        if (!value.HasValue)
            return Results.NotFound(new { key, found = false });
        
        return Results.Ok(new { key, value = value.ToString(), found = true });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
    .WithName("GetCacheValue")
    .WithOpenApi();

// Compose configuration endpoint
app.MapGet("/api/compose-info", () =>
{
    return Results.Ok(new 
    {
        application = "Docker Compose Training - Day 5",
        services = new[]
        {
            new { 
                name = "web-api",
                image = "day5-webapi:latest",
                container = "day5-web-api",
                port = 8080
            },
            new { 
                name = "postgres",
                image = "postgres:16-alpine",
                container = "day5-postgres",
                port = 5432
            },
            new { 
                name = "redis",
                image = "redis:7-alpine",
                container = "day5-redis",
                port = 6379
            },
            new { 
                name = "nginx",
                image = "nginx:alpine",
                container = "day5-nginx",
                port = 80
            }
        },
        network = "training-network",
        volumes = new[] { "postgres-data", "redis-data" },
        timestamp = DateTime.UtcNow
    });
})
    .WithName("ComposeInfo")
    .WithOpenApi();

app.Run();

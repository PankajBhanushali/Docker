/**
 * Day 3: Docker Networking - Multi-Container Communication
 * 
 * Learning Objectives:
 * - Understand Docker networking types (bridge, host, overlay, none)
 * - Container-to-container communication
 * - Service discovery
 * - Port mapping and exposure
 * - DNS resolution in Docker networks
 * - Custom bridge networks
 * 
 * Lab Exercises:
 * 1. Create custom Docker networks
 * 2. Run multiple containers on same network
 * 3. Test inter-container communication
 * 4. Environment variables for service discovery
 * 5. HTTP calls between containers
 */

using System.Net;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Docker Networking - Day 3",
        Version = "1.0.0",
        Description = "Multi-container communication and service discovery"
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

// Network info endpoint
app.MapGet("/api/network-info", () => new 
{
    hostname = System.Net.Dns.GetHostName(),
    containerIp = GetContainerIP(),
    environment = new Dictionary<string, string>
    {
        { "CONTAINER_ID", Environment.GetEnvironmentVariable("CONTAINER_ID") ?? "N/A" },
        { "NETWORK", Environment.GetEnvironmentVariable("DOCKER_NETWORK") ?? "bridge" },
        { "SERVICE_NAME", Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "app" }
    },
    timestamp = DateTime.UtcNow
})
    .WithName("NetworkInfo")
    .WithOpenApi()
    .Produces<NetworkInfoResponse>();

// DNS resolution test
app.MapPost("/api/resolve-service", async (Dictionary<string, string> request) =>
{
    var serviceName = request.GetValueOrDefault("service", "");
    if (string.IsNullOrEmpty(serviceName))
        return Results.BadRequest("service parameter required");

    try
    {
        var addresses = await Dns.GetHostAddressesAsync(serviceName);
        return Results.Ok(new 
        { 
            service = serviceName, 
            ips = addresses.Select(a => a.ToString()).ToList(),
            resolved = true 
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new 
        { 
            service = serviceName, 
            error = ex.Message,
            resolved = false 
        });
    }
})
    .WithName("ResolveService")
    .WithOpenApi();

// Inter-container communication
app.MapGet("/api/call-service/{serviceName}", async (string serviceName) =>
{
    var port = Environment.GetEnvironmentVariable("INTERNAL_PORT") ?? "8080";
    var url = $"http://{serviceName}:{port}/api/network-info";

    try
    {
        using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
        {
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Results.Ok(new 
            { 
                success = true,
                service = serviceName,
                statusCode = response.StatusCode,
                data = content
            });
        }
    }
    catch (Exception ex)
    {
        return Results.Ok(new 
        { 
            success = false,
            service = serviceName,
            error = ex.Message
        });
    }
})
    .WithName("CallService")
    .WithOpenApi();

// Network diagnostics
app.MapGet("/api/network-diagnostics", () => new 
{
    networkMode = GetNetworkMode(),
    ipAddress = GetContainerIP(),
    gateway = GetGateway(),
    dnsServers = GetDNSServers(),
    openPorts = new[] { 8080 },
    exposedPorts = new[] { "8080/tcp" },
    networkTypes = new[] 
    {
        new { type = "bridge", secure = false, useCases = "Single-host inter-container communication" },
        new { type = "host", secure = false, useCases = "Direct network access, no isolation" },
        new { type = "overlay", secure = true, useCases = "Multi-host swarm communication" },
        new { type = "none", secure = true, useCases = "No networking, maximum isolation" }
    }
})
    .WithName("NetworkDiagnostics")
    .WithOpenApi();

// Container linking simulation
app.MapGet("/api/linked-services", () => new 
{
    linkedServices = new[]
    {
        new { name = "redis", host = "redis", port = 6379, protocol = "tcp" },
        new { name = "postgres", host = "postgres", port = 5432, protocol = "tcp" },
        new { name = "api", host = "api", port = 8080, protocol = "http" }
    },
    discoveryMethod = "DNS resolution via container name",
    recommendation = "Use custom bridge networks instead of legacy linking"
})
    .WithName("LinkedServices")
    .WithOpenApi();

app.Run();

static string GetContainerIP()
{
    try
    {
        using (var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            var endPoint = socket.LocalEndPoint as IPEndPoint;
            return endPoint?.Address.ToString() ?? "127.0.0.1";
        }
    }
    catch
    {
        return "127.0.0.1";
    }
}

static string GetNetworkMode()
{
    var dockerHost = Environment.GetEnvironmentVariable("DOCKER_HOST");
    if (!string.IsNullOrEmpty(dockerHost))
        return "host";
    
    var isDocker = Environment.GetEnvironmentVariable("DOCKER_NETWORK");
    return isDocker ?? "bridge";
}

static string GetGateway()
{
    // In Docker container, gateway is typically .1 of the network
    var ip = GetContainerIP();
    var parts = ip.Split('.');
    if (parts.Length == 4 && int.TryParse(parts[3], out int lastOctet))
    {
        parts[3] = "1";
        return string.Join(".", parts);
    }
    return "172.17.0.1";
}

static string[] GetDNSServers()
{
    return new[] { "127.0.0.11:53", "8.8.8.8" };
}

public record NetworkInfoResponse(
    string Hostname,
    string ContainerIp,
    Dictionary<string, string> Environment,
    DateTime Timestamp
);

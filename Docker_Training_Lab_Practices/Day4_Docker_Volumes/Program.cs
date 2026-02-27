/**
 * Day 4: Docker Volumes - Data Persistence
 * 
 * Learning Objectives:
 * - Understand volume types (named, bind mounts, tmpfs)
 * - Persist data across container restarts
 * - Share data between containers
 * - Volume drivers and plugins
 * - Backup and restore volumes
 * - Mount options and permissions
 * 
 * Lab Exercises:
 * 1. Work with named volumes
 * 2. Use bind mounts for development
 * 3. Create volume snapshots
 * 4. Share volumes between containers
 * 5. Implement data backup strategies
 */

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Docker Volumes - Day 4",
        Version = "1.0.0",
        Description = "Data persistence and volume management"
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

// Create data directory
var dataDir = "/data";
var filesDir = Path.Combine(dataDir, "files");
if (!Directory.Exists(filesDir))
{
    Directory.CreateDirectory(filesDir);
}

// Store data from request
app.MapPost("/api/store-data", async (Dictionary<string, string> request) =>
{
    try
    {
        var key = request.GetValueOrDefault("key", $"data-{DateTime.UtcNow:yyyyMMdd-HHmmss}");
        var value = request.GetValueOrDefault("value", "");
        
        var filePath = Path.Combine(filesDir, $"{key}.json");
        var data = new { key, value, stored = DateTime.UtcNow, file = filePath };
        
        await System.IO.File.WriteAllTextAsync(filePath, System.Text.Json.JsonSerializer.Serialize(data));
        
        return Results.Created($"/api/retrieve-data/{key}", new 
        { 
            success = true,
            key,
            message = $"Data stored to {filePath}"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
})
    .WithName("StoreData")
    .WithOpenApi();

// Retrieve stored data
app.MapGet("/api/retrieve-data/{key}", async (string key) =>
{
    try
    {
        var filePath = Path.Combine(filesDir, $"{key}.json");
        
        if (!System.IO.File.Exists(filePath))
            return Results.NotFound(new { success = false, error = "Data not found" });
        
        var content = await System.IO.File.ReadAllTextAsync(filePath);
        return Results.Ok(new { success = true, key, data = content });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
})
    .WithName("RetrieveData")
    .WithOpenApi();

// List all stored data
app.MapGet("/api/list-data", () =>
{
    try
    {
        var files = Directory.GetFiles(filesDir, "*.json")
            .Select(f => new { 
                filename = Path.GetFileName(f),
                size = new FileInfo(f).Length,
                created = System.IO.File.GetCreationTime(f),
                modified = System.IO.File.GetLastWriteTime(f)
            })
            .ToList();
        
        return Results.Ok(new 
        { 
            success = true,
            count = files.Count,
            files
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
})
    .WithName("ListData")
    .WithOpenApi();

// Volume information
app.MapGet("/api/volume-info", () => new 
{
    dataDirectory = dataDir,
    filesDirectory = filesDir,
    diskSpace = new
    {
        total = GetDriveSpace().total,
        used = GetDriveSpace().used,
        available = GetDriveSpace().available
    },
    volumeTypes = new[] 
    {
        new { 
            type = "named", 
            path = "/var/lib/docker/volumes/<name>/_data",
            managed = true,
            portable = true,
            use = "Production data, databases" 
        },
        new { 
            type = "bind", 
            path = "/host/path:/container/path",
            managed = false,
            portable = false,
            use = "Development, source code" 
        },
        new { 
            type = "tmpfs", 
            path = "memory only",
            managed = true,
            portable = false,
            use = "Temporary data, caches" 
        }
    }
})
    .WithName("VolumeInfo")
    .WithOpenApi();

// Backup current data
app.MapPost("/api/backup", async () =>
{
    try
    {
        var backupDir = Path.Combine(dataDir, "backups");
        Directory.CreateDirectory(backupDir);
        
        var backupFileName = $"backup-{DateTime.UtcNow:yyyyMMdd-HHmmss}.tar";
        var backupPath = Path.Combine(backupDir, backupFileName);
        
        // In real scenario, would use tar/zip command
        var backupInfo = new 
        {
            timestamp = DateTime.UtcNow,
            path = backupPath,
            status = "Backup would be created here"
        };
        
        await System.IO.File.WriteAllTextAsync(
            Path.Combine(backupDir, $"backup-manifest-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json"),
            System.Text.Json.JsonSerializer.Serialize(backupInfo)
        );
        
        return Results.Ok(new { success = true, backup = backupInfo });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
})
    .WithName("BackupData")
    .WithOpenApi();

// Cleanup old data
app.MapDelete("/api/cleanup-old-data", () =>
{
    try
    {
        var files = Directory.GetFiles(filesDir, "*.json");
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        
        var deletedFiles = files
            .Where(f => System.IO.File.GetLastWriteTime(f) < thirtyDaysAgo)
            .Select(f => 
            {
                System.IO.File.Delete(f);
                return Path.GetFileName(f);
            })
            .ToList();
        
        return Results.Ok(new 
        { 
            success = true,
            deletedCount = deletedFiles.Count,
            deletedFiles
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { success = false, error = ex.Message });
    }
})
    .WithName("CleanupOldData")
    .WithOpenApi();

app.Run();

static (long total, long used, long available) GetDriveSpace()
{
    try
    {
        var drive = System.IO.DriveInfo.GetDrives().FirstOrDefault(d => d.Name == "/");
        if (drive != null)
        {
            return (drive.TotalSize, drive.TotalSize - drive.AvailableFreeSpace, drive.AvailableFreeSpace);
        }
    }
    catch { }
    
    return (0, 0, 0);
}

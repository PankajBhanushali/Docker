# Day 4: Docker Volumes - Data Persistence

## Overview
Learn how to persist data across container restarts and shut-downs using volumes, bind mounts, and advanced storage strategies.

## Learning Objectives
- ✓ Understand volume types and drivers
- ✓ Create and manage named volumes
- ✓ Use bind mounts for development
- ✓ Share volumes between containers
- ✓ Implement backup and restore workflows
- ✓ Handle permissions and ownership
- ✓ Monitor volume usage and cleanup

## Prerequisites
- Completion of Days 1-3
- Docker Desktop
- .NET 8 SDK
- Basic file system knowledge

## Lab Exercises

### Exercise 1: Create Named Volume
```bash
# Create a named volume
docker volume create training-data

# List volumes
docker volume ls

# Inspect volume
docker volume inspect training-data
```

### Exercise 2: Build Day 4 Image
```bash
cd Day4_Docker_Volumes
docker build -t day4-app:latest .

# Verify build
docker images day4-app
```

### Exercise 3: Run Container with Named Volume
```bash
# Run container with named volume mounted
docker run -d \
  --name data-app1 \
  -v training-data:/data \
  -p 5020:8080 \
  day4-app:latest

# Wait for startup
sleep 3

# Test the API
curl http://localhost:5020/api/volume-info
```

### Exercise 4: Store Data in Volume
```bash
# Store data through API
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"user-data","value":"Important information"}' \
  http://localhost:5020/api/store-data

# Store more data
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"config","value":"Production config"}' \
  http://localhost:5020/api/store-data

# List all stored data
curl http://localhost:5020/api/list-data
```

### Exercise 5: Verify Data Persistence
```bash
# Retrieve data
curl http://localhost:5020/api/retrieve-data/user-data

# Stop and remove container
docker stop data-app1
docker rm data-app1

# Run new container with same volume
docker run -d \
  --name data-app2 \
  -v training-data:/data \
  -p 5021:8080 \
  day4-app:latest

sleep 3

# Data persists! Retrieve same data
curl http://localhost:5021/api/retrieve-data/user-data

# List data (should show same files)
curl http://localhost:5021/api/list-data
```

### Exercise 6: Inspect Volume Directly
```bash
# On Docker Desktop for Windows/Mac, inspect via container
docker run -it --rm -v training-data:/data alpine ls -la /data/files/

# View volume location
docker volume inspect training-data | grep Mountpoint

# On Windows with WSL2:
# Volume usually at: \\wsl$\docker-desktop-data\version-pack-data\community\docker\volumes\{volume-name}\_data
```

### Exercise 7: Bind Mount for Development
```bash
# Create local directory
mkdir -p ./local-data

# Run container with bind mount
docker run -d \
  --name dev-app \
  -v %cd%\local-data:/data \
  -p 5022:8080 \
  day4-app:latest

# On Linux/Mac: -v $(pwd)/local-data:/data
# On Windows PowerShell: -v ${PWD}\local-data:/data

sleep 3

# Store data
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"dev-config","value":"Development settings"}' \
  http://localhost:5022/api/store-data

# Check local directory - files are there!
ls -la ./local-data/files/
cat ./local-data/files/dev-config.json
```

### Exercise 8: Share Volume Between Containers
```bash
# Run second container with same named volume
docker run -d \
  --name data-app3 \
  -v training-data:/data \
  -p 5023:8080 \
  day4-app:latest

# Data from app1 is accessible in app3!
curl http://localhost:5023/api/list-data

# Store data in app3
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"shared-data","value":"Shared between containers"}' \
  http://localhost:5023/api/store-data

# Retrieve in app1
curl http://localhost:5020/api/retrieve-data/shared-data
```

### Exercise 9: Read-Only Volume
```bash
# Mount volume as read-only
docker run -d \
  --name readonly-app \
  -v training-data:/data:ro \
  -p 5024:8080 \
  day4-app:latest

sleep 3

# Can read data
curl http://localhost:5024/api/list-data

# Cannot write data (would fail)
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"fail","value":"This should fail"}' \
  http://localhost:5024/api/store-data
```

### Exercise 10: Backup Volume
```bash
# Create backup directory
mkdir -p ./backups

# Backup volume using tar
docker run --rm \
  -v training-data:/data \
  -v %cd%\backups:/backup \
  alpine tar czf /backup/training-data.tar.gz -C /data .

# On Linux/Mac: -v $(pwd)/backups:/backup

# Verify backup
ls -lh ./backups/

# View backup contents
tar tzf ./backups/training-data.tar.gz
```

### Exercise 11: Restore Volume from Backup
```bash
# Create new volume
docker volume create restored-data

# Restore from backup
docker run --rm \
  -v restored-data:/data \
  -v %cd%\backups:/backup \
  alpine tar xzf /backup/training-data.tar.gz -C /data

# Verify restoration
docker run --rm \
  -v restored-data:/data \
  alpine ls -la /data/files/
```

### Exercise 12: Volume Cleanup
```bash
# List unused volumes
docker volume ls

# Remove unused volumes
docker volume prune

# Remove specific volume
docker volume rm training-data

# Warning: This cannot be undone!
```

### Exercise 13: Tmpfs (Temporary Data)
```bash
# Mount temporary file system in memory
docker run -d \
  --name temp-app \
  --tmpfs /temp_data:size=100m \
  -p 5025:8080 \
  day4-app:latest

sleep 3

# Data stored here is temporary and fast
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"temp","value":"Temporary data in memory"}' \
  http://localhost:5025/api/store-data

# Stop container
docker stop temp-app

# Data is gone (not persisted)
```

### Exercise 14: Volume Permissions
```bash
# Check permissions in volume
docker exec data-app1 ls -la /data/

# Check owner
docker exec data-app1 stat /data/

# Change permissions
docker exec data-app1 chmod 755 /data/

# Note: Can be tricky with bind mounts - user ID mapping matters
```

### Exercise 15: Cleanup
```bash
# Stop all containers
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)

# View volumes
docker volume ls

# Keep volumes for demonstration, or remove all
docker volume prune
```

## Key Concepts

### Volume Types

#### 1. Named Volumes
```
docker run -v volume-name:/container/path

Advantages:
  • Managed by Docker
  • Easy backup/restore
  • Works across containers
  • Independent of container lifecycle
  • Can specify driver

Disadvantages:
  • Stored in Docker's managed location
  • Less direct access from host
```

#### 2. Bind Mounts
```
docker run -v /host/path:/container/path

Advantages:
  • Direct host filesystem access
  • Great for development
  • Can mount individual files
  • Full control over location

Disadvantages:
  • Must exist on host
  • Not portable across machines
  • Permission issues possible
  • Less stable than named volumes
```

#### 3. Tmpfs Mounts
```
docker run --tmpfs /container/path

Advantages:
  • Fast (in-memory)
  • Automatic cleanup
  • No disk I/O
  • Temporary secrets/caches

Disadvantages:
  • Lost on container stop
  • Limited by RAM size
  • Cannot be shared
  • Not for persistent data
```

### Volume Data Flow
```
Host OS                          Container
┌─────────────────────┐         ┌──────────────────┐
│ Host Filesystem     │◄────────│ Container FS     │
│ /var/mydata    ◄─────Bind────►│ /app/data   │
│                     │    Mount │                  │
└─────────────────────┘         └──────────────────┘
       ↓                               ↓
   Host                           Container
   Data                           Data
```

### Volume Lifecycle
```
Create Volume → Mount to Container → Store Data → Unmount → Persist

✓ Data survives container restart
✓ Data survives container removal
✓ Data survives image update
✗ Data removed with volume deletion
```

### Mount Options
```bash
# Read-only
-v volume:/path:ro

# Read-write (default)
-v volume:/path:rw

# Bind mount with options
-v /host/path:/container/path:rslave

# Tmpfs options
--tmpfs /path:size=100m,mode=1777
```

## Volume Management Commands

| Command | Purpose |
|---------|---------|
| `docker volume create` | Create new volume |
| `docker volume ls` | List volumes |
| `docker volume inspect` | View volume details |
| `docker volume rm` | Remove volume |
| `docker volume prune` | Remove unused volumes |
| `docker volume ls -f dangling=true` | Find unused volumes |

## Best Practices

### ✓ DO
- Use named volumes for production data
- Use bind mounts for development only
- Back up important volumes regularly
- Monitor volume usage and cleanup
- Use read-only mounts when appropriate
- Set proper permissions for shared data
- Use health checks to validate data integrity

### ✗ DON'T
- Store persistent data in container layer (use volumes)
- Use bind mounts for production (use named volumes)
- Mix data from multiple applications in one volume
- Forget to backup before major changes
- Leave dangling volumes consuming space
- Make volumes world-writable (security risk)
- Assume containers share volumes without declaration

## Data Backup Strategy

### Daily Backup
```bash
docker run --rm \
  --volumes-from backup-container \
  -v /backups:/backup \
  alpine \
  tar czf /backup/daily-$(date +%Y%m%d).tar.gz -C /data .
```

### Weekly Full Backup
```bash
docker run --rm \
  --volumes-from backup-container \
  -v /backups:/backup \
  alpine \
  tar czf /backup/weekly-$(date +%Y%V).tar.gz -C /data .
```

### Restore Procedure
```bash
1. Create recovery volume
2. Run container with recovery volume
3. Extract backup into recovery volume
4. Verify data integrity
5. Switch application to recovery volume
```

## Troubleshooting

### Cannot write to volume
```bash
# Check permissions
docker exec <container> ls -la /data

# Check owner
docker exec <container> stat /data

# Solution: Fix permissions in Dockerfile
RUN chown -R appuser:appgroup /data
```

### Volume permission denied on bind mount
```bash
# Issue: User ID mismatch between host and container
# Solution: Use proper permission mapping

docker run -u $(id -u):$(id -g) \
  -v /host/path:/container/path \
  image

# Or adjust container user to match host
```

### Cannot backup bind mount
```bash
# Bind mounts are just directories, back them up normally
tar czf backup.tar.gz /host/path/

# No need for docker volume backup commands
```

## Next Steps
- Orchestrate volumes with Docker Compose (Day 5)
- Advanced: Storage drivers and plugins
- Advanced: Backup and disaster recovery


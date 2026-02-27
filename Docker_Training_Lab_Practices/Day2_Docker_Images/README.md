# Day 2: Docker Images - Building and Optimizing

## Overview
Learn how to build efficient, optimized Docker images using multi-stage builds, Alpine Linux, and best practices.

## Learning Objectives
- ✓ Build multi-stage Docker images
- ✓ Understand image layers and caching
- ✓ Optimize image size using Alpine Linux
- ✓ Proper image tagging and versioning
- ✓ Add metadata labels and health checks
- ✓ Run containers as non-root users
- ✓ Push/pull from Docker registries

## Prerequisites
- Completion of Day 1
- Docker Desktop
- .NET 8 SDK

## Lab Exercises

### Exercise 1: Build Standard Image (Full Framework)
```bash
# Build using full ASP.NET Core image
docker build -t day2-app:full -f Dockerfile .

# Check image size
docker images | grep day2-app

# Expected: ~600MB
```

### Exercise 2: Build Alpine Image (Optimized)
```bash
# Build using Alpine base image
docker build -t day2-app:alpine -f Dockerfile.Alpine .

# Check Alpine image size
docker images | grep day2-app

# Expected: ~150MB (75% size reduction!)
```

### Exercise 3: Compare Image Sizes
```bash
# View both images
docker images day2-app

# Expected output:
# REPOSITORY  TAG    SIZE
# day2-app    full   600MB
# day2-app    alpine 150MB
```

### Exercise 4: Run Alpine Container
```bash
# Run the Alpine container
docker run -d -p 5001:8080 --name day2-alpine day2-app:alpine

# Test endpoints
curl http://localhost:5001/api/image-info
curl http://localhost:5001/api/size-comparison
curl http://localhost:5001/api/layers
```

### Exercise 5: Examine Image Layers
```bash
# View image history (layers)
docker history day2-app:alpine

# Detailed inspection
docker inspect day2-app:alpine

# Compare layer counts
docker history day2-app:full
docker history day2-app:alpine
```

### Exercise 6: Image Tagging and Versioning
```bash
# Tag with version
docker tag day2-app:alpine day2-app:2.0.0
docker tag day2-app:alpine day2-app:latest

# View all tags for this image
docker images day2-app

# Tag with registry (for pushing)
docker tag day2-app:alpine docker.io/yourusername/day2-app:2.0.0
docker tag day2-app:alpine docker.io/yourusername/day2-app:latest
```

### Exercise 7: Health Check
```bash
# Run with health check enabled
docker run -d -p 5002:8080 --name day2-health day2-app:alpine

# Check container health status
docker inspect day2-health | grep -A 10 "Health"

# View logs with health check info
docker logs day2-health
```

### Exercise 8: Container with Security Best Practices
```bash
# Run with resource limits
docker run -d \
  -p 5003:8080 \
  --name day2-secure \
  --memory="256m" \
  --cpus="0.5" \
  --read-only \
  --tmpfs /tmp \
  day2-app:alpine

# Verify security settings
docker inspect day2-secure

# Test the app still works
curl http://localhost:5003/api/image-info
```

### Exercise 9: Build Cache Optimization
```bash
# First build (full build time)
docker build -t day2-app:v1 --progress=plain .

# Make no changes and rebuild (should use cache)
docker build -t day2-app:v2 --progress=plain .

# Change a line and rebuild (cache busts from that layer)
# Modify Program.cs and rebuild - notice build time increases
```

### Exercise 10: Push to Registry (Optional)
```bash
# Login to Docker Hub
docker login

# Tag with registry information
docker tag day2-app:alpine yourusername/day2-app:2.0.0

# Push image
docker push yourusername/day2-app:2.0.0

# Verify on Docker Hub
curl https://hub.docker.com/v2/repositories/yourusername/day2-app/

# Pull on another machine
docker pull yourusername/day2-app:2.0.0
```

### Exercise 11: Cleanup
```bash
# Stop and remove containers
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)

# Remove images (but keep for Day 3)
# docker rmi day2-app:full day2-app:alpine day2-app:latest
```

## Key Concepts

### Multi-Stage Builds
- **Build Stage**: Compiles source code (heavy with SDKs)
- **Publish Stage**: Creates deployment artifacts
- **Runtime Stage**: Runs the application (minimal dependencies)
- **Benefit**: Final image only contains runtime, not build tools

### Image Layers
- Each Dockerfile instruction creates a layer
- Layers are cached independently
- Changing a layer invalidates cache for all subsequent layers
- Organize instructions to maximize cache hit rate

### Alpine Linux
- Minimal Linux distribution (~5MB base)
- Reduces image size by 75-90%
- Fewer packages = smaller attack surface
- Trade-off: May require additional tools (like `curl`)

### Image Metadata
- **LABEL**: Key-value pairs for image metadata
- **HEALTHCHECK**: Container health monitoring
- **USER**: Run as non-root for security
- **ENV**: Environment variables

### Image Size Optimization
| Strategy | Impact | Trade-off |
|----------|--------|-----------|
| Multi-stage build | -70% | Slightly complex Dockerfile |
| Alpine base | -85% | May need extra packages |
| .dockerignore | -10-20% | Must maintain file |
| Layer caching | Build time | Organization overhead |
| Distroless images | -95% | Limited debugging tools |

### Security Best Practices
1. **Non-root User**: Limit damage if container compromised
2. **Health Checks**: Automatic container restart on failure
3. **Resource Limits**: Prevent resource exhaustion
4. **Read-only FS**: Prevent unauthorized writes
5. **Minimal Base Image**: Reduce attack surface
6. **Layer Security**: Scan images for vulnerabilities

### Tagging Strategy
```
<registry>/<repository>/<image>:<tag>

Examples:
docker.io/mycompany/myapp:1.0.0
docker.io/mycompany/myapp:latest
docker.io/mycompany/myapp:production
127.0.0.1:5000/myapp:dev
```

## Docker Image Commands Reference
| Command | Purpose |
|---------|---------|
| `docker build` | Build image from Dockerfile |
| `docker images` | List images with sizes |
| `docker history` | View image layers |
| `docker inspect` | Detailed image metadata |
| `docker tag` | Create image alias/version |
| `docker rmi` | Remove image |
| `docker push` | Upload image to registry |
| `docker pull` | Download image from registry |
| `docker save` | Export image to tar |
| `docker load` | Import image from tar |

## Dockerfile Best Practices

### Layer Ordering (put stable things first)
```dockerfile
# ✓ Good: Stable layers first
FROM ...
RUN apt-get update && apt-get install -y stable-deps
COPY project.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build

# ✗ Bad: Frequently changing layers first
FROM ...
COPY . .
RUN dotnet build
RUN apt-get update && apt-get install -y stable-deps
```

### Minimize Layer Size
```dockerfile
# ✓ Good: Single RUN with cleanup
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# ✗ Bad: Multiple RUNs increase size
RUN apt-get update
RUN apt-get install -y curl
RUN rm -rf /var/lib/apt/lists/*
```

### Use .dockerignore
```
Exclude unnecessary files from build context:
- .git
- node_modules
- *.log
- bin/
- obj/
- .vs
```

## Image Size Benchmarks
- Full ASP.NET Core: ~600MB
- ASP.NET Core Alpine: ~150MB
- Distroless .NET: ~100MB
- Scratch + binary: ~50MB

## Troubleshooting

### Image build too slow
- Check .dockerignore file
- Reorganize Dockerfile for better caching
- Use `--cache-from` to leverage previous builds

### Image size too large
- Switch to Alpine base
- Implement multi-stage build
- Remove unnecessary packages
- Use distroless images

### Layer inspection
```bash
# Detailed layer info
docker history --human day2-app:alpine

# See disk usage
docker system df

# Clean up unused images
docker image prune
```

## Next Steps
- Learn about container networking (Day 3)
- Manage persistent data with volumes (Day 4)
- Orchestrate multiple containers (Day 5)


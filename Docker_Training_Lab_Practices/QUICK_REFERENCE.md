# Docker Training Lab Practices - Quick Reference Card

## 📋 Command Cheat Sheet

### Basic Docker Commands
```bash
# Images
docker build -t name:tag .                  # Build image
docker images                               # List images
docker rmi image:tag                        # Remove image
docker history image                        # View layers
docker inspect image                        # Get details

# Containers
docker run -d -p host:container image       # Run container
docker ps                                   # List running
docker ps -a                                # List all
docker logs container                       # View logs
docker logs -f container                    # Follow logs
docker stop container                       # Stop container
docker start container                      # Start container
docker rm container                         # Remove container
docker exec -it container bash             # Access shell
docker inspect container                    # Get details

# Networks
docker network ls                           # List networks
docker network create name                 # Create network
docker network inspect name                # View network
docker network connect net container       # Connect container

# Volumes
docker volume ls                            # List volumes
docker volume create name                  # Create volume
docker volume inspect name                 # View volume
docker volume rm name                      # Remove volume

# Registry
docker push image:tag                       # Push to registry
docker pull image:tag                       # Pull from registry
docker login                                # Login to Docker Hub
```

### Docker Compose Commands
```bash
docker-compose up -d                       # Start all services
docker-compose down                        # Stop all services
docker-compose ps                          # List services
docker-compose logs -f                     # Follow logs
docker-compose exec service cmd            # Run command
docker-compose restart service             # Restart service
docker-compose build                       # Build images
docker-compose config                      # Validate config
docker-compose pull                        # Pull images
```

---

## 🎯 Day-by-Day Quick Reference

### Day 1: Docker Basics
```bash
cd Day1_Docker_Basics
docker build -t day1-app:latest .
docker run -d -p 5000:8080 --name day1 day1-app:latest
curl http://localhost:5000/api/health
docker logs day1
docker stop day1 && docker rm day1
```

### Day 2: Docker Images
```bash
cd Day2_Docker_Images
docker build -t day2-app:full -f Dockerfile .
docker build -t day2-app:alpine -f Dockerfile.Alpine .
docker images | grep day2-app                # Compare sizes
docker history day2-app:alpine              # View layers
docker tag day2-app:latest docker.io/user/day2-app:1.0
```

### Day 3: Docker Networking
```bash
cd Day3_Docker_Networking
docker build -t day3-app:latest .
docker network create training-network
docker run -d --network training-network --name api day3-app:latest
docker exec api curl http://localhost:8080/api/network-info
docker network inspect training-network
```

### Day 4: Docker Volumes
```bash
cd Day4_Docker_Volumes
docker volume create training-data
docker run -d -v training-data:/data -p 5020:8080 day4-app:latest
curl -X POST -H "Content-Type: application/json" \
  -d '{"key":"data1","value":"test"}' \
  http://localhost:5020/api/store-data
docker volume inspect training-data
```

### Day 5: Docker Compose
```bash
cd Day5_Docker_Compose
docker-compose up -d
docker-compose logs -f
curl http://localhost:5030/health
docker-compose exec postgres psql -U postgres -d training -c "\dt"
docker-compose down -v
```

---

## 🔧 Docker Run Options Reference

```bash
docker run [OPTIONS] IMAGE [COMMAND]

# Common options:
-d                          # Detached mode (background)
-p host:container          # Port mapping
-v volume:/path            # Volume mount
-e VAR=value               # Environment variable
--name name                # Container name
--network net              # Connect to network
-u user                    # User to run as
--rm                       # Auto-remove on exit
-it                        # Interactive terminal
--restart policy           # Restart policy (no, always, unless-stopped)
--cpus 0.5                 # CPU limit
--memory 512m              # Memory limit
--health-cmd CMD           # Health check command
--expose port              # Document port
-w /path                   # Working directory
--entrypoint cmd           # Override entrypoint
```

---

## 📊 Image Size Reference

| Base Image | Size | Use Case |
|-----------|------|----------|
| dotnet:8.0-sdk | 800MB | Development |
| aspnet:8.0 | 600MB | Full .NET Framework |
| aspnet:8.0-alpine | 150MB | Production |
| dotnet:8.0-runtime | 420MB | Console apps |
| dotnet:8.0-runtime-alpine | 100MB | Minimal |
| alpine:latest | 7MB | Utilities only |

---

## 🌐 Networking Ports Reference

| Service | Port | Protocol |
|---------|------|----------|
| Web API (Day 1) | 5000 (host) → 8080 | HTTP |
| Web API (Day 2) | 5001 (host) → 8080 | HTTP |
| App1 (Day 3) | 5010 (host) → 8080 | HTTP |
| App2 (Day 4) | 5020 (host) → 8080 | HTTP |
| Compose Web API | 5030 (host) → 8080 | HTTP |
| PostgreSQL | 5432 → 5432 | TCP |
| Redis | 6379 → 6379 | TCP |
| Nginx | 80, 443 → 80, 443 | HTTP/HTTPS |
| PgAdmin | 5050 (host) → 80 | HTTP |

---

## 🐳 Docker Architecture

```
┌─────────────────────────────────────────┐
│         Docker Desktop/Engine           │
├─────────────────────────────────────────┤
│ Docker Daemon (dockerd)                 │
│  ├─ Container Runtime (containerd)      │
│  ├─ Image Service                       │
│  ├─ Network Manager                     │
│  └─ Volume Manager                      │
├─────────────────────────────────────────┤
│ Docker CLI (docker commands)            │
└─────────────────────────────────────────┘
```

---

## 🔐 Security Checklist

- [ ] Use non-root user in containers
- [ ] Scan images for vulnerabilities
- [ ] Use health checks
- [ ] Limit container resources
- [ ] Use read-only filesystems when possible
- [ ] Keep base images updated
- [ ] Store secrets separately (not in Dockerfile)
- [ ] Use specific base image tags (not :latest)
- [ ] Minimize layers and image size
- [ ] Remove build tools from runtime images

---

## 🐛 Debugging Commands

```bash
# View logs with timestamps
docker logs --timestamps container

# Follow logs for specific service
docker-compose logs -f service-name

# Inspect full container details
docker inspect container | less

# Check resource usage
docker stats

# View all images and sizes
docker images --format "table {{.Repository}}\t{{.Size}}"

# Test network connectivity
docker exec container ping host
docker exec container nslookup service-name
docker exec container curl http://service:port

# Access container filesystem
docker exec -it container /bin/bash
```

---

## ⚡ Performance Tips

### Build Optimization
- Use .dockerignore to exclude unnecessary files
- Order Dockerfile steps: stable → frequently changing
- Use specific base image tags
- Cache layers effectively
- Use multi-stage builds

### Runtime Optimization
- Use Alpine base images (-75% size)
- Set resource limits
- Use health checks
- Implement proper logging
- Remove unnecessary packages

### Network Optimization
- Use custom bridge networks
- Enable DNS caching
- Use local DNS resolver
- Reduce inter-container hops

---

## 📈 Common Mistakes to Avoid

❌ **Wrong**
```dockerfile
FROM ubuntu:latest
RUN apt-get update && apt-get install -y build-essential
COPY . /app
RUN cd /app && ./build.sh
```

✅ **Right**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
COPY --from=build /app/publish .
USER appuser
EXPOSE 8080
```

---

## 🔄 Common Workflows

### Development Workflow
```bash
docker build -t myapp:latest .
docker run -d -v $(pwd):/app -p 8080:8080 myapp:latest
# Make code changes (volume syncs)
docker logs -f <container>
```

### Production Workflow
```bash
docker build --build-arg ENV=prod -t myapp:1.0.0 .
docker tag myapp:1.0.0 registry.com/myapp:1.0.0
docker tag myapp:1.0.0 registry.com/myapp:latest
docker push registry.com/myapp:1.0.0
```

### Testing Workflow
```bash
docker-compose -f docker-compose.test.yml up
docker-compose -f docker-compose.test.yml logs
docker-compose -f docker-compose.test.yml down -v
```

---

## 📞 Quick Help

- `docker help` - Docker CLI help
- `docker COMMAND --help` - Specific command help
- `docker-compose --help` - Docker Compose help
- [Docker Docs](https://docs.docker.com/)
- [Play with Docker](https://labs.play-with-docker.com/)

---

**Print this card for quick reference during training!**


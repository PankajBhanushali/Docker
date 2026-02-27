# Day 1: Docker Basics

## Overview
Learn the fundamentals of Docker by building and running your first containerized ASP.NET Core application.

## Learning Objectives
- ✓ Install and configure Docker
- ✓ Understand Docker images and containers
- ✓ Build a Docker image from a Dockerfile
- ✓ Run containers and manage their lifecycle
- ✓ Access applications running in containers
- ✓ Explore Docker commands (ps, logs, exec, stop, rm)

## Prerequisites
- Docker Desktop installed
- .NET 8 SDK installed
- Basic command line knowledge

## Lab Exercises

### Exercise 1: Setup and Verification
```bash
# Verify Docker installation
docker --version
docker run hello-world
```

### Exercise 2: Build the Project
```bash
# Restore dependencies and build the ASP.NET Core project
dotnet build
```

### Exercise 3: Create Docker Image
```bash
# Build the Docker image
docker build -t day1-app:latest .

# List images to verify
docker images
```

### Exercise 4: Run Container
```bash
# Run container and expose port
docker run -d -p 5000:8080 --name day1-container day1-app:latest

# View running containers
docker ps
```

### Exercise 5: Test the Application
```bash
# Health check endpoint
curl http://localhost:5000/api/health

# Application info endpoint
curl http://localhost:5000/api/info

# Or access Swagger UI at http://localhost:5000/swagger
```

### Exercise 6: Container Management
```bash
# View container logs
docker logs day1-container
docker logs -f day1-container  # Follow logs

# Execute command inside container
docker exec day1-container ls -la /app

# Inspect container details
docker inspect day1-container

# Stop the container
docker stop day1-container

# View stopped containers
docker ps -a

# Restart the container
docker start day1-container

# Remove the container
docker rm day1-container
```

### Exercise 7: Cleanup
```bash
# Stop all containers
docker stop $(docker ps -q)

# Remove all stopped containers
docker rm $(docker ps -a -q)

# Remove the image
docker rmi day1-app:latest
```

## Key Concepts

### Docker Image
- A lightweight, standalone, executable package containing everything needed to run the application
- Built from instructions in a Dockerfile
- Immutable template for containers

### Docker Container
- A running instance of a Docker image
- Isolated process with its own filesystem, networking, and environment
- Ephemeral by default (removed when stopped)

### Dockerfile
- Text file containing instructions to build a Docker image
- Uses multi-stage builds for optimization
- Stages: build → publish → final runtime

### Ports
- `-p 5000:8080` maps container port 8080 to host port 5000
- Container exposes 8080 internally
- Access via http://localhost:5000 on host

## Common Docker Commands
| Command | Purpose |
|---------|---------|
| `docker build` | Build image from Dockerfile |
| `docker run` | Create and run container |
| `docker ps` | List running containers |
| `docker ps -a` | List all containers |
| `docker logs` | View container output |
| `docker exec` | Execute command in container |
| `docker stop` | Stop running container |
| `docker start` | Start stopped container |
| `docker rm` | Remove container |
| `docker rmi` | Remove image |
| `docker inspect` | View detailed container info |

## Troubleshooting

### Port already in use
```bash
# Use a different port
docker run -p 5001:8080 day1-app:latest

# Find what's using the port
netstat -ano | findstr :5000  # Windows
lsof -i :5000  # Linux/Mac
```

### Container won't start
```bash
# Check logs for errors
docker logs <container_id>

# Inspect the container
docker inspect <container_id>
```

### Image build fails
```bash
# Build with verbose output
docker build -t day1-app:latest . --progress=plain

# Check that Dockerfile exists
dir Dockerfile  # Windows
ls -la Dockerfile  # Linux
```

## Next Steps
- Explore container networking (Day 3)
- Learn about volumes and data persistence (Day 4)
- Work with multiple containers using Docker Compose (Day 5)


# Day 5: Docker Compose - Multi-Container Orchestration

## Overview
Master Docker Compose to orchestrate multi-container applications with databases, caches, and reverse proxies.

## Learning Objectives
- ✓ Define services in docker-compose.yml
- ✓ Service dependencies and startup order
- ✓ Health checks for reliability
- ✓ Inter-service networking and discovery
- ✓ Volume management and persistence
- ✓ Environment variables and configuration
- ✓ Development vs production configurations
- ✓ Logging and monitoring

## Prerequisites
- Completion of Days 1-4
- Docker Desktop
- .NET 8 SDK
- Docker Compose (usually included with Docker Desktop)

## Lab Exercises

### Exercise 1: Verify Docker Compose Installation
```bash
# Check Docker Compose version
docker-compose --version

# Expected: Docker Compose version 2.x.x or higher
```

### Exercise 2: Build the Day 5 Application
```bash
cd Day5_Docker_Compose

# Build the Web API image
docker build -t day5-webapi:latest -f Dockerfile.WebAPI .

# Verify build
docker images | grep day5
```

### Exercise 3: Start All Services with Docker Compose
```bash
# Start services in background
docker-compose up -d

# Verify all services are running
docker-compose ps

# Expected output: 4 services (web-api, postgres, redis, nginx)
```

### Exercise 4: View Service Logs
```bash
# View all services logs
docker-compose logs

# Follow logs in real-time
docker-compose logs -f

# View specific service logs
docker-compose logs web-api
docker-compose logs postgres
docker-compose logs -f redis

# Exit follow mode: Ctrl+C
```

### Exercise 5: Test Health Checks
```bash
# Check service status via HTTP
curl http://localhost:5030/health

# Check services endpoint
curl http://localhost:5030/api/services

# Check external access via Nginx
curl http://localhost/health
```

### Exercise 6: Database Connection
```bash
# Test database connectivity
curl http://localhost:5030/api/database-info

# Expected: Connected to PostgreSQL 16

# Access database directly with psql
docker exec day5-postgres psql -U postgres -d training -c "SELECT * FROM training_logs LIMIT 5;"

# Or using Docker
docker run -it --rm \
  --network training-network \
  postgres:16-alpine \
  psql -h postgres -U postgres -d training -c "SELECT COUNT(*) FROM training_logs;"
```

### Exercise 7: Redis Cache Integration
```bash
# Set a cache value
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"key":"session-123","value":"user-data","timestamp":"2024-02-27"}' \
  http://localhost:5030/api/cache/session-123

# Retrieve cache value
curl http://localhost:5030/api/cache/session-123

# Access Redis directly
docker exec day5-redis redis-cli KEYS "*"
docker exec day5-redis redis-cli GET session-123
```

### Exercise 8: Inter-Service Communication
```bash
# From nginx, call web-api
docker exec day5-nginx curl http://web-api:8080/api/services

# From web-api, connect to postgres
docker exec day5-web-api curl http://localhost:8080/api/database-info

# From web-api, access redis
docker exec day5-web-api curl http://localhost:8080/api/cache/test-key
```

### Exercise 9: Environment Variables
```bash
# View container environment
docker exec day5-web-api env | grep -E "DATABASE|REDIS|SERVICE"

# View compose configuration
docker-compose config

# View service configuration
docker-compose config --services
```

### Exercise 10: View Volumes
```bash
# List volumes created by compose
docker volume ls | grep day5 || docker volume ls | grep training

# Inspect volume
docker volume inspect docker_compose_postgres-data

# List contents of volume
docker run --rm \
  -v day5_docker_compose_postgres-data:/data \
  alpine ls -la /data
```

### Exercise 11: Scale Services (if applicable)
```bash
# Compose file limitation: Can't easily scale dependent services
# But you can run additional instances:

docker run -d \
  --name web-api-2 \
  --network training-network \
  -p 5031:8080 \
  -e DATABASE_CONNECTION_STRING="Server=postgres;..." \
  day5-webapi:latest

# Now you have a load-balanced setup with Nginx
curl http://localhost/api/services  # Routed through Nginx
```

### Exercise 12: Development Configuration
```bash
# Use development override configuration
# This would rebuild with development target and enable additional services

docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

# Check for pgAdmin (database UI)
# Access at: http://localhost:5050
# Login: admin@training.local / Admin@123

# PgAdmin should show postgres as linked server
```

### Exercise 13: Production Configuration
```bash
# Build with production settings
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# Check resource limits are applied
docker stats

# Services run with stricter constraints and better logging
```

### Exercise 14: Restart Services
```bash
# Stop services without removing
docker-compose stop

# Verify stopped
docker-compose ps

# Start stopped services
docker-compose start

# Verify running
docker-compose ps
```

### Exercise 15: Update Configuration
```bash
# Modify docker-compose.yml (e.g., change port)
# Edit: ports: - "5031:8080"

# Update running services
docker-compose up -d

# Only changed services are recreated
```

### Exercise 16: Logs and Debugging
```bash
# Check for errors in services
docker-compose logs | grep -i error

# See real-time logs with timestamps
docker-compose logs -f --timestamps

# Get specific lines from logs
docker-compose logs -f web-api | head -50
```

### Exercise 17: Execute Commands in Services
```bash
# Run command in web-api
docker-compose exec web-api ls -la /app

# View database tables
docker-compose exec postgres psql -U postgres -d training -c "\dt"

# Redis info
docker-compose exec redis redis-cli INFO

# Check Nginx config
docker-compose exec nginx nginx -T
```

### Exercise 18: Cleanup and Shutdown
```bash
# Stop and remove containers (keep volumes)
docker-compose down

# Stop and remove containers AND volumes
docker-compose down -v

# Verify everything is removed
docker-compose ps
docker volume ls
```

## Docker Compose File Structure

### Services
```yaml
services:
  service-name:
    image: image:tag
    container_name: friendly-name
    ports:
      - "host:container"
    environment:
      - VAR=value
    volumes:
      - volume:/path
    depends_on:
      - other-service
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost/health"]
      interval: 30s
      timeout: 10s
      retries: 3
```

### Networks
```yaml
networks:
  service-network:
    driver: bridge
```

### Volumes
```yaml
volumes:
  named-volume:
    driver: local
```

## Key Concepts

### Service Discovery
```
In Docker Compose:
  service-name        ← DNS resolution via service name
  service-name:port   ← Connect to specific port
  
Example:
  web-api   → http://web-api:8080
  postgres  → Server=postgres;Port=5432
  redis     → redis:6379
```

### Dependencies
```yaml
depends_on:
  postgres:
    condition: service_healthy  # Wait for health check
```

### Health Checks
```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
  interval: 30s        # Check every 30 seconds
  timeout: 10s         # Timeout after 10 seconds
  retries: 3          # Fail after 3 failures
  start_period: 40s   # Grace period after start
```

### Environment Variables
```yaml
# Option 1: Inline
environment:
  - DATABASE_PASSWORD=secret
  - REDIS_HOST=redis

# Option 2: From file
env_file:
  - .env

# Option 3: Interpolation
environment:
  - COMPOSE_PROJECT_NAME=${COMPOSE_PROJECT_NAME}
```

### Volumes
```yaml
# Named volume
volumes:
  - postgres-data:/var/lib/postgresql/data

# Bind mount
volumes:
  - ./src:/app/src

# Read-only mount
volumes:
  - ./config:/app/config:ro
```

## Common Docker Compose Commands

| Command | Purpose |
|---------|---------|
| `docker-compose up` | Create and start containers |
| `docker-compose up -d` | Start in background |
| `docker-compose ps` | List running services |
| `docker-compose logs` | View logs |
| `docker-compose logs -f` | Follow logs |
| `docker-compose exec` | Run command in service |
| `docker-compose stop` | Stop services |
| `docker-compose start` | Start services |
| `docker-compose restart` | Restart services |
| `docker-compose down` | Stop and remove |
| `docker-compose down -v` | Remove volumes too |
| `docker-compose build` | Build images |
| `docker-compose pull` | Download images |
| `docker-compose config` | Validate config |
| `docker-compose images` | List images |

## Development Workflow

### 1. Initial Setup
```bash
docker-compose up -d
docker-compose logs -f web-api
```

### 2. Make Changes to Code
```bash
# Edit Program.cs or other files
# If using watch mode: automatically rebuilds
```

### 3. Test Changes
```bash
docker-compose exec web-api curl http://localhost:8080/api/test
```

### 4. Debug Issues
```bash
docker-compose logs web-api
docker-compose exec database command
```

### 5. Shutdown
```bash
docker-compose down
```

## Production Deployment

### Best Practices

✓ **Use separate docker-compose files**
```bash
docker-compose -f docker-compose.yml \
               -f docker-compose.prod.yml up -d
```

✓ **Set resource limits**
```yaml
deploy:
  resources:
    limits:
      cpus: '1'
      memory: 512M
```

✓ **Configure logging**
```yaml
logging:
  driver: "json-file"
  options:
    max-size: "10m"
    max-file: "3"
```

✓ **Use health checks**
```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost/health"]
  interval: 30s
  timeout: 10s
  retries: 3
```

✓ **Store secrets properly**
```bash
# Use Docker secrets (Swarm mode) or external secret management
# NOT inline in docker-compose.yml
```

## Troubleshooting

### Service won't start
```bash
docker-compose logs service-name
docker-compose ps
docker inspect service-name  # Container ID
```

### Services can't communicate
```bash
# Verify network
docker network ls | grep compose

# Check DNS
docker-compose exec service-a nslookup service-b
```

### Volumes not persisting
```bash
# Verify volume mount
docker inspect container-name | grep Mounts

# Check volume exists
docker volume ls
```

### Port conflicts
```bash
# Change port in docker-compose.yml
ports:
  - "5031:8080"  # Use 5031 instead of 5030

# Rebuild
docker-compose up -d
```

## Next Steps
- Advanced: Docker Swarm for orchestration
- Advanced: Kubernetes deployment
- Advanced: Service mesh (Istio, Linkerd)
- Advanced: GitOps workflows


# Day 5: Multi-Container Application with Docker Compose

This directory demonstrates Docker Compose configuration for orchestrating multiple containers.

## Files in this directory

- `docker-compose.yml` - Main composition file with services
- `docker-compose.override.yml` - Development overrides
- `docker-compose.prod.yml` - Production configuration
- `Day5_WebAPI/` - Web API service
- `Day5_Database/` - Database service
- `Day5_Cache/` - Cache service (Redis)

## Quick Start

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Remove volumes
docker-compose down -v
```

## Services in this Compose

1. **Web API** - Main ASP.NET Core application
2. **PostgreSQL Database** - Relational database
3. **Redis Cache** - In-memory cache
4. **Nginx** - Reverse proxy/load balancer

## Network

All services communicate on a custom network called `training-network`.

Service discovery:
- Web API: `web-api:8080`
- Database: `postgres:5432`
- Cache: `redis:6379`
- Nginx: `nginx:80`

## Volumes

Named volumes for data persistence:
- `postgres-data` - Database files
- `redis-data` - Cache persistence (optional)

## Building

Build DAY 5 ASP.NET Core application first, or use pre-built images.


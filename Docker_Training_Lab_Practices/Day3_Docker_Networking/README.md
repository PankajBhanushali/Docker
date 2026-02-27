# Day 3: Docker Networking

## Overview
Master Docker networking to enable communication between multiple containers using custom networks, service discovery, and DNS resolution.

## Learning Objectives
- ✓ Understand Docker network drivers (bridge, host, overlay, none)
- ✓ Create and manage custom bridge networks
- ✓ Container-to-container communication
- ✓ Service discovery via DNS
- ✓ Port mapping and exposure
- ✓ Environment variables for service configuration
- ✓ Troubleshoot network connectivity

## Prerequisites
- Completion of Day 1 & 2
- Docker Desktop
- .NET 8 SDK
- Network diagnostic tools (curl, netstat)

## Lab Exercises

### Exercise 1: Explore Default Networks
```bash
# List all networks
docker network ls

# Should show: bridge, host, none

# Inspect default bridge network
docker network inspect bridge

# View network details
docker network inspect bridge | grep -E "Containers|Gateway|Subnet"
```

### Exercise 2: Build the Day 3 Image
```bash
cd Day3_Docker_Networking
docker build -t day3-app:latest .

# Verify build
docker images day3-app
```

### Exercise 3: Run Container on Default Bridge Network
```bash
# Run single container on default bridge
docker run -d -p 5010:8080 --name day3-app1 day3-app:latest

# Wait for startup
sleep 3

# Test connectivity
curl http://localhost:5010/api/network-info

# Get container IP
docker inspect day3-app1 | grep "IPAddress" | head -1
```

### Exercise 4: Create Custom Bridge Network
```bash
# Create custom network
docker network create training-network

# Verify creation
docker network ls

# Inspect custom network
docker network inspect training-network
```

### Exercise 5: Connect Containers on Custom Network
```bash
# Run two containers on custom network
docker run -d \
  --name web-server \
  --network training-network \
  -e SERVICE_NAME=web-server \
  -e INTERNAL_PORT=8080 \
  day3-app:latest

docker run -d \
  --name api-server \
  --network training-network \
  -e SERVICE_NAME=api-server \
  -e INTERNAL_PORT=8080 \
  day3-app:latest

# Verify both are running
docker ps
```

### Exercise 6: Test DNS Service Discovery
```bash
# Access web-server container
docker exec web-server curl http://api-server:8080/api/network-info

# Output should show api-server hostname and IP

# Reverse query
docker exec api-server curl http://web-server:8080/api/network-info

# Test DNS resolution endpoint
docker exec web-server curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"service":"api-server"}' \
  http://localhost:8080/api/resolve-service
```

### Exercise 7: Inter-Container Communication
```bash
# From web-server, call api-server
docker exec web-server curl http://localhost:8080/api/call-service/api-server

# Should successfully call and return api-server info

# Try calling non-existent service
docker exec web-server curl http://localhost:8080/api/call-service/nonexistent
```

### Exercise 8: Port Mapping
```bash
# Run container with custom port mapping
docker run -d \
  --name api-mapped \
  --network training-network \
  -p 5011:8080 \
  day3-app:latest

# Access via host port
curl http://localhost:5011/api/network-info

# But containers on network can access via container port
docker exec web-server curl http://api-mapped:8080/api/network-info
```

### Exercise 9: Host Network Mode
```bash
# Run container in host network mode (Linux only, limited on Windows)
# This shares host network interface - containers see all host ports

docker run -d \
  --name host-mode-app \
  --network host \
  day3-app:latest

# On Linux: container shares host network exactly
# On Windows/Mac: limited support due to VM layer
```

### Exercise 10: Multiple Network Assignment
```bash
# Connect existing container to another network
docker network create secondary-network
docker network connect secondary-network api-server

# Inspect container - should show both networks
docker inspect api-server | grep "Networks" -A 20

# Test connectivity on both networks
docker exec web-server curl http://api-server:8080/api/network-info
```

### Exercise 11: Network Diagnostics
```bash
# Check network configuration
docker exec web-server curl http://localhost:8080/api/network-diagnostics

# Advanced diagnostics from container
docker exec web-server ip addr
docker exec web-server ip route
docker exec web-server cat /etc/resolv.conf
```

### Exercise 12: Environment Variables for Discovery
```bash
# Run with environment variables
docker run -d \
  --name config-app \
  --network training-network \
  -e REDIS_HOST=redis \
  -e REDIS_PORT=6379 \
  -e DATABASE_HOST=postgres \
  -e DATABASE_PORT=5432 \
  day3-app:latest

# Verify environment in container
docker exec config-app env | grep -E "REDIS|DATABASE"
```

### Exercise 13: Disconnecting from Network
```bash
# Disconnect container from network
docker network disconnect training-network api-server

# Container should lose connectivity to others on that network
docker exec web-server curl http://api-server:8080/api/network-info
# Should fail with connection refused

# Reconnect
docker network connect training-network api-server

# Should work again
docker exec web-server curl http://api-server:8080/api/network-info
```

### Exercise 14: Network Cleanup
```bash
# Remove containers
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)

# Remove networks (custom only)
docker network rm training-network secondary-network

# Verify
docker network ls
```

## Key Concepts

### Docker Network Drivers

| Driver | Scope | Use Case | Isolation |
|--------|-------|----------|-----------|
| **bridge** | Single host | Single-host multi-container | High |
| **host** | Single host | Direct network access, no isolation | None |
| **overlay** | Multi-host | Docker Swarm, multi-host services | High |
| **none** | Isolated | No networking | Complete |
| **macvlan** | Single/Multi-host | Legacy applications needing MAC | Medium |

### Bridge Network (Default)
```
Host Network Stack
      ↓
docker0 (virtual bridge interface)
      ↓
veth interfaces (virtual ethernet pairs)
      ↓
Containers (172.17.0.2, 172.17.0.3, etc.)
```

### Custom Bridge Network
- **Automatic DNS**: Containers resolve each other by name
- **Network Isolation**: Containers can't see other networks
- **User-Defined**: Better for multi-container applications
- **Recommended**: Use for most production scenarios

### Service Discovery
```
Container A (API)          Container B (Web)
      ↓                         ↓
http://api:8080 ←DNS→ Resolve "api" to 172.18.0.2
      ↓                         ↓
     port 8080              Connect to 172.18.0.2:8080
```

### Port Mapping vs Exposure
```
Port Mapping:     -p 5000:8080  (host:container)
  ├─ Access: http://localhost:5000
  ├─ Visibility: Only from host
  └─ Use: External access

Port Exposure:    EXPOSE 8080   (in Dockerfile)
  ├─ Access: container-to-container via :8080
  ├─ Visibility: Only on network
  └─ Use: Inter-container communication
```

## Docker Network Commands Reference

| Command | Purpose |
|---------|---------|
| `docker network ls` | List all networks |
| `docker network create` | Create new network |
| `docker network inspect` | View network details |
| `docker network connect` | Connect container to network |
| `docker network disconnect` | Disconnect container from network |
| `docker network rm` | Remove network |
| `docker network prune` | Remove unused networks |

## DNS in Docker

### Embedded DNS Server
- Address: `127.0.0.11:53`
- Automatic entry for each container on custom bridge
- Resolves container names to IPs
- Resolves external domains via host DNS

### Example Resolution
```
Custom Bridge Network (172.18.0.0/16):
  - web-app: 172.18.0.2
  - api-server: 172.18.0.3
  - redis: 172.18.0.4

When web-app resolves "api-server":
  curl http://api-server:8080
    ↓
  DNS query to 127.0.0.11:53
    ↓
  Returns 172.18.0.3
    ↓
  TCP connection established to 172.18.0.3:8080
```

## Inter-Container Communication Best Practices

### ✓ DO
- Create named custom networks for groups of related containers
- Use container names for DNS resolution
- Set `EXPOSE` in Dockerfile for documented ports
- Use environment variables for service endpoints
- Implement health checks for reliability

### ✗ DON'T
- Rely on container IP addresses (they can change)
- Use default bridge network in production
- Hard-code service IPs in application code
- Mix network drivers for same application
- Forget to document exposed ports

## Network Isolation Example

```bash
# Container A on network1 cannot reach Container B on network2
docker network create network1
docker network create network2

docker run -d --network network1 --name app-a day3-app:latest
docker run -d --network network2 --name app-b day3-app:latest

# This fails - app-a cannot resolve app-b
docker exec app-a curl http://app-b:8080/api/network-info

# But app-a can reach app-b if connected to both
docker network connect network2 app-a
# Now it works!
```

## Troubleshooting

### Container cannot reach another container
```bash
# 1. Verify both on same network
docker inspect <container> | grep "NetworkSettings" -A 30

# 2. Check DNS resolution
docker exec <container> nslookup <target-container>

# 3. Verify ports are exposed
docker exec <container> netstat -tuln

# 4. Check firewall rules
docker exec <container> iptables -L -n
```

### DNS resolution fails
```bash
# Test DNS server
docker exec <container> curl http://127.0.0.11:53
docker exec <container> nslookup api-server 127.0.0.11

# Check /etc/resolv.conf in container
docker exec <container> cat /etc/resolv.conf

# Should show: nameserver 127.0.0.11
```

### Port already in use
```bash
# Find what's using the port on host
netstat -ano | findstr :5000

# Use different port
docker run -p 5001:8080 day3-app:latest
```

## Next Steps
- Persist data with volumes (Day 4)
- Orchestrate multi-container apps with Docker Compose (Day 5)
- Advanced: Docker Swarm and Kubernetes networking


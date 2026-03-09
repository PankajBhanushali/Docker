# Docker Training Lab Practices for SQEs (C# Projects)

A comprehensive 5-day hands-on Docker training program designed for Software Quality Engineers. Each day builds upon previous knowledge with practical C# ASP.NET Core applications.

## 📅 Program Structure

### **Day 1: Docker Basics**
*Foundations and first container*
- [Day 1 README](Day1_Docker_Basics/README.md)
- **Topics**: Images, containers, registry, Docker lifecycle
- **Project**: Simple ASP.NET Core Web API
- **Skills**: Build images, run containers, manage container lifecycle
- **Exercises**: 7 hands-on labs

### **Day 2: Docker Images**
*Building and optimizing images*
- [Day 2 README](Day2_Docker_Images/README.md)
- **Topics**: Multi-stage builds, Alpine Linux, image optimization, security
- **Project**: Compare full framework vs optimized images
- **Skills**: Image tagging, layer optimization, health checks
- **Exercises**: 11 hands-on labs

### **Day 3: Docker Networking**
*Inter-container communication*
- [Day 3 README](Day3_Docker_Networking/README.md)
- **Topics**: Bridge networks, DNS, service discovery, port mapping
- **Project**: Multi-container network communication
- **Skills**: Create networks, inter-container communication, debugging
- **Exercises**: 14 hands-on labs

### **Day 4: Docker Volumes**
*Data persistence and storage*
- [Day 4 README](Day4_Docker_Volumes/README.md)
- **Topics**: Named volumes, bind mounts, tmpfs, backup/restore
- **Project**: Data persistence across container restarts
- **Skills**: Volume management, backups, permissions
- **Exercises**: 15 hands-on labs

### **Day 5: Docker Compose**
*Multi-container orchestration*
- [Day 5 README](Day5_Docker_Compose/README.md)
- **Topics**: Service composition, dependencies, production configs
- **Project**: Web API + PostgreSQL + Redis + Nginx stack
- **Skills**: Docker Compose, multi-service debugging, scaling
- **Exercises**: 18 hands-on labs

---

## 📚 Total Curriculum

- **65+ Hands-on Exercises**
- **5 Complete C# Projects**
- **10+ Dockerfiles** (various optimization levels)
- **3 Docker Compose configurations** (dev, prod, overrides)
- **~15 integrated microservices** (API, DB, Cache, Proxy)

---

## 🎯 Learning Path

```
Start Here
    ↓
Day 1: Basics (Simple Container)
    ↓
Day 2: Images (Optimization & Security)
    ↓
Day 3: Networking (Multi-Container Communication)
    ↓
Day 4: Volumes (Data Persistence)
    ↓
Day 5: Compose (Full Stack Orchestration)
    ↓
Ready for Production Deployments!
```

---

## ✅ Prerequisites

- **Docker Desktop** (Windows/Mac) or **Docker Engine** (Linux)
- **.NET 8 SDK**
- **Text Editor or IDE** (VS Code, Visual Studio)
- **Command Line Knowledge** (basic CLI commands)
- **HTTP Client** (curl or Postman)

### Verify Installation
```bash
docker --version
docker-compose --version
dotnet --version
```

---

## 🚀 Quick Start

### Clone or Navigate to Training Directory
```bash
cd Docker_Training_Lab_Practices
```

### Run Day 1 (10 minutes)
```bash
cd Day1_Docker_Basics
docker build -t day1-app:latest .
docker run -d -p 5000:8080 day1-app:latest
curl http://localhost:5000/api/health
```

### Run Day 5 (15 minutes)
```bash
cd Day5_Docker_Compose
docker-compose up -d
docker-compose logs -f
# Visit http://localhost/api/services
```

---

## 📖 Each Day Includes

- ✅ **README.md** - Comprehensive learning guide with 10-18 exercises
- ✅ **Program.cs** - Functional ASP.NET Core application
- ✅ **Dockerfile** - Optimized container configuration
- ✅ **docker-compose.yml** - Service orchestration (Day 5)
- ✅ **.dockerignore** - Efficient build context
- ✅ **Lab Exercises** - Step-by-step instructions
- ✅ **Best Practices** - Production-ready patterns
- ✅ **Troubleshooting** - Common issues and solutions

---

## 🔧 Key Technologies

| Day | Technologies | Focus |
|-----|--------------|-------|
| Day 1 | Docker CLI, .NET 8 SDK | Container basics |
| Day 2 | Multi-stage builds, Alpine | Image optimization |
| Day 3 | Bridge networks, DNS | Inter-container comms |
| Day 4 | Named volumes, bind mounts | Data persistence |
| Day 5 | Docker Compose, PostgreSQL, Redis, Nginx | Full stack |

---

## 📋 Exercise Breakdown

### Day 1: 7 Exercises
1. Docker setup verification
2. Project build
3. Docker image creation
4. Container execution
5. Application testing
6. Container management
7. Cleanup procedures

### Day 2: 11 Exercises
1. Standard image build
2. Alpine image build
3. Image size comparison
4. Layer examination
5. Image tagging
6. Health checks
7. Security best practices
8. Build cache optimization
9. Docker Hub push (optional)
10. Registry interaction
11. Cleanup

### Day 3: 14 Exercises
1. Default network exploration
2. Image build
3. Bridge networking
4. Custom network creation
5. Multi-container connection
6. DNS resolution testing
7. Inter-container communication
8. Port mapping
9. Host network mode
10. Multi-network assignment
11. Network diagnostics
12. Environment variable configuration
13. Network disconnection
14. Cleanup

### Day 4: 15 Exercises
1. Named volume creation
2. Application build
3. Volume mounting
4. Data storage via API
5. Data persistence verification
6. Volume inspection
7. Bind mount for development
8. Volume sharing
9. Read-only volumes
10. Volume backup creation
11. Backup restoration
12. Volume cleanup
13. Tmpfs temporary storage
14. Permission management
15. Final cleanup

### Day 5: 18 Exercises
1. Docker Compose verification
2. Web API build
3. Multi-service startup
4. Service logs viewing
5. Health check verification
6. Database connectivity
7. Redis cache integration
8. Inter-service communication
9. Environment variable inspection
10. Volume management
11. Service scaling
12. Development mode
13. Production configuration
14. Service restart
15. Configuration updates
16. Log analysis
17. Container command execution
18. Complete shutdown

---

## 🎓 Learning Outcomes

After completing all 5 days, you will be able to:

### ✓ Core Docker Skills
- Build and manage Docker images
- Create and run containers with proper configuration
- Understand container lifecycle and optimization
- Implement security best practices

### ✓ Advanced Docker Skills
- Design multi-stage Dockerfiles for production
- Implement inter-container networking
- Persist and manage application data
- Use volume strategies for different scenarios
- Deploy health checks and monitoring

### ✓ Multi-Container Orchestration
- Define complex multi-service applications
- Configure service dependencies
- Implement service discovery
- Manage configuration across environments

### ✓ Best Practices
- Production-ready Docker configurations
- Security hardening techniques
- Performance optimization
- Debugging and troubleshooting

---

## 📊 Complexity Progression

```
Day 1: ⭐☆☆☆☆ (Beginner)
Day 2: ⭐⭐☆☆☆ (Beginner+)
Day 3: ⭐⭐⭐☆☆ (Intermediate)
Day 4: ⭐⭐⭐⭐☆ (Advanced)
Day 5: ⭐⭐⭐⭐⭐ (Expert)
```

---

## 💡 Tips for Instructors

### Running the Course

1. **Day 1-2**: 2-3 hours each (local development focus)
2. **Day 3-4**: 3-4 hours each (system architecture focus)
3. **Day 5**: 4-5 hours (integration and orchestration)

### Customization Options

- Modify ports to avoid conflicts: `5000 → 5000 + Day*100`
- Adjust resource limits for different hardware
- Add additional microservices for extended learning
- Integrate with CI/CD pipelines

### Extended Scenarios

- Add Kubernetes deployment (Day 6)
- Implement Docker Swarm clustering
- Add Docker registry setup
- Include security scanning
- Implement monitoring solutions

---

## 🐛 Troubleshooting Reference

### Day 1 Issues
- **Port conflicts**: Use different ports
- **Docker not found**: Check Docker Desktop installation
- **Image build fails**: Verify .NET SDK installation

### Day 2 Issues
- **Alpine image errors**: May need `apk add` for missing tools
- **Size not reduced**: Check that multi-stage build is correct
- **Permission denied**: Check user and group settings

### Day 3 Issues
- **DNS resolution fails**: Verify custom network creation
- **Port not accessible**: Check port mapping direction
- **Can't reach other container**: Ensure on same network

### Day 4 Issues
- **Data not persisting**: Verify volume is mounted correctly
- **Permission denied writing**: Check owner and permissions
- **Volume bloats**: Implement regular cleanup

### Day 5 Issues
- **Services won't start**: Check docker-compose.yml syntax
- **Database connection fails**: Verify connection string
- **Port conflicts**: Check what's using the port

---

## 📞 Support Resources

### Within Each Directory
- **README.md** - Complete exercise guide
- **inline comments** - Code documentation
- **Dockerfile comments** - Configuration explanation

### General Docker Resources
- [Docker Official Documentation](https://docs.docker.com/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [Docker Compose Reference](https://docs.docker.com/compose/compose-file/)

### Command Reference
```bash
# Common commands across all days
docker build -t name:tag .
docker run -d -p host:container image
docker ps / docker ps -a
docker logs container
docker exec -it container /bin/bash
docker stop container
docker rm container

# Networking (Day 3)
docker network ls
docker network create name
docker network inspect name

# Volumes (Day 4)
docker volume create name
docker volume ls
docker volume inspect name

# Compose (Day 5)
docker-compose up -d
docker-compose ps
docker-compose logs -f
docker-compose down -v
```

---

## 🎯 Goals for Each Learner

### SQE - Quality Assurance Goals
- Understand containerization for test environments
- Master multi-container test orchestration
- Implement proper data management for tests
- Debug and inspect container behavior
- Document reproducible test setups

### SQE - DevOps Goals
- Deploy and manage containerized applications
- Implement Docker best practices
- Configure production-ready images
- Manage multi-service infrastructure
- Monitor and troubleshoot containers

### SQE - Development Goals
- Use Docker for local development
- Share development environments
- Understand application deployment
- Implement docker-based CI/CD
- Troubleshoot containerization issues

---

## 📝 Certification Check

Upon completion, you should be able to:

- [ ] Write a Dockerfile from scratch
- [ ] Optimize images using multi-stage builds
- [ ] Configure networking between containers
- [ ] Implement persistent storage
- [ ] Create docker-compose.yml for complex stacks
- [ ] Debug Docker containers and networks
- [ ] Implement health checks
- [ ] Apply security best practices
- [ ] Scale services appropriately
- [ ] Troubleshoot common Docker issues

---

## 🔄 Recommended Study Plan

### Option 1: Sequential (1 day per week)
- Week 1: Day 1 Basics
- Week 2: Day 2 Images
- Week 3: Day 3 Networking
- Week 4: Day 4 Volumes
- Week 5: Day 5 Compose

### Option 2: Intensive (5 consecutive workdays)
- Finish entire curriculum in one week
- Each day 2-5 hours hands-on
- Final day for review and integration

### Option 3: Self-Paced
- Take as much time as needed
- Revisit exercises as needed
- Complete all 65+ exercises

---

## 📈 Progress Tracking

Each day can be validated through:

1. ✅ Successfully running all exercises
2. ✅ Creating working containers
3. ✅ Demonstrating inter-container communication
4. ✅ Proving data persistence
5. ✅ Completing full multi-container stack

---

## 🎓 Next Steps After Training

1. **Apply Knowledge**
   - Containerize existing applications
   - Create production Docker images
   - Implement in CI/CD pipelines

2. **Advanced Topics**
   - Kubernetes orchestration
   - Docker Swarm clustering
   - Docker registry management
   - Container security scanning

3. **Real-World Projects**
   - Microservices architecture
   - Container monitoring solutions
   - Infrastructure as Code (IaC)
   - GitOps workflows

---

## 📄 File Structure

```
Docker_Training_Lab_Practices/
├── Day1_Docker_Basics/
│   ├── Program.cs
│   ├── Dockerfile
│   ├── .dockerignore
│   ├── Day1_Docker_Basics.csproj
│   └── README.md
├── Day2_Docker_Images/
│   ├── Program.cs
│   ├── Dockerfile
│   ├── Dockerfile.Alpine
│   ├── .dockerignore
│   ├── Day2_Docker_Images.csproj
│   └── README.md
├── Day3_Docker_Networking/
│   ├── Program.cs
│   ├── Dockerfile
│   ├── .dockerignore
│   ├── Day3_Docker_Networking.csproj
│   └── README.md
├── Day4_Docker_Volumes/
│   ├── Program.cs
│   ├── Dockerfile
│   ├── .dockerignore
│   ├── Day4_Docker_Volumes.csproj
│   └── README.md
├── Day5_Docker_Compose/
│   ├── Program.cs
│   ├── Dockerfile.WebAPI
│   ├── docker-compose.yml
│   ├── docker-compose.override.yml
│   ├── docker-compose.prod.yml
│   ├── init-db.sql
│   ├── nginx.conf
│   ├── .dockerignore
│   ├── .env.example
│   ├── Day5_WebAPI.csproj
│   ├── README.md
│   └── TRAINING_GUIDE.md
└── README.md (this file)
```

---

## 📞 Support

For issues, questions, or feedback:
1. Check the relevant day's README
2. Review troubleshooting section
3. Refer to [Docker Documentation](https://docs.docker.com/)
4. Check Docker CLI help: `docker --help`

---

## 📜 License & Usage

These training materials are designed for:
- ✅ Internal SQE team training
- ✅ Hands-on Docker education
- ✅ Reference implementations
- ✅ CI/CD pipeline examples

---

## 🙋 Questions?

Start with the specific day's README - all materials are comprehensive and self-contained with 10-18 exercises per day.

**Happy Learning! 🐳**

---

**Last Updated**: February 2026
**Docker Version**: 24.0+
**.NET Version**: 8.0 LTS
**Complexity**: Beginner → Expert (Progressive)


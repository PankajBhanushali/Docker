# Docker Training Lab - Instructor & Participant Checklist

## 📋 Pre-Training Setup Checklist

### For Instructors

#### Environment Preparation
- [ ] Verify all 5 day projects build successfully
- [ ] Test all exercises on target platform (Windows/Mac/Linux)
- [ ] Ensure stable internet connection (for Docker pulls)
- [ ] Prepare backup Dockerfiles in case of issues
- [ ] Set up demonstration machine with proper resolution
- [ ] Create participant accounts/logins as needed
- [ ] Prepare any additional Q&A materials

#### Training Materials
- [ ] Print QUICK_REFERENCE.md cards for participants
- [ ] Review all README.md files for each day
- [ ] Prepare additional examples if needed
- [ ] Have Docker troubleshooting guide ready
- [ ] Create backup exercises for advanced students

#### System Requirements Check
- [ ] Docker Desktop/Engine version 24.0+
- [ ] .NET 8 SDK installed on all machines
- [ ] Sufficient disk space (min 10GB recommended)
- [ ] Network connectivity for image pulls
- [ ] Ports 5000-5050 available (for exercises)

### For Participants

#### Before Training Starts
- [ ] Install Docker Desktop or Docker Engine latest version
- [ ] Install .NET 8 SDK
- [ ] Install VS Code or preferred IDE
- [ ] Install curl or Postman for API testing
- [ ] Clone or download training materials
- [ ] Verify installations work: `docker --version`, `dotnet --version`
- [ ] Download required images (optional pre-warm)

---

## 🎓 Per-Day Checklist

### Day 1: Docker Basics

#### Learning Objectives Check
- [ ] Understand Docker concepts (images, containers, registry)
- [ ] Build first Docker image
- [ ] Run and manage containers
- [ ] Understand container lifecycle

#### Hands-On Exercises
- [ ] Exercise 1: Docker installation verification
- [ ] Exercise 2: Project build
- [ ] Exercise 3: Docker image creation
- [ ] Exercise 4: Container execution
- [ ] Exercise 5: Application testing
- [ ] Exercise 6: Container management
- [ ] Exercise 7: Cleanup procedures

#### Success Criteria
- [ ] All participants can build image from Dockerfile
- [ ] All participants can run and access container
- [ ] All participants understand port mapping
- [ ] All participants can view container logs

#### Estimated Time: 2-3 hours

---

### Day 2: Docker Images

#### Learning Objectives Check
- [ ] Build multi-stage Docker images
- [ ] Understand image layers and caching
- [ ] Optimize image size using Alpine Linux
- [ ] Understand image tagging and versioning
- [ ] Add metadata labels and health checks

#### Hands-On Exercises
- [ ] Exercise 1: Build standard image (full framework)
- [ ] Exercise 2: Build Alpine image (optimized)
- [ ] Exercise 3: Compare image sizes
- [ ] Exercise 4: Run Alpine container
- [ ] Exercise 5: Examine image layers
- [ ] Exercise 6: Image tagging and versioning
- [ ] Exercise 7: Health checks implementation
- [ ] Exercise 8: Security best practices
- [ ] Exercise 9: Build cache optimization
- [ ] Exercise 10: Docker Hub push (optional)
- [ ] Exercise 11: Cleanup

#### Success Criteria
- [ ] Participants understand image size differences (600MB vs 150MB)
- [ ] Participants can explain multi-stage builds
- [ ] Participants know Alpine vs full framework trade-offs
- [ ] Participants can troubleshoot build cache issues

#### Estimated Time: 3-4 hours

---

### Day 3: Docker Networking

#### Learning Objectives Check
- [ ] Understand Docker network drivers
- [ ] Create custom bridge networks
- [ ] Enable container-to-container communication
- [ ] Implement service discovery via DNS
- [ ] Understand port mapping vs exposure

#### Hands-On Exercises
- [ ] Exercise 1: Explore default networks
- [ ] Exercise 2: Build Day 3 image
- [ ] Exercise 3: Run on default bridge
- [ ] Exercise 4: Create custom network
- [ ] Exercise 5: Connect containers on custom network
- [ ] Exercise 6: Test DNS service discovery
- [ ] Exercise 7: Inter-container communication
- [ ] Exercise 8: Port mapping experiments
- [ ] Exercise 9: Host network mode
- [ ] Exercise 10: Multiple network assignment
- [ ] Exercise 11: Network diagnostics
- [ ] Exercise 12: Environment variables for discovery
- [ ] Exercise 13: Network disconnection
- [ ] Exercise 14: Cleanup

#### Success Criteria
- [ ] Participants can create custom networks
- [ ] Participants understand DNS resolution between containers
- [ ] Participants can debug network connectivity issues
- [ ] Participants know when to use different network drivers

#### Estimated Time: 3-4 hours

---

### Day 4: Docker Volumes

#### Learning Objectives Check
- [ ] Understand volume types (named, bind, tmpfs)
- [ ] Persist data across container restarts
- [ ] Share volumes between containers
- [ ] Implement backup and restore workflows
- [ ] Handle permissions and ownership

#### Hands-On Exercises
- [ ] Exercise 1: Create named volume
- [ ] Exercise 2: Build Day 4 image
- [ ] Exercise 3: Run container with named volume
- [ ] Exercise 4: Store data in volume
- [ ] Exercise 5: Verify data persistence
- [ ] Exercise 6: Inspect volume
- [ ] Exercise 7: Bind mount for development
- [ ] Exercise 8: Share volume between containers
- [ ] Exercise 9: Read-only volumes
- [ ] Exercise 10: Backup volume
- [ ] Exercise 11: Restore from backup
- [ ] Exercise 12: Volume cleanup
- [ ] Exercise 13: Tmpfs (temporary storage)
- [ ] Exercise 14: Volume permissions
- [ ] Exercise 15: Final cleanup

#### Success Criteria
- [ ] Data persists across container restarts
- [ ] Participants can create and manage volumes
- [ ] Participants understand bind mounts vs named volumes
- [ ] Participants can implement backup/restore procedures

#### Estimated Time: 3-4 hours

---

### Day 5: Docker Compose

#### Learning Objectives Check
- [ ] Define multi-container applications
- [ ] Configure service dependencies
- [ ] Implement health checks for reliability
- [ ] Manage multi-service networking
- [ ] Differentiate dev vs prod configurations

#### Hands-On Exercises
- [ ] Exercise 1: Verify Docker Compose installation
- [ ] Exercise 2: Build Day 5 application
- [ ] Exercise 3: Start all services
- [ ] Exercise 4: View service logs
- [ ] Exercise 5: Test health checks
- [ ] Exercise 6: Database connection
- [ ] Exercise 7: Redis cache integration
- [ ] Exercise 8: Inter-service communication
- [ ] Exercise 9: Environment variables
- [ ] Exercise 10: Volume management
- [ ] Exercise 11: Service scaling
- [ ] Exercise 12: Development configuration
- [ ] Exercise 13: Production configuration
- [ ] Exercise 14: Restart services
- [ ] Exercise 15: Update configuration
- [ ] Exercise 16: Logs and debugging
- [ ] Exercise 17: Execute commands
- [ ] Exercise 18: Cleanup and shutdown

#### Success Criteria
- [ ] Multi-container stack runs successfully
- [ ] All services communicate properly
- [ ] Data persists in PostgreSQL
- [ ] Cache works with Redis
- [ ] Nginx routes traffic correctly

#### Estimated Time: 4-5 hours

---

## ✅ Participant Progress Tracking

### Day 1 Completion
- [ ] Docker basics understood
- [ ] First image built successfully
- [ ] Container ran and was accessible
- [ ] Logs viewed and understood
- [ ] Container properly managed (stop/remove)

### Day 2 Completion
- [ ] Standard image built and tested
- [ ] Alpine image built and tested
- [ ] Image optimization principles understood
- [ ] Size reduction achieved (75%)
- [ ] Health checks implemented

### Day 3 Completion
- [ ] Custom network created
- [ ] Multiple containers on same network
- [ ] DNS resolution working
- [ ] Container-to-container calls successful
- [ ] Network diagnostics understood

### Day 4 Completion
- [ ] Named volumes created and mounted
- [ ] Data persisted across restarts
- [ ] Volumes shared between containers
- [ ] Backup and restore successful
- [ ] Bind mounts working for development

### Day 5 Completion
- [ ] Docker Compose file understood
- [ ] All services running together
- [ ] Inter-service communication working
- [ ] Health checks monitoring services
- [ ] Development and production configs created

---

## 🎯 Certification Criteria

### Knowledge Assessment
- [ ] Understands Docker architecture
- [ ] Knows Docker vs VMs differences
- [ ] Can explain container isolation
- [ ] Understands layered images
- [ ] Knows networking concepts

### Skill Assessment
- [ ] Can write functional Dockerfile
- [ ] Can run containers with proper configuration
- [ ] Can create and manage volumes
- [ ] Can set up multi-container networking
- [ ] Can write docker-compose.yml

### Practical Tasks
- [ ] Build custom image from scratch
- [ ] Run multi-container application
- [ ] Create persistent storage setup
- [ ] Configure service discovery
- [ ] Deploy complete stack

---

## 📊 Common Student Issues & Solutions

### Day 1 Issues
| Issue | Solution | Prevention |
|-------|----------|-----------|
| Docker not installed | Run installer again, restart system | Pre-training check |
| Image build fails | Check Dockerfile syntax, verify SDK | Validate environment |
| Port already in use | Use different port (5001, 5002) | Document port ranges |
| Cannot access container | Check port mapping direction | Explain -p host:container |

### Day 2 Issues
| Issue | Solution | Prevention |
|-------|----------|-----------|
| Alpine build fails | Install missing tools (curl, etc) | Review Alpine Linux limitations |
| Size not reduced | Verify multi-stage Dockerfile | Show layer analysis |
| Permission errors | Set proper USER in Dockerfile | Explain non-root containers |

### Day 3 Issues
| Issue | Solution | Prevention |
|-------|----------|-----------|
| DNS resolution fails | Verify network creation | Show network inspect |
| Cannot reach other container | Check both on same network | Explain service discovery |
| Port mapping confusion | Clarify host:container order | Use consistent naming |

### Day 4 Issues
| Issue | Solution | Prevention |
|-------|----------|-----------|
| Data not persisting | Check volume is mounted at /data | Show mount points |
| Permission denied | Fix owner/permissions in Dockerfile | Explain user ownership |
| Volume bloats | Implement cleanup procedures | Show volume inspection |

### Day 5 Issues
| Issue | Solution | Prevention |
|-------|----------|-----------|
| YAML syntax errors | Validate with docker-compose config | Show YAML lint |
| Services won't start | Check docker-compose logs | Explain dependencies |
| Database connection fails | Verify connection string | Show env variables |

---

## 🎬 Presentation Tips

### General Tips
- Use live demonstrations - don't just show slides
- Pause frequently to let participants catch up
- Encourage questions throughout
- Use screen recording for asynchronous learning
- Provide reference materials

### Day 1 Presentation
- Start with Docker concepts (images vs containers)
- Show visual diagram of container architecture
- Demo build process step-by-step
- Show image layers in action

### Day 2 Presentation
- Compare image sizes visually (600MB vs 150MB)
- Show build layer optimization benefits
- Explain multi-stage build process
- Demonstrate health check response

### Day 3 Presentation
- Draw network diagrams on whiteboard
- Show DNS resolution in action
- Demonstrate container communication working
- Explain port mapping vs exposure clearly

### Day 4 Presentation
- Show volume persistence live (delete container, data remains)
- Demonstrate backup/restore workflow
- Show bind mount for development
- Explain permission issues and solutions

### Day 5 Presentation
- Show full stack coming up together
- Demonstrate service dependencies working
- Show inter-service communication
- Explain production vs dev configurations

---

## 📈 Assessment Questions

### Day 1
1. What's the difference between a Docker image and a container?
2. How do you map ports when running a container?
3. What happens to container data when you stop it?

### Day 2
1. Why use Alpine Linux for images?
2. How do multi-stage builds reduce image size?
3. What's a health check and why is it important?

### Day 3
1. What's DNS service discovery?
2. How do containers communicate on custom networks?
3. What's the difference between bridge and host networks?

### Day 4
1. Name three types of volumes in Docker
2. How do you persist data between container restarts?
3. What's the difference between named volumes and bind mounts?

### Day 5
1. What does docker-compose do?
2. How do services discover each other in Compose?
3. What's the difference between docker-compose.yml and docker-compose.prod.yml?

---

## 🏆 Completion Certificate Template

```
═══════════════════════════════════════════════════════════════
             DOCKER TRAINING PROGRAM COMPLETION
═══════════════════════════════════════════════════════════════

This certifies that ___________________________

has successfully completed the

    Docker Training Lab Practices for SQEs
         (5-Day Hands-On Course in C#)

and has demonstrated proficiency in:

  ✓ Docker Basics and Container Fundamentals
  ✓ Docker Image Building and Optimization  
  ✓ Multi-Container Networking and Service Discovery
  ✓ Docker Volumes and Data Persistence
  ✓ Docker Compose Multi-Service Orchestration

Total Exercises Completed: 65+
Total Hands-On Hours: 16-20
 
Completion Date: __________________

Instructor Signature: __________________

═══════════════════════════════════════════════════════════════
```

---

## 📝 Feedback Form Template

```
DOCKER TRAINING FEEDBACK FORM

Date: ____________
Participant Name: ____________
Instructor: ____________

Overall Rating (1-5): ____

Day 1 - Docker Basics (1-5): ____
Day 2 - Docker Images (1-5): ____
Day 3 - Docker Networking (1-5): ____
Day 4 - Docker Volumes (1-5): ____
Day 5 - Docker Compose (1-5): ____

Most Valuable Topic: ________________
Least Valuable Topic: ________________

Suggested Improvements:
_________________________________
_________________________________

Additional Topics of Interest:
_________________________________
_________________________________

Would Recommend to Others? Yes / No

Comments:
_________________________________
_________________________________
```

---

## 📚 Additional Resources to Share

### Official Documentation
- Docker Official Documentation: https://docs.docker.com/
- Docker Best Practices: https://docs.docker.com/develop/
- Docker Compose Reference: https://docs.docker.com/compose/

### Interactive Learning
- Play with Docker: https://labs.play-with-docker.com/
- Docker Playground: https://www.docker.com/play

### Community Resources
- Docker Community Forums
- Stack Overflow (tag: docker)
- Docker Slack Community

---

## ✨ Tips for Success

### For Instructors
- ✅ Practice all exercises before teaching
- ✅ Have backup internet connection
- ✅ Keep a list of common errors
- ✅ Encourage experimentation
- ✅ Celebrate small wins
- ✅ Build community among participants

### For Participants
- ✅ Follow exercise steps exactly first, then experiment
- ✅ Read error messages carefully
- ✅ Google error messages - rarely unique
- ✅ Document learnings in personal notes
- ✅ Practice after training
- ✅ Help peers understand concepts

---

**Good luck with your Docker training! 🐳**

**Last Updated**: February 2026


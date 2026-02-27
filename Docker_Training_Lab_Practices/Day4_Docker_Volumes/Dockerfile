FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Day4_Docker_Volumes.csproj", "."]
RUN dotnet restore "Day4_Docker_Volumes.csproj"
COPY . .
RUN dotnet build "Day4_Docker_Volumes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Day4_Docker_Volumes.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
RUN apk add --no-cache curl tar gzip

COPY --from=publish /app/publish .

LABEL maintainer="Docker Training Team"
LABEL version="4.0.0"
LABEL description="Docker Volumes Training - Day 4"

# Create data directory for volumes
RUN mkdir -p /data/files /data/backups

# Set permissions for data directory
RUN chown -R 1000:1000 /data

# Define volume mount point
VOLUME ["/data"]

# Environment for data storage
ENV DATA_DIR=/data
ENV FILES_DIR=/data/files

RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/api/volume-info || exit 1

ENTRYPOINT ["dotnet", "Day4_Docker_Volumes.dll"]

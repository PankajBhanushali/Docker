FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Day3_Docker_Networking.csproj", "."]
RUN dotnet restore "Day3_Docker_Networking.csproj"
COPY . .
RUN dotnet build "Day3_Docker_Networking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Day3_Docker_Networking.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
RUN apk add --no-cache curl net-tools dnsmasq

COPY --from=publish /app/publish .

LABEL maintainer="Docker Training Team"
LABEL version="3.0.0"
LABEL description="Docker Networking Training - Day 3"

# Environment variables for network configuration
ENV DOCKER_NETWORK=bridge
ENV INTERNAL_PORT=8080
ENV SERVICE_DISCOVERY=dns

RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/api/network-info || exit 1

ENTRYPOINT ["dotnet", "Day3_Docker_Networking.dll"]

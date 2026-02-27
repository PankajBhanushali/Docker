# Standard multi-stage build with full framework
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Day2_Docker_Images.csproj", "."]
RUN dotnet restore "Day2_Docker_Images.csproj"
COPY . .
RUN dotnet build "Day2_Docker_Images.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Day2_Docker_Images.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

LABEL maintainer="Docker Training Team"
LABEL version="2.0.0"
LABEL description="Docker Images Training - Day 2 (Full Framework)"

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/api/image-info || exit 1

ENTRYPOINT ["dotnet", "Day2_Docker_Images.dll"]

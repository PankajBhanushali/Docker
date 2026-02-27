# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Day1_Docker_Basics.csproj", "."]
RUN dotnet restore "Day1_Docker_Basics.csproj"
COPY . .
RUN dotnet build "Day1_Docker_Basics.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Day1_Docker_Basics.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Day1_Docker_Basics.dll"]
